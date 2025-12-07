using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FixIntPhysics
{
    public abstract class ColliderBehaviour 
    {
        public bool Active = true;
        public ColliderType ColliderType { get; protected set; }
        public FixIntVector3 LogicPosition { get; set; }
        public FixIntVector3 XAxis { get { return FixIntVector3.right; } }
        public FixIntVector3 YAxis { get { return FixIntVector3.up; } }
        public FixIntVector3 ZAxis { get { return FixIntVector3.forward; } }
        public FixInt X { get { return LogicPosition.x; } }
        public FixInt Y { get { return LogicPosition.y; } }
        public FixInt Z { get { return LogicPosition.z; } }

        public FixIntVector3 Conter { get; protected set; }
        public FixIntVector3 Size { get; protected set; }
        public virtual void UpdateColliderInfo(FixIntVector3 pos, FixIntVector3 size=default(FixIntVector3), FixInt radius=default(FixInt))
        {
            this.LogicPosition = pos;
        }
        public virtual void UpdateColliderInfo(Vector3 pos, Vector3 size = default, float radius = default) { }
        public virtual void SetBoxData(float raduis, Vector3 conter, bool isFloowTarget = false) { }

        public virtual void SetBoxData(Vector3 conter, Vector3 size, bool isFloowTarget = false) { }

        public virtual void OnRelease()
        {

        }
    }
}