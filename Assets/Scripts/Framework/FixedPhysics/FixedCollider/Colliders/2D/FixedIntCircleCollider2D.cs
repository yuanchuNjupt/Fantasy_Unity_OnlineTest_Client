using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders.Types;

namespace FixedPhysics.FixedCollider.Colliders._2D
{
    public class FixedIntCircleCollider2D : FixedIntCollider2D
    {
        public FixedInt Radius { get; protected set; }

        public FixedIntCircleCollider2D(FixedIntVector2 position, FixedIntVector2 offset, FixedInt radius
        ) : base(position, offset, FixedIntCollider2DType.AABB)
        {
            Radius = radius;
        }
        
        public void UpdateRadius(FixedInt radius)
        {
            Radius = radius;
        }
    }
}