using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders.Types;

namespace FixedPhysics.FixedCollider.Colliders._3D
{
    public class FixedIntBoxCollider : FixedIntCollider3D
    {
        public FixedIntVector3 Size { get; protected set; }
        
        public FixedInt HalfWidth => Size.X / 2;
        
        public FixedInt HalfHeight => Size.Y / 2;
        
        public FixedInt HalfDepth => Size.Z / 2;

        public FixedIntVector3 Rotation { get; protected set; }


        public FixedIntBoxCollider(FixedIntVector3 position, FixedIntVector3 offset, FixedIntVector3 size,
            FixedIntVector3 rotation, FixedIntCollider3DType colliderType) : base(position, offset, colliderType)
        {
            Size = size;
            Rotation = rotation;
        }
        
        public void UpdateSize(FixedIntVector3 size)
        {
            Size = size;
        }
        
        public void UpdateRotation(FixedIntVector3 rotation)
        {
            Rotation = rotation;
        }
    }
}