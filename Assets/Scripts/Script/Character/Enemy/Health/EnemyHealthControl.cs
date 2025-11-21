using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using Script.Manager;
using UnityEngine;

namespace ARPG.Health
{
    public class EnemyHealthControl : CharacterHealthBase
    {
        protected override void Awake()
        {
            base.Awake();
            EnemyManager.MainInstance.AddEnemyUnit(gameObject);
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            GameEventManager.MainInstance.AddListener<Transform>("玩家使用重攻击攻击中了敌人" ,HeavyAttackEventHandle);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameEventManager.MainInstance.RemoveListener<Transform>("玩家使用重攻击攻击中了敌人" ,HeavyAttackEventHandle);
            
        }


        protected override void CharacterHitAction( float damage, string hitName, string parryName)
        {
            //1.先判断角色的架势条 是否大于0 
            //大于0就可以格挡
            //2.如果伤害值 大于 某个值 那么就是一个破防攻击
            //会扣除大量的架势条
            if (_animator.AnimationAtTag("Attack"))
            {
                //如果自己在攻击状态下 被攻击到了 直接受伤 并扣血   这是通用的逻辑
                _animator.Play(hitName , 0 , 0f);
                //播放音效
                GamePoolManager.MainInstance.TryGetPoolItem("HitSound", transform.position, Quaternion.identity); //?
                //直接掉血 不看架势条
                TakeDamage(damage);
                
                //这是敌人专有的 不应该放在通用逻辑里
                if (_characterHealthInfo.CurrentHealth <= 0)
                {
                    _canBeKilled = true;
                    GameEventManager.MainInstance.CallEvent("激活处决" , true);
                    

                }
                //到这里 就结束了
                return;
            }
           
            //自己有架势条 且不在攻击状态下 那么可以进行格挡
            if (_characterHealthInfo.IsHaveStrength && damage < 30f)
            {
                if (!_animator.AnimationAtTag("Attack")) //如果敌人不在攻击状态下 允许格挡
                {
                    //格挡
                    _animator.Play(parryName , 0 , 0f);
                    //播放音效
                    GamePoolManager.MainInstance.TryGetPoolItem("BlockSound", transform.position, Quaternion.identity); //?
                    
                    _characterHealthInfo.DamageToStrenght(damage);
                    
                    //如果角色的架势条已经等于0 那么就不能再进行格挡了
                    //通知当前敌人被处决了

                    // if (!_characterHealthInfo.IsHaveStrength)
                    // {
                    //     _canBeKilled = true;
                    // }
                    
                }

            }
            else
            //自己没有架势条
            {
                //受伤
                _animator.Play(hitName , 0 , 0f);
                //播放音效
                GamePoolManager.MainInstance.TryGetPoolItem("HitSound", transform.position, Quaternion.identity); //?
                TakeDamage(damage);
            }
            //当生命值低于一定值 玩家可以处决
            if (_characterHealthInfo.CurrentHealth <= 0)
            {
                _canBeKilled = true;
                GameEventManager.MainInstance.CallEvent("激活处决" , true);
                    

            }
            
            
            
            // if (damage < 30f)
            // {
            //     //说明不是破防动作 那么我们就要进行格挡或闪避
            //     //TODO:取随机值闪避或格挡
            //     
            // }
            // else
            // {
            //     //说明体力值已经 <= 0
            //     //那么执行受伤动画
            //     _animator.Play(hitName , 0 , 0f);
            //     //播放音效
            //     GamePoolManager.MainInstance.TryGetPoolItem("HitSound", transform.position, Quaternion.identity);
            //     
            //     
            // }
        }

        private void HeavyAttackEventHandle(Transform attacker)
        {
            //如果被攻击的人不是自己 那么不做处理
            if (attacker != transform)
            {
                return;
            }
            
            //如果自己被打没血了 就不要再起身 且 Die = true
            if (_characterHealthInfo.CurrentHealth <= 0)
            {
                

                _animator.SetBool(AnimationID.Die , true);
                EnemyManager.MainInstance.RemoveEnemyUnit(gameObject);

            }
        }
    }
}
