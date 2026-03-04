using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders.Types;

namespace FixedPhysics.FixedCollider.Colliders._3D
{
    public class FixedIntCylinderCollider : FixedIntCollider3D
    {
        
        public FixedInt Radius { get; protected set; }
        
        public FixedInt Height { get; protected set; }
        
        public FixedInt HalfHeight => Height / 2;
        
        
        //这个项目的圆柱形碰撞体不可以旋转
        public FixedIntCylinderCollider(FixedInt radius , FixedInt height , FixedIntVector3 position, FixedIntVector3 offset) : base(position, offset, FixedIntCollider3DType.AABB)
        {
            this.Radius = radius;
            this.Height = height;
        }
        
        
        public void UpdateRadius(FixedInt radius)
        {
            this.Radius = radius;
        }

        public void UpdateHeight(FixedInt height)
        {
            this.Height = height;
        }
        
        
        
        
    }
}