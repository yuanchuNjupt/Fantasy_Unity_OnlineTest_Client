using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders.Types;
using UnityEngine;

namespace FixedPhysics.FixedCollider.Colliders._2D
{
    public class FixedIntBoxCollider2D : FixedIntCollider2D
    {
        public FixedIntVector2 Size { get; protected set; }

        public FixedInt HalfWidth => Size.X / 2;

        public FixedInt HalfHeight => Size.Y / 2;

        public FixedInt Rotation { get; protected set; }
        
        
        

        public FixedIntBoxCollider2D(FixedIntVector2 position, FixedIntVector2 offset, FixedIntVector2 size,
            FixedInt rotation, FixedIntCollider2DType type) : base(position, offset, type)
        {
            Size = size;
            Rotation = rotation;
        }

        public void UpdateSize(FixedIntVector2 size)
        {
            Size = size;
        }
        
        public void UpdateRotation(FixedInt rotation)
        {
            Rotation = rotation;
        }
        
        
        
    }
}