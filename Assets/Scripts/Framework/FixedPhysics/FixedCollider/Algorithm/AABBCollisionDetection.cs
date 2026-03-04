﻿using FixedPhysics.Fixed_pointNumber.Core;
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

        /// <summary>
        /// 检测两个圆柱碰撞器之间的碰撞（AABB，无旋转）
        /// XZ平面做圆与圆检测，Y轴做区间重叠检测
        /// </summary>
        public static bool DetectCollision(FixedIntCylinderCollider cylinderA, FixedIntCylinderCollider cylinderB)
        {
            if (!cylinderA.Active || !cylinderB.Active)
                return false;

            // Y轴区间重叠检测
            FixedInt aMinY = cylinderA.Y - cylinderA.HalfHeight;
            FixedInt aMaxY = cylinderA.Y + cylinderA.HalfHeight;
            FixedInt bMinY = cylinderB.Y - cylinderB.HalfHeight;
            FixedInt bMaxY = cylinderB.Y + cylinderB.HalfHeight;
            if (aMaxY < bMinY || bMaxY < aMinY)
                return false;

            // XZ平面圆圆检测
            FixedInt dx = cylinderA.X - cylinderB.X;
            FixedInt dz = cylinderA.Z - cylinderB.Z;
            FixedInt radiusSum = cylinderA.Radius + cylinderB.Radius;
            return (dx * dx + dz * dz) <= (radiusSum * radiusSum);
        }

        /// <summary>
        /// 检测圆柱碰撞器与盒体碰撞器之间的碰撞（AABB，无旋转）
        /// XZ平面做圆与矩形检测，Y轴做区间重叠检测
        /// </summary>
        public static bool DetectCollision(FixedIntCylinderCollider cylinderCollider, FixedIntBoxCollider boxCollider)
        {
            if (!cylinderCollider.Active || !boxCollider.Active)
                return false;

            // Y轴区间重叠检测
            FixedInt cylMinY = cylinderCollider.Y - cylinderCollider.HalfHeight;
            FixedInt cylMaxY = cylinderCollider.Y + cylinderCollider.HalfHeight;
            FixedInt boxMinY = boxCollider.Y - boxCollider.HalfHeight;
            FixedInt boxMaxY = boxCollider.Y + boxCollider.HalfHeight;
            if (cylMaxY < boxMinY || boxMaxY < cylMinY)
                return false;

            // XZ平面：圆心到矩形最近点距离检测
            FixedInt dx = cylinderCollider.X - boxCollider.X;
            FixedInt dz = cylinderCollider.Z - boxCollider.Z;
            FixedInt closestX = FixedIntMathf.Clamp(dx, -boxCollider.HalfWidth, boxCollider.HalfWidth);
            FixedInt closestZ = FixedIntMathf.Clamp(dz, -boxCollider.HalfDepth, boxCollider.HalfDepth);
            dx -= closestX;
            dz -= closestZ;
            return (dx * dx + dz * dz) <= (cylinderCollider.Radius * cylinderCollider.Radius);
        }

        /// <summary>
        /// 检测盒体碰撞器与圆柱碰撞器之间的碰撞（调用顺序相反的重载）
        /// </summary>
        public static bool DetectCollision(FixedIntBoxCollider boxCollider, FixedIntCylinderCollider cylinderCollider)
        {
            return DetectCollision(cylinderCollider, boxCollider);
        }

        /// <summary>
        /// 检测圆柱碰撞器与球体碰撞器之间的碰撞（AABB，无旋转）
        /// 将球体与圆柱表面的最近点距离和球半径比较
        /// </summary>
        public static bool DetectCollision(FixedIntCylinderCollider cylinderCollider, FixedIntSphereCollider sphereCollider)
        {
            if (!cylinderCollider.Active || !sphereCollider.Active)
                return false;

            // Y轴：球心到圆柱Y范围最近点的距离
            FixedInt cylMinY = cylinderCollider.Y - cylinderCollider.HalfHeight;
            FixedInt cylMaxY = cylinderCollider.Y + cylinderCollider.HalfHeight;
            FixedInt closestY = FixedIntMathf.Clamp(sphereCollider.Y, cylMinY, cylMaxY);
            FixedInt dy = sphereCollider.Y - closestY;

            // XZ平面：球心到圆柱轴的距离，再减去圆柱半径得到最近点
            FixedInt dx = sphereCollider.X - cylinderCollider.X;
            FixedInt dz = sphereCollider.Z - cylinderCollider.Z;
            FixedInt zero = new FixedInt(0);
            FixedInt xzDistSq = dx * dx + dz * dz;
            FixedInt xzDelta;
            if (xzDistSq > zero)
            {
                FixedInt xzDist = FixedIntMathf.Sqrt(xzDistSq);
                xzDelta = xzDist - cylinderCollider.Radius;
                if (xzDelta < zero) xzDelta = zero;
            }
            else
            {
                xzDelta = zero;
            }

            return (xzDelta * xzDelta + dy * dy) <= (sphereCollider.Radius * sphereCollider.Radius);
        }

        /// <summary>
        /// 检测球体碰撞器与圆柱碰撞器之间的碰撞（调用顺序相反的重载）
        /// </summary>
        public static bool DetectCollision(FixedIntSphereCollider sphereCollider, FixedIntCylinderCollider cylinderCollider)
        {
            return DetectCollision(cylinderCollider, sphereCollider);
        }

        #endregion

    }
}