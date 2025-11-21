using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPG.ComboData.ComboData;
using GGG.Tool;

namespace ARPG.Combat
{
    public class CharacterCombatBase : MonoBehaviour
    {
        //玩家和AI 攻击事件触发是相同的
        //伤害触发是相同的
        //基础组合技是差不多的
        //组合技信息攻击是差不多的
        protected Animator _animator;
        
        [SerializeField, Header("角色组合技")] protected CharacterComboSO _baseCombo;
        [SerializeField, Header("变招")] protected CharacterComboSO _changeHeavyCombo; //轻攻击的变招
        [SerializeField, Header("处决")] protected CharacterComboSO _finishCombo; //轻攻击的变招
        [SerializeField, Header("暗杀")] protected CharacterComboSO _assassinationCombo; //轻攻击的变招

        //处决
        protected int _finishComboIndex;
        //可以处决
        protected bool _canFinish;


        [SerializeField]protected Transform _currentEnemy;

        protected CharacterComboSO _currentCombo;

        [SerializeField] protected float _animationCrossNormalTime;
        
        //需要一个当前组合技的动作索引 相当于现在在使用哪一招
        //攻击的最大间隔时间
        //是否允许输入攻击信号
   
        [SerializeField]protected int _currentComboIndex;
        protected float _maxColdTime;
        protected bool _canAttackInput;
        protected int _hitIndex;
        
        protected int _currentComboCount;
        
        protected virtual void Awake()
        {
        
            _animator = GetComponent<Animator>();
        }

       

        protected virtual void Update()
        {
            

            MatchPosition();
            LookTargetOnAttack();


        }


        protected virtual void CharacterBaseAttack()
        {
            
        }
        #region 改变组合技

        protected void ChangeComboData(CharacterComboSO data)
        {
            if (_currentCombo != data)
            {
                _currentCombo = data;
                ResetComboInfo();
            }
        }

        #endregion


        #region 位置同步

        protected virtual void MatchPosition()
        {
            if(_currentEnemy == null) return;
            if(!_animator) return;
            if (_animator.AnimationAtTag("KillSkill"))
            {
                Debug.Log("执行处决中");
                transform.rotation = Quaternion.LookRotation(-_currentEnemy.forward);
                ExecuteMatch(_finishCombo , _finishComboIndex);
            }
            else if (_animator.AnimationAtTag("BlackKill"))
            {
                transform.rotation = Quaternion.LookRotation(_currentEnemy.forward);
                ExecuteMatch(_assassinationCombo , _finishComboIndex);
            }

            // if (_animator.AnimationAtTag("Attack"))
            // {
            //     var timer = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //     if(timer >= 0.35f)
            //         return;
            //     if(DevelopmentToos.DistanceForTarget(_currentEnemy , transform) > 1.5f)
            //         return;
            //     if (!_animator.isMatchingTarget && !_animator.IsInTransition(0))
            //     {
            //         _animator.MatchTarget(_currentEnemy.position +(-transform.forward * _currentCombo.TryGetComboPositionOffset(_currentComboIndex)) ,
            //             Quaternion.identity ,AvatarTarget.Body , new MatchTargetWeightMask(Vector3.one, 0f) , 0f ,0.35f );
            //     }
            // }
      
       
        }


        protected void ExecuteMatch(CharacterComboSO data ,int index ,float starttime = 0f , float endtime = 0.03f)
        {
            if (!_animator.isMatchingTarget && !_animator.IsInTransition(0))
            {
                Debug.Log("开始处决匹配,偏差值:" + data.TryGetComboPositionOffset(index));
                _animator.MatchTarget(_currentEnemy.position +(-transform.forward * data.TryGetComboPositionOffset(index)) ,//_currentComboIndex
                    Quaternion.identity ,AvatarTarget.Body , new MatchTargetWeightMask(Vector3.one, 0f) , starttime , endtime);
           
            }
      
        }
        #endregion

        #region 攻击事件

        /// <summary>
        /// 动画事件触发的攻击事件
        /// </summary>
        private void ATK()
        {


            TriggerDamage();
            //此函数只用作更新受击应该播放的动画匹配 不用过于关心
            UpdateHitInfo();
            GamePoolManager.MainInstance.TryGetPoolItem("ATKSound", transform.position, Quaternion.identity);

        }

