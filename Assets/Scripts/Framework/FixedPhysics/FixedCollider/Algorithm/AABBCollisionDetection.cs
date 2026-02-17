using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.Fixed_pointNumber.FixedIntMath;
using FixedPhysics.FixedCollider.Colliders._2D;
using FixedPhysics.FixedCollider.Colliders._3D;

namespace FixedPhysics.FixedCollider.Algorithm
{
    
    /// <summary>
    /// 使用轴对齐包围盒（AABB）进行碰撞检测的算法。适用于不旋转矩形(长方体)和圆形(球体)碰撞体的检测。
    /// </summary>
    public static class AABBCollisionDetection
    {
        #region 2D Collision Detection
        
        public static bool DetectCollision(FixedIntBoxCollider2D boxColliderA, FixedIntBoxCollider2D boxColliderB)
        {
            if (!boxColliderA.Active || !boxColliderB.Active)
                return false;
            return boxColliderA.X + boxColliderA.HalfWidth >= boxColliderB.X - boxColliderB.HalfWidth &&
                   boxColliderA.X - boxColliderA.HalfWidth <= boxColliderB.X + boxColliderB.HalfWidth &&
                   boxColliderA.Y + boxColliderA.HalfHeight >= boxColliderB.Y - boxColliderB.HalfHeight &&
                   boxColliderA.Y - boxColliderA.HalfHeight <= boxColliderB.Y + boxColliderB.HalfHeight;
        }
        
        public static bool DetectCollision(FixedIntCircleCollider2D circleColliderA , FixedIntCircleCollider2D circleColliderB)
        {
            if (!circleColliderA.Active || !circleColliderB.Active)
                return false;
            
            //判断半径即可
            return (circleColliderA.Position - circleColliderB.Position).sqrMagnitude <= FixedIntMathf.Pow(circleColliderA.Radius + circleColliderB.Radius, 2);
        }

        public static bool DetectCollision(FixedIntBoxCollider2D boxCollider, FixedIntCircleCollider2D circleCollider)
        {
            if (!boxCollider.Active || !circleCollider.Active)
                return false;

            //找到圆心在矩形坐标系中的位置
            FixedInt deltaX = circleCollider.X - boxCollider.X;
            FixedInt deltaY = circleCollider.Y - boxCollider.Y;

            //将圆心位置限制在矩形范围内
            FixedInt closestX = FixedIntMathf.Clamp(deltaX, -boxCollider.HalfWidth, boxCollider.HalfWidth);
            FixedInt closestY = FixedIntMathf.Clamp(deltaY, -boxCollider.HalfHeight, boxCollider.HalfHeight);

            //计算圆心与最近点之间的距离
            deltaX -= closestX;
            deltaY -= closestY;

            //如果距离小于等于半径，则发生碰撞
            return (deltaX * deltaX + deltaY * deltaY) <= (circleCollider.Radius * circleCollider.Radius);
        }

        public static bool DetectCollision(FixedIntCircleCollider2D circleCollider, FixedIntBoxCollider2D boxCollider)
        {
            return DetectCollision(boxCollider, circleCollider);
        }
        
        #endregion

        #region 3D Collision Detection

        /// <summary>
        /// 检测两个3D盒体碰撞器之间的碰撞
        /// </summary>
        public static bool DetectCollision(FixedIntBoxCollider boxColliderA, FixedIntBoxCollider boxColliderB)
        {
            if (!boxColliderA.Active || !boxColliderB.Active)
                return false;
            
            // AABB 3D碰撞检测：检查三个轴上的重叠
            return boxColliderA.X + boxColliderA.HalfWidth >= boxColliderB.X - boxColliderB.HalfWidth &&
                   boxColliderA.X - boxColliderA.HalfWidth <= boxColliderB.X + boxColliderB.HalfWidth &&
                   boxColliderA.Y + boxColliderA.HalfHeight >= boxColliderB.Y - boxColliderB.HalfHeight &&
                   boxColliderA.Y - boxColliderA.HalfHeight <= boxColliderB.Y + boxColliderB.HalfHeight &&
                   boxColliderA.Z + boxColliderA.HalfDepth >= boxColliderB.Z - boxColliderB.HalfDepth &&
                   boxColliderA.Z - boxColliderA.HalfDepth <= boxColliderB.Z + boxColliderB.HalfDepth;
        }

        /// <summary>
        /// 检测两个球体碰撞器之间的碰撞
        /// </summary>
        public static bool DetectCollision(FixedIntSphereCollider sphereColliderA, FixedIntSphereCollider sphereColliderB)
        {
            if (!sphereColliderA.Active || !sphereColliderB.Active)
                return false;
            
            // 判断两个球体的距离是否小于半径之和
            return (sphereColliderA.Position - sphereColliderB.Position).sqrMagnitude <= 
                   FixedIntMathf.Pow(sphereColliderA.Radius + sphereColliderB.Radius, 2);
        }

        /// <summary>
        /// 检测盒体碰撞器与球体碰撞器之间的碰撞
        /// </summary>
        public static bool DetectCollision(FixedIntBoxCollider boxCollider, FixedIntSphereCollider sphereCollider)
        {
            if (!boxCollider.Active || !sphereCollider.Active)
                return false;

            // 找到球心在盒体坐标系中的位置
            FixedInt deltaX = sphereCollider.X - boxCollider.X;
            FixedInt deltaY = sphereCollider.Y - boxCollider.Y;
            FixedInt deltaZ = sphereCollider.Z - boxCollider.Z;

            // 将球心位置限制在盒体范围内，找到盒体上最接近球心的点
            FixedInt closestX = FixedIntMathf.Clamp(deltaX, -boxCollider.HalfWidth, boxCollider.HalfWidth);
            FixedInt closestY = FixedIntMathf.Clamp(deltaY, -boxCollider.HalfHeight, boxCollider.HalfHeight);
            FixedInt closestZ = FixedIntMathf.Clamp(deltaZ, -boxCollider.HalfDepth, boxCollider.HalfDepth);

            // 计算球心与最近点之间的距离
            deltaX -= closestX;
            deltaY -= closestY;
            deltaZ -= closestZ;

            // 如果距离小于等于半径，则发生碰撞
            return (deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ) <= (sphereCollider.Radius * sphereCollider.Radius);
        }

        /// <summary>
        /// 检测球体碰撞器与盒体碰撞器之间的碰撞（调用顺序相反的重载）
        /// </summary>
        public static bool DetectCollision(FixedIntSphereCollider sphereCollider, FixedIntBoxCollider boxCollider)
        {
            return DetectCollision(boxCollider, sphereCollider);
        }

        #endregion

    }
}