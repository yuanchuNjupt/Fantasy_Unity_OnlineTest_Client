using System;
using System.Collections;
using System.Collections.Generic;
using ARPG.HealthData;
using GGG.Tool;
using UnityEngine;

namespace ARPG.Health
{
    public abstract class CharacterHealthBase : MonoBehaviour
    {
        //共同都有受伤函数
        //同样都有被处决 对应的函数
        //都有格挡
        //抖音生命值信息


        [SerializeField, Header("生命值模板信息")] protected CharacterHealthInfo _healthInfo;
        protected CharacterHealthInfo _characterHealthInfo;
        
        
        
        
        
        
        protected Animator _animator;  //动画组件
        
        //这套脚本是AI和玩家共同的基类 所以要严格区分
        protected Transform _currentAttacker;  //当前的攻击者 即 当前的目标
        
        //是否能被处决 
        public bool _canBeKilled = false;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _characterHealthInfo = ScriptableObject.Instantiate(_healthInfo);
        }

        protected virtual void Start()
        {
            _characterHealthInfo.Init();
        }


        protected virtual void OnEnable()
        {
            GameEventManager.MainInstance.AddListener<float,string,string,Transform,Transform>("触发伤害" , OnCharacterHitEventHandle);
            GameEventManager.MainInstance.AddListener<string,Transform,Transform>("触发处决" , OnCharacterFinishEventHandle);
            GameEventManager.MainInstance.AddListener<float,Transform>("生成伤害" , TriggerDanamgeEventHandle);

        }

        protected virtual void OnDisable()
        {
            GameEventManager.MainInstance.RemoveListener<float,string,string,Transform,Transform>("触发伤害" , OnCharacterHitEventHandle);
            GameEventManager.MainInstance.RemoveListener<string,Transform,Transform>("触发处决" , OnCharacterFinishEventHandle);
            GameEventManager.MainInstance.RemoveListener<float,Transform>("生成伤害" , TriggerDanamgeEventHandle);


        }

        protected virtual void Update()
        {
            OnHitLookAttacker();
        }


        /// <summary>
        /// 受伤行为
        /// 虚函数 因为玩家和怪物的受伤行为不同 玩家是主动的 怪物是AI
        /// </summary>
        /// <param name="hitName">受伤</param>
        /// <param name="parryName">格挡</param>
        protected virtual void CharacterHitAction(float damage, string hitName, string parryName)
        {
            
        }

        protected void TakeDamage(float damage)
        {
            //TODO:去扣除生命值
            _characterHealthInfo.Damage(damage);
            
        }
        
        
        /// <summary>
        /// 设置当前的攻击者
        /// </summary>
        /// <param name="attackerTransform"></param>
        private void SetAttacker(Transform attackerTransform)
        {
            if(_currentAttacker == null || _currentAttacker != attackerTransform)
                _currentAttacker = attackerTransform;

        }
        //临时 : 攻击时也看向对方
        //不应该在这里攻击时看向对象 因为可能没attacker
        private void OnHitLookAttacker()
        {
            if (_currentAttacker == null || _animator.GetBool(AnimationID.Die)) return;
            if (_animator.AnimationAtTag("Hit") || _animator.AnimationAtTag("Parry")  //取消临时 : Attack
                ) //&& _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f
            {
                transform.Look(_currentAttacker.position , 40f);
            }
        }
            
        
        //此函数会在 判断自己的攻击确实攻击到了对方之后 会调用此函数
        //此函数需重点区分玩家和敌人 分别做出不同的反应
        //需要注意这里传入的两个Transform 第一个是攻击者 第二个是攻击者攻击到的敌人
        private void OnCharacterHitEventHandle(float damage, string hitName, string parryName, Transform attack , Transform self)
        {
            if(self != transform)
                return;
            //如果传进来的self不是自身 就是玩家正在攻击的人不是自己 就不要做反应
            
            //走到这 说明攻击者(attack)确实在打我
            
            //打到我的人是attack 设置attacker
            SetAttacker(attack);
            
            //执行受伤行为
            CharacterHitAction(damage, hitName, parryName);
        }


        private void OnCharacterFinishEventHandle(string hitName, Transform attacker, Transform self)
        {
            if(self != transform)
                return;
            SetAttacker(attacker);
            _animator.Play(hitName);
            _animator.SetBool(AnimationID.Die , true);
        }

        //此函数应该废弃 因为生成伤害是为了处决的时候多段伤害 
        //但是处决已经只会在生命值低于0的时候触发 所以不需要多段伤害
        //废弃
        private void TriggerDanamgeEventHandle(float damage , Transform self)
        {
            if (self != transform)
            {
                return;
            }
            TakeDamage(damage);
            GamePoolManager.MainInstance.TryGetPoolItem("HitSound", transform.position, Quaternion.identity);

        }
        
        
        
        
        
        
        
        
        
        
        
        
    }
}
