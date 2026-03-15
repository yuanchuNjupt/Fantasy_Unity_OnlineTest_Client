using System;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders._2D;
using FixedPhysics.FixedCollider.Colliders.Types;

namespace FixedPhysics.FixedCollider.Colliders._3D
{
    public class FixedIntCollider3D
    {
        
        public FixedIntVector3 Position { get; protected set; }
        
        public FixedIntVector3 Offset { get; private set; }
        
        public readonly FixedIntCollider3DType ColliderType;
        
        public bool Active { get; private set; }
        
        public FixedInt X => Position.X;
        public FixedInt Y => Position.Y;
        public FixedInt Z => Position.Z;
        
        public FixedIntCollider3D(FixedIntVector3 position, FixedIntVector3 offset , FixedIntCollider3DType colliderType)
        {
            Offset = offset;
            Position = position + offset;
            
            ColliderType = colliderType;
            Active = true; // 默认激活碰撞体
        }
        
        
        public virtual void UpdatePosition(FixedIntVector3 newPosition)
        {
            Position = newPosition + Offset;
        }
        
        public virtual void UpdateOffset(FixedIntVector3 newOffset)
        {
            Offset = newOffset;
        }
        
        public void SetActive(bool active)
        {
            Active = active;
        }

        #region 生命周期回调

        /// <summary>
        /// 碰撞开始时触发（第一次检测到碰撞）
        /// </summary>
        public Action<FixedIntCollider3D> OnCollisionEnterCallBack;
        
        /// <summary>
        /// 碰撞持续时触发（每帧都在碰撞中）
        /// </summary>
        public Action<FixedIntCollider3D> OnCollisionStayCallBack;
        
        /// <summary>
        /// 碰撞结束时触发（不再碰撞）
        /// </summary>
        public Action<FixedIntCollider3D> OnCollisionExitCallBack;
        
        /// <summary>
        /// 触发碰撞进入回调
        /// </summary>
        public void TriggerOnCollisionEnter(FixedIntCollider3D other)
        {
            OnCollisionEnterCallBack?.Invoke(other);
        }
        
        /// <summary>
        /// 触发碰撞保持回调
        /// </summary>
        public void TriggerOnCollisionStay(FixedIntCollider3D other)
        {
            OnCollisionStayCallBack?.Invoke(other);
        }
        
        /// <summary>
        /// 触发碰撞退出回调
        /// </summary>
        public void TriggerOnCollisionExit(FixedIntCollider3D other)
        {
            OnCollisionExitCallBack?.Invoke(other);
        }
        
        #endregion      
        
        public virtual void OnDestroy()
        {
            // 清理回调，避免内存泄漏
            OnCollisionEnterCallBack = null;
            OnCollisionStayCallBack = null;
            OnCollisionExitCallBack = null;
        }
        
        
        
    }
}