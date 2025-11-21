using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using Script.Manager;
using UnityEngine;

namespace ARPG.Combat
{
    public class EnemyCombatControl : CharacterCombatBase
    {
        
        //AI 的攻击指令是由AI管理器指派的 而非自身行为
        //AI 在收到攻击指令 需判断自身的情况来决定 是否接受这个指令
        //玩家不希望AI去接受
        [SerializeField]private bool _attackCommand; //攻击指令


        protected override void Awake()
        {
            base.Awake();
            //敌人的敌人就是玩家 不会改变
            _currentEnemy = EnemyManager.MainInstance.GetMainPlayerTransform();
        }

        private void Start()
        {
            _canAttackInput = true;
        }
        
        //当AI接受指令 但被玩家攻击了
        //放弃攻击指令


        /// <summary>
        /// 检查AI当前自身状况是否允许接受指令
        /// </summary>
        /// <returns></returns>
        private bool CheckAIState()
        {
            if (_animator.AnimationAtTag("Hit")) return false;
            if (_animator.AnimationAtTag("Parry")) return false;
            if (_animator.AnimationAtTag("KillSkillHit")) return false;
            if(_animator.AnimationAtTag("Attack")) return false;
            return true;
        }

        public void AIBasedAttack()
        {
            if(!CheckAIState()) return;
            ChangeComboData(_baseCombo);
            ExecuteComboAttack();
        }

        // protected override void UpdateComboInfo()
        // {
        //     base.UpdateComboInfo();
        // }


        private void ResetAttackCommand()
        {
            _attackCommand = false;
        }
        
        /// <summary>
        /// 获取AI的攻击指令
        /// </summary>
        /// <returns></returns>
        public bool GetAttackCommand() => _attackCommand;

        public void SetAttackCommand(bool value)
        {
            //判断自身情况是否允许接受攻击指令
            if (!CheckAIState())
            {
                ResetAttackCommand();
                return;
            }
            _attackCommand = value;
        }

        public void StopAllAction()
        {
            if (_attackCommand)
            {
                ResetAttackCommand();
            }

            if (_animator.AnimationAtTag("Attack"))
            {
                _animator.Play("Idle" ,0 ,0f);
                
            }
            
            
        }

        protected override void ExecuteComboAttack()
        {
            //更新当前动作的HitIndex索引值
            _currentComboCount += (_currentCombo == _baseCombo) ? 1 : 0;

            _hitIndex = 0;
           
            if (_currentComboIndex == _currentCombo.TryGetComboMaxCount())  
            {
                //如果当前的攻击动作已经执行到最后一招;
                _currentComboIndex = 0;
                _currentComboCount = 1;
                ResetAttackCommand();  //?
                

            }

            _maxColdTime = _currentCombo.TryGetColdTime(_currentComboIndex);
        
            //播放攻击动作
       
            _animator.CrossFadeInFixedTime(_currentCombo.TryGetOneComboAction(_currentComboIndex) , 0.15555f , 0 , 0f);
            TimerManager.MainInstance.TryGetOneTimer(_maxColdTime , UpdateComboInfo);
            ResetAttackCommand();//?
            Debug.Log(_currentComboIndex);
            //这些都是玩家需要关心的 敌人不用
            // _canAttackInput = false;
            // _animator.SetBool(AnimationID.CanMove , false);
        }
    }
}
