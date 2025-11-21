using System.Collections;
using System.Collections.Generic;
using ARPG.Health;
using GGG.Tool;
using Script.Manager;
using UnityEngine;

namespace ARPG.Health
{
    public class PlayerHealthControl : CharacterHealthBase
    {
        protected override void Update()
        {
            base.Update();
            PlayerParryInput();
        }

        protected override void CharacterHitAction(float damage, string hitName, string parryName)
        {
            //如果玩家在处决敌人 不接受任何伤害信息
            if(_animator.AnimationAtTag("SkillKill"))
                return;
            //玩家在格挡就执行格挡动作
            if (_animator.GetBool(AnimationID.Parry) && damage < 30f && _characterHealthInfo.IsHaveStrength)
            {
                _animator.Play(parryName, 0, 0);
                GamePoolManager.MainInstance.TryGetPoolItem("BlockSound", transform.position, Quaternion.identity); //?
                _characterHealthInfo.DamageToStrenght(damage);
                Debug.Log("玩家当前血量:" + _characterHealthInfo.CurrentHealth);
                Debug.Log("玩家当前体力:" + _characterHealthInfo.CurrentStrength);

            }
            else
            {
                
               
                //受伤 播放音效 扣血
                _animator.Play(hitName ,0 ,0);
                GamePoolManager.MainInstance.TryGetPoolItem("HitSound", transform.position, Quaternion.identity); //?
                TakeDamage(damage);
                Debug.Log("玩家当前血量:" + _characterHealthInfo.CurrentHealth);
                Debug.Log("玩家当前体力:" + _characterHealthInfo.CurrentStrength);

                if (_characterHealthInfo.CurrentHealth <= 0)
                {
                    _animator.SetBool(AnimationID.Die, true);
                    Debug.Log("玩家死亡");
                    EnemyManager.MainInstance.StopAllAttackCommand();
                }
            }
        }

        private void PlayerParryInput()
        {
            if(_animator.AnimationAtTag("Hit") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f)
                return;
            if(_animator.AnimationAtTag("Attack") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f)
                return;
            _animator.SetBool(AnimationID.Parry, GameInputManager.MainInstance.Parry);
        }
    }
}
