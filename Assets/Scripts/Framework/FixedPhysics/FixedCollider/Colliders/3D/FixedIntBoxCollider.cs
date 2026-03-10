using FixedPhysics.Bounds;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.Fixed_pointNumber.FixedIntMath;
using FixedPhysics.FixedCollider.Colliders.Types;
using UnityEngine;

namespace FixedPhysics.FixedCollider.Colliders._3D
{
    public class FixedIntBoxCollider : FixedIntCollider3D
    {
        public FixedIntVector3 Size { get; protected set; }
        
        public FixedInt HalfWidth => Size.X / 2;
        
        public FixedInt HalfHeight => Size.Y / 2;
        
        public FixedInt HalfDepth => Size.Z / 2;

        public FixedInt Rotation { get; protected set; }

#if UNITY_EDITOR
        private BoxColliderBounds _boxColliderBounds;
#endif

        public FixedIntBoxCollider(FixedIntVector3 position, FixedIntVector3 offset, FixedIntVector3 size,
            FixedInt rotation, FixedIntCollider3DType colliderType) : base(position, offset, colliderType)
        {
            Size = size;
            Rotation = rotation % 360;
            // 基类构造时 Rotation 尚未赋值，在此用正确旋转初始化 Position
#if UNITY_EDITOR
            var go = new GameObject("BoxColliderBounds");
            _boxColliderBounds = go.AddComponent<BoxColliderBounds>();
            SyncBounds(); // InitPosition 之后再 SyncBounds，确保 Gizmos 显示正确位置
#endif
        }

#if UNITY_EDITOR
        private void SyncBounds()
        {
            _boxColliderBounds?.UpdateRenderPosition(Position);
            _boxColliderBounds?.UpdateRenderSize(Size);
            _boxColliderBounds?.UpdateRenderOffset(Offset);
            _boxColliderBounds?.UpdateRenderRotation(Rotation);
        }
#endif

        public override void UpdatePosition(FixedIntVector3 newPosition)
        {
            // 将 Offset 沿 Y 轴旋转后叠加，使 offset 在碰撞体自身局部坐标系中生效
            Position = newPosition + Offset;
#if UNITY_EDITOR
            _boxColliderBounds?.UpdateRenderPosition(Position);
#endif
        }

        public override void UpdateOffset(FixedIntVector3 newOffset)
        {
            base.UpdateOffset(newOffset);
#if UNITY_EDITOR
            _boxColliderBounds?.UpdateRenderOffset(Offset);
#endif
        }

        public void UpdateSize(FixedIntVector3 size)
        {
            Size = size;
#if UNITY_EDITOR
            _boxColliderBounds?.UpdateRenderSize(size);
#endif
        }
        
        public void UpdateRotation(FixedInt rotation)
        {
            Rotation = rotation % 360;
#if UNITY_EDITOR
            _boxColliderBounds?.UpdateRenderRotation(rotation);
#endif
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
#if UNITY_EDITOR
            if (_boxColliderBounds != null)
            {
                Object.DestroyImmediate(_boxColliderBounds.gameObject);
                _boxColliderBounds = null;
            }
#endif
        }

    }
}

