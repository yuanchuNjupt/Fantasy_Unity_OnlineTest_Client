using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FixIntPhysics
{
    public class FixIntSphereCollider:ColliderBehaviour
    {
        /// <summary>
        /// 球形检测范围配置
        /// </summary>
        public SphereColliderGizom mSphereGizomObj;
        /// <summary>
        /// 检测半径
        /// </summary>
        public FixInt Radius { get; private set; }
        /// <summary>
        /// 是否跟随目标 若跟随，球形碰撞范围则持续跟随
        /// </summary>
        private bool mIsFloowTarget;
        public FixIntSphereCollider(FixInt radius, Vector3 conter)
        {
            this.Radius = radius;
            this.ColliderType = ColliderType.Shpere;
            this.Center = new FixIntVector3(conter);
        }
        
        public FixIntSphereCollider(FixInt radius, FixIntVector3 center)
        {
            Radius = radius;
            this.ColliderType = ColliderType.Shpere;
            this.Center = center;
        }
        
        
        /// <summary>
        /// 更新碰撞体信息
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="radius"></param>
        public override void UpdateColliderInfo(FixIntVector3 pos, FixIntVector3 size = default, FixInt radius = default)
        {
            base.UpdateColliderInfo(pos, size, radius);
            this.Radius = radius/2;
#if UNITY_EDITOR
            this.mSphereGizomObj.transform.position = pos.ToVector3();
#endif
        }
        /// <summary>
        /// 更新碰撞体信息
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="radius"></param>
        public override void UpdateColliderInfo(Vector3 pos, Vector3 size = default, float radius = default)
        {
            this.LogicPosition = new FixIntVector3(pos);
            this.Radius = new FixInt(radius)/2;
#if UNITY_EDITOR
            this.mSphereGizomObj.transform.position = pos;
#endif
        }
        /// <summary>
        /// 设置碰撞信息
        /// </summary>
        /// <param name="raduis">半径</param>
        /// <param name="center">中心偏移位置</param>
        /// <param name="isFloowTarget">是否跟随目标</param>
        public override void SetBoxData(float raduis, Vector3 center, bool isFloowTarget = false)
        {
#if UNITY_EDITOR
            if (mSphereGizomObj == null)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                mSphereGizomObj = obj.AddComponent<SphereColliderGizom>();
            }
#endif
            mIsFloowTarget = isFloowTarget;
            this.Center = new FixIntVector3(center);
            this.Radius = raduis;
#if UNITY_EDITOR
            mSphereGizomObj.SetBoxData(raduis, center, mIsFloowTarget);
#endif
        }
        
        
        public override void SetBoxData(float raduis, FixIntVector3 center, bool isFloowTarget = false)
        {
#if UNITY_EDITOR
            if (mSphereGizomObj == null)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                mSphereGizomObj = obj.AddComponent<SphereColliderGizom>();
            }
#endif
            mIsFloowTarget = isFloowTarget;
            this.Center = center;
            this.Radius = raduis;
#if UNITY_EDITOR
            mSphereGizomObj.SetBoxData(raduis, center.ToVector3(), mIsFloowTarget);
#endif
        }
        
        
        
        
        
        
        public override void OnRelease()
        {
#if UNITY_EDITOR
            if (mSphereGizomObj != null && mSphereGizomObj.gameObject != null)
            {
                GameObject.DestroyImmediate(mSphereGizomObj.gameObject);
                mSphereGizomObj = null;
            }
#endif
        }
    }
}
