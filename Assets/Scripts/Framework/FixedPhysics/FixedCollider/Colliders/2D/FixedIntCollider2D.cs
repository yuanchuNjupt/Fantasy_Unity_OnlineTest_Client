using System;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders.Types;
using UnityEngine;

namespace FixedPhysics.FixedCollider.Colliders._2D
{
    public class FixedIntCollider2D
    {
        public FixedIntVector2 Position { get; protected set; }
        
        public FixedIntVector2 Offset { get; protected set; }

        public FixedIntCollider2DType ColliderType { get; protected set; }
        
        
        public bool Active { get; private set; }
        
        public FixedInt X => Position.X;
        
        public FixedInt Y => Position.Y;
        
        protected bool UseAdjustPos { get; private set; }
        
        public bool CanAdjust {get; private set;}

        private FixedIntVector2 _adjustPos;
        
        public FixedIntVector2 AdjustPos
        {
            get
            {
                CanAdjust = false;
                return _adjustPos;
            }
            set
            {
                CanAdjust = true;
                _adjustPos = value;
            }
            
            
        }


        public FixedIntCollider2D(FixedIntVector2 position, FixedIntVector2 offset , FixedIntCollider2DType colliderType)
        {
            Position = position + offset;
            Offset = offset;
            ColliderType = colliderType;
            Active = true; // 默认激活碰撞体
        }
        
        public void UpdatePosition(FixedIntVector2 newPosition)
        {
            Position = newPosition + Offset;
        }

        public void UpdateOffset(FixedIntVector2 newOffset)
        {
            Offset = newOffset;
        }
        
        public void SetActive(bool active)
        {
            Active = active;
        }
        
        public void SetUseAdjustPos(bool use)
        {
            UseAdjustPos = use;
        }

        #region 生命周期回调

        /// <summary>
        /// 碰撞开始时触发（第一次检测到碰撞）
        /// </summary>
        public Action<FixedIntCollider2D> OnCollisionEnter2DCallBack;
        
        /// <summary>
        /// 碰撞持续时触发（每帧都在碰撞中）
        /// </summary>
        public Action<FixedIntCollider2D> OnCollisionStay2DCallBack;
        
        /// <summary>
        /// 碰撞结束时触发（不再碰撞）
        /// </summary>
        public Action<FixedIntCollider2D> OnCollisionExit2DCallBack;
        
        /// <summary>
        /// 触发碰撞进入回调
        /// </summary>
        public void TriggerOnCollisionEnter(FixedIntCollider2D other)
        {
            OnCollisionEnter2DCallBack?.Invoke(other);
        }
        
        /// <summary>
        /// 触发碰撞保持回调
        /// </summary>
        public void TriggerOnCollisionStay(FixedIntCollider2D other)
        {
            OnCollisionStay2DCallBack?.Invoke(other);
        }
        
        /// <summary>
        /// 触发碰撞退出回调
        /// </summary>
        public void TriggerOnCollisionExit(FixedIntCollider2D other)
        {
            OnCollisionExit2DCallBack?.Invoke(other);
        }
        
        #endregion
        
        
    }
}