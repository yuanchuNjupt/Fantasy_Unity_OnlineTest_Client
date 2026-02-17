using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders.Types;

namespace FixedPhysics.FixedCollider.Colliders._3D
{
    public class FixedIntSphereCollider : FixedIntCollider3D
    {
        public FixedInt Radius { get; protected set; }
        
        public FixedIntSphereCollider(FixedIntVector3 position, FixedIntVector3 offset, FixedInt radius) : base(position, offset, FixedIntCollider3DType.AABB)
        {
            Radius = radius;
        }
        
        public void UpdateRadius(FixedInt radius)
        {
            Radius = radius;
        }
    }
}