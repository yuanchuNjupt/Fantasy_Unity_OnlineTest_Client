using FixedPhysics.Bounds;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders.Types;
using UnityEngine;

namespace FixedPhysics.FixedCollider.Colliders._3D
{
    public class FixedIntCylinderCollider : FixedIntCollider3D
    {
        public FixedInt Radius { get; protected set; }
        
        public FixedInt Height { get; protected set; }
        
        public FixedInt HalfHeight => Height / 2;
        
#if UNITY_EDITOR
        private CylinderColliderBounds _cylinderColliderBounds;
#endif
        
        // 这个项目的圆柱形碰撞体不可以旋转
        public FixedIntCylinderCollider(FixedInt radius, FixedInt height, FixedIntVector3 position, FixedIntVector3 offset) : base(position, offset, FixedIntCollider3DType.AABB)
        {
            this.Radius = radius;
            this.Height = height;
#if UNITY_EDITOR
            var go = new GameObject("CylinderColliderBounds");
            _cylinderColliderBounds = go.AddComponent<CylinderColliderBounds>();
            SyncBounds();
#endif
        }

#if UNITY_EDITOR
        private void SyncBounds()
        {
            _cylinderColliderBounds?.SyncRenderData(Position, Radius, Height, Offset);
        }
#endif

        public override void UpdatePosition(FixedIntVector3 newPosition)
        {
            base.UpdatePosition(newPosition);
#if UNITY_EDITOR
            _cylinderColliderBounds?.UpdateRenderPosition(newPosition);
#endif
        }

        public override void UpdateOffset(FixedIntVector3 newOffset)
        {
            base.UpdateOffset(newOffset);
#if UNITY_EDITOR
            _cylinderColliderBounds?.UpdateRenderOffset(newOffset);
#endif
        }

        public void UpdateRadius(FixedInt radius)
        {
            this.Radius = radius;
#if UNITY_EDITOR
            _cylinderColliderBounds?.UpdateRenderRadius(radius);
#endif
        }

        public void UpdateHeight(FixedInt height)
        {
            this.Height = height;
#if UNITY_EDITOR
            _cylinderColliderBounds?.UpdateRenderHeight(height);
#endif
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
#if UNITY_EDITOR
            if (_cylinderColliderBounds != null)
            {
                Object.DestroyImmediate(_cylinderColliderBounds.gameObject);
                _cylinderColliderBounds = null;
            }
            
#endif
            
            
        }
    }
}