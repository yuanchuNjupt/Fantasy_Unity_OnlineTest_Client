using FixedPhysics.Bounds;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders.Types;
using UnityEngine;

namespace FixedPhysics.FixedCollider.Colliders._3D
{
    public class FixedIntSphereCollider : FixedIntCollider3D
    {
        public FixedInt Radius { get; protected set; }

#if UNITY_EDITOR
        private SphereColliderBounds _sphereColliderBounds;
#endif

        public FixedIntSphereCollider(FixedIntVector3 position, FixedIntVector3 offset, FixedInt radius) : base(position, offset, FixedIntCollider3DType.AABB)
        {
            Radius = radius;
#if UNITY_EDITOR
            var go = new GameObject("SphereColliderBounds");
            _sphereColliderBounds = go.AddComponent<SphereColliderBounds>();
            SyncBounds();
#endif
        }

#if UNITY_EDITOR
        private void SyncBounds()
        {
            _sphereColliderBounds?.UpdateRenderPosition(Position);
            _sphereColliderBounds?.UpdateRenderRadius(Radius);
            _sphereColliderBounds?.UpdateRenderOffset(Offset);
        }
#endif

        public override void UpdatePosition(FixedIntVector3 newPosition)
        {
            base.UpdatePosition(newPosition);
#if UNITY_EDITOR
            _sphereColliderBounds?.UpdateRenderPosition(Position);
#endif
        }

        public override void UpdateOffset(FixedIntVector3 newOffset)
        {
            base.UpdateOffset(newOffset);
#if UNITY_EDITOR
            _sphereColliderBounds?.UpdateRenderOffset(Offset);
#endif
        }

        public void UpdateRadius(FixedInt radius)
        {
            Radius = radius;
#if UNITY_EDITOR
            _sphereColliderBounds?.UpdateRenderRadius(radius);
#endif
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
#if UNITY_EDITOR
            if (_sphereColliderBounds != null)
            {
                Object.DestroyImmediate(_sphereColliderBounds.gameObject);
                _sphereColliderBounds = null;
            }
#endif
            
            
        }
    }
}