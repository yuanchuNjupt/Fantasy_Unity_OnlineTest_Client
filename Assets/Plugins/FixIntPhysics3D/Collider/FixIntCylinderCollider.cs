using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FixIntPhysics
{
    public class FixIntCylinderCollider : ColliderBehaviour
    {
        /// <summary>
        /// 检测半径
        /// </summary>
        public FixInt Radius { get; private set; }
        /// <summary>
        /// 初始化碰撞体信息
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="conter"></param>
        public FixIntCylinderCollider(FixInt radius, Vector3 conter)
        {
            this.Radius = radius;
            this.ColliderType = ColliderType.Cylinder;
            this.Conter = new FixIntVector3(conter);
        }
        /// <summary>
        /// 更新碰撞体信息
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="size">大小</param>
        /// <param name="radius">半径</param>
        public override void UpdateColliderInfo(FixIntVector3 pos, FixIntVector3 size = default, FixInt radius = default)
        {
            base.UpdateColliderInfo(pos, size, radius);
            this.Radius = radius / 2;
        }
    }
}