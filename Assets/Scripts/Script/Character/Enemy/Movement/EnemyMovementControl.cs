using System.Collections;
using System.Collections.Generic;
using ARPG.HealthData;
using ARPG.Movement;
using GGG.Tool;
using Script.Manager;
using UnityEngine;

namespace ARPG.Movement
{
    public class EnemyMovementControl : CharacterMovementControlBase
    {
        //1.动画的控制
        //2.移动动画播放的时候 让AI看着我们的方向
        //3.在非移动状态下 把动画控制器移动的值设置为0
        private bool _applyMovement;
        
        
        
        
        
        
        
        protected override void Start()
        {
            base.Start();
            _applyMovement = true;
        }

        protected override void Update()
        {
            base.Update();
            LooKTargetDirection();
            DrawDirection();
        }


        private void LooKTargetDirection()
        {
            if (_animator.AnimationAtTag("Motion"))
            {
                transform.Look(EnemyManager.MainInstance.GetMainPlayerTransform().position , 40f);
            }
        }



        public void SetAnimatorMoveValue(float horizontal, float vertical)
        {
            if (_applyMovement)
            {
                _animator.SetBool(AnimationID.HasInput, true);
                _animator.SetFloat(AnimationID.Lock, 1);
                _animator.SetFloat(AnimationID.Horizontal, horizontal, 0.2f, Time.deltaTime);
                _animator.SetFloat(AnimationID.Vertical, vertical, 0.2f, Time.deltaTime);
            }
            else
            {
                _animator.SetBool(AnimationID.HasInput, false);

                _animator.SetFloat(AnimationID.Lock, 0);
                _animator.SetFloat(AnimationID.Horizontal, 0, 0.2f, Time.deltaTime);
                _animator.SetFloat(AnimationID.Vertical, 0, 0.2f, Time.deltaTime);
            }
        }

        private void DrawDirection()
        {
            Debug.DrawRay(transform.position + transform.up * 0.7f ,EnemyManager.MainInstance.GetMainPlayerTransform().position - transform.position ,Color.yellow);
        }
        
        
        
        
        public void SetApplyMovement(bool value)
        {
            _applyMovement = value;
        }

        public void EnableCharacterController(bool value)
        {
            _control.enabled = value;
            SetApplyMovement(false);
        }
    }
}