        #endregion
        
        #region 伤害触发

        private void TriggerDamage()
        {
            //1.要确保有目标
            //2.要确保敌人处于我们可触发伤害的距离和角度
            //3.去呼叫事件中心触发伤害
             
            // 敌人的_currentEnemy肯定是空的 不应该在这里判断 
            if(_currentEnemy == null)
                return;
            if (_animator.AnimationAtTag("Attack"))
            {
                if(Vector3.Dot(transform.forward , DevelopmentToos.DirectionForTarget(transform , _currentEnemy)) < 0.85f) return;
                if(DevelopmentToos.DistanceForTarget(_currentEnemy , transform) > 1.3f) return;
                
                
                //说明确确实实打到了
       
                GameEventManager.MainInstance.CallEvent("触发伤害" , _currentCombo.TryGetComboDamage(_currentComboIndex) ,
                    _currentCombo.TryGetOneHitname(_currentComboIndex , _hitIndex) , _currentCombo.TryGetOneParryname(_currentComboIndex ,_hitIndex) ,
                    transform , _currentEnemy);
                //玩家独有
                if(_currentCombo == _changeHeavyCombo)
                    GameEventManager.MainInstance.CallEvent("玩家使用重攻击攻击中了敌人" , _currentEnemy);
                //这里传的是单个受伤片段
            }
            else if (_animator.AnimationAtTag("KillSkill"))
            {
                //触发处决 or 暗杀
                //处决是一个完整的动作
                //会触发多次伤害
                GameEventManager.MainInstance.CallEvent("生成伤害", _finishCombo.TryGetComboDamage(_finishComboIndex) , _currentEnemy);//_currentComboIndex
            }
       
       
        }

        #endregion
        
        #region 更新连招信息

        protected virtual void UpdateComboInfo()
        {
       
            _currentComboIndex++;
            _maxColdTime = 0;
            _canAttackInput = true;
            _animator.SetBool(AnimationID.CanMove , true);

        }

        private void UpdateHitInfo()
        {
            _hitIndex++;
            if (_hitIndex == _currentCombo.TryGetHitAndParryMaxCount(_currentComboIndex))
            {
                _hitIndex = 0;
            }
        }
   
   

        #endregion
        
        #region 重置连招信息

        protected void ResetComboInfo()
        {
            _currentComboIndex = 0;
            _maxColdTime = 0;
            _hitIndex = 0;
        }
    
        //在攻击结束后 即使没打完所有连招 也要重置
        protected void OnEndAttack()
        {
            if (_animator.AnimationAtTag("Motion") && _canAttackInput)
            {
                ResetComboInfo();
                _currentComboCount = 0;
            }
        }
   
   
   
   

        #endregion
        
        #region 攻击的时候转向目标

        
        //在Update中一直调用
        protected void LookTargetOnAttack()
        {
            if(_currentEnemy == null) return;
            if(DevelopmentToos.DistanceForTarget(_currentEnemy , transform) > 5f) return;
            if (_animator.AnimationAtTag("Attack")&& _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f ) //
            {
           
                transform.Look(_currentEnemy.position , 50f);
            }
        }

        #endregion
        
        #region 动作执行

        protected virtual void ExecuteComboAttack()
        {
            //更新当前动作的HitIndex索引值
            _currentComboCount += (_currentCombo == _baseCombo) ? 1 : 0;

            _hitIndex = 0;
            if (_currentComboIndex == _currentCombo.TryGetComboMaxCount())  
            {
                //如果当前的攻击动作已经执行到最后一招;
                _currentComboIndex = 0;
                _currentComboCount = 1;
                

            }

            _maxColdTime = _currentCombo.TryGetColdTime(_currentComboIndex);
        
            //播放攻击动作
       
            _animator.CrossFadeInFixedTime(_currentCombo.TryGetOneComboAction(_currentComboIndex) , 0.15555f , 0 , 0f);
            TimerManager.MainInstance.TryGetOneTimer(_maxColdTime , UpdateComboInfo);
            _canAttackInput = false;
            _animator.SetBool(AnimationID.CanMove , false);
       
       

        }

        #endregion


    }
}
