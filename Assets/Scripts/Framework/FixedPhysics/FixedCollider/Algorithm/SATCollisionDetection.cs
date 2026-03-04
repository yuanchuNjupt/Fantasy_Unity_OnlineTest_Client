using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.Fixed_pointNumber.FixedIntMath;
using FixedPhysics.FixedCollider.Colliders._2D;
using FixedPhysics.FixedCollider.Colliders._3D;
using FixedPhysics.FixedCollider.Colliders.Types;

namespace FixedPhysics.FixedCollider.Algorithm
{
    /// <summary>
    /// 使用分离轴定理（SAT - Separating Axis Theorem）进行碰撞检测的算法。
    /// </summary>
    public static class SATCollisionDetection
    {
        #region 2D Collision Detection

        /// <summary>
        /// 检测两个带旋转的2D盒体碰撞器之间的碰撞（使用SAT算法）
        /// </summary>
        public static bool DetectCollision(FixedIntBoxCollider2D boxColliderA, FixedIntBoxCollider2D boxColliderB)
        {
            if (!boxColliderA.Active || !boxColliderB.Active)
                return false;

            // 如果两个盒体都没有旋转，使用更快的AABB检测
            if (boxColliderA.Rotation == 0 && boxColliderB.Rotation == 0)
            {
                return AABBCollisionDetection.DetectCollision(boxColliderA, boxColliderB);
            }

            // 获取盒体A的四个顶点
            FixedIntVector2[] verticesA = GetBoxVertices(boxColliderA);
            // 获取盒体B的四个顶点
            FixedIntVector2[] verticesB = GetBoxVertices(boxColliderB);

            // 获取盒体A的两个轴（法向量）
            FixedIntVector2[] axesA = GetBoxAxes(verticesA);
            // 获取盒体B的两个轴（法向量）
            FixedIntVector2[] axesB = GetBoxAxes(verticesB);

            // SAT算法：在所有轴上投影，如果在任何轴上没有重叠，则不碰撞
            // 检查盒体A的轴
            foreach (var axis in axesA)
            {
                if (!ProjectionsOverlap(verticesA, verticesB, axis))
                    return false;
            }

            // 检查盒体B的轴
            foreach (var axis in axesB)
            {
                if (!ProjectionsOverlap(verticesA, verticesB, axis))
                    return false;
            }

            // 所有轴上都有重叠，发生碰撞
            return true;
        }

        /// <summary>
        /// 检测带旋转的盒体碰撞器与圆形碰撞器之间的碰撞
        /// </summary>
        public static bool DetectCollision(FixedIntBoxCollider2D boxCollider, FixedIntCircleCollider2D circleCollider)
        {
            if (!boxCollider.Active || !circleCollider.Active)
                return false;

            // 如果盒体没有旋转，使用更快的AABB检测
            if (boxCollider.Rotation == 0)
            {
                return AABBCollisionDetection.DetectCollision(boxCollider, circleCollider);
            }

            // 1. 找到矩形上距离圆心最近的点
            FixedIntVector2 closestPoint = GetClosestPointOnRotatedBox(boxCollider, circleCollider.Position);

            // 2. 计算圆心到最近点的距离
            FixedInt distanceSquared = (circleCollider.Position - closestPoint).sqrMagnitude;

            // 3. 如果距离小于等于半径，则发生碰撞
            return distanceSquared <= circleCollider.Radius * circleCollider.Radius;
        }

        /// <summary>
        /// 检测圆形碰撞器与带旋转的盒体碰撞器之间的碰撞（调用顺序相反的重载）
        /// </summary>
        public static bool DetectCollision(FixedIntCircleCollider2D circleCollider, FixedIntBoxCollider2D boxCollider)
        {
            return DetectCollision(boxCollider, circleCollider);
        }

        #endregion



        #region 3D Collision Detection

        public static bool DetectCollision(FixedIntBoxCollider boxColliderA, FixedIntBoxCollider boxColliderB)
        {
            if(!boxColliderA.Active || !boxColliderB.Active)
                return false;
            
            if(boxColliderA.ColliderType is FixedIntCollider3DType.AABB && boxColliderB.ColliderType is FixedIntCollider3DType.AABB)
            {
                return AABBCollisionDetection.DetectCollision(boxColliderA, boxColliderB);
            }
            
            if(boxColliderA.ColliderType is not FixedIntCollider3DType.WithRotation && boxColliderB.ColliderType is not FixedIntCollider3DType.WithRotation)
            {
                //降维到2D + Y轴进行检测
                
                //先检测Y轴上的重叠，如果Y轴没有重叠，则不碰撞
                if (!(boxColliderA.Y + boxColliderA.HalfHeight >= boxColliderB.Y - boxColliderB.HalfHeight &&
                      boxColliderA.Y - boxColliderA.HalfHeight <= boxColliderB.Y + boxColliderB.HalfHeight))
                {
                    // Y轴没有重叠，不碰撞
                    return false;
                }
                
                
                // 获取盒体A的四个顶点
                FixedIntVector2[] verticesA = GetBoxVertices(boxColliderA);
                // 获取盒体B的四个顶点
                FixedIntVector2[] verticesB = GetBoxVertices(boxColliderB);

                // 获取盒体A的两个轴（法向量）
                FixedIntVector2[] axesA = GetBoxAxes(verticesA);
                // 获取盒体B的两个轴（法向量）
                FixedIntVector2[] axesB = GetBoxAxes(verticesB);

                // SAT算法：在所有轴上投影，如果在任何轴上没有重叠，则不碰撞
                // 检查盒体A的轴
                foreach (var axis in axesA)
                {
                    if (!ProjectionsOverlap(verticesA, verticesB, axis))
                        return false;
                }
                // 检查盒体B的轴
                foreach (var axis in axesB)
                {
                    if (!ProjectionsOverlap(verticesA, verticesB, axis))
                        return false;
                }
                // 所有轴上都有重叠，发生碰撞
                return true;
            }

            //带三维旋转盒体
            return false;

        }

        public static bool DetectCollision(FixedIntSphereCollider sphereCollider, FixedIntBoxCollider boxCollider)
        {
            return false;
        }
        
        public static bool DetectCollision(FixedIntBoxCollider boxCollider , FixedIntSphereCollider sphereCollider)
        {
            return DetectCollision(sphereCollider, boxCollider);
        }

        /// <summary>
        /// 检测带Y轴旋转的长方体碰撞器与圆柱碰撞器之间的碰撞。
        /// 算法：Y轴区间重叠检测 + XZ平面将圆柱轴心变换到Box局部坐标系后做"圆-矩形"检测。
        /// </summary>
        public static bool DetectCollision(FixedIntBoxCollider boxCollider, FixedIntCylinderCollider cylinderCollider)
        {
            if (!boxCollider.Active || !cylinderCollider.Active)
                return false;

            // 1. Y轴区间重叠检测
            FixedInt boxMinY = boxCollider.Y - boxCollider.HalfHeight;
            FixedInt boxMaxY = boxCollider.Y + boxCollider.HalfHeight;
            FixedInt cylMinY = cylinderCollider.Y - cylinderCollider.HalfHeight;
            FixedInt cylMaxY = cylinderCollider.Y + cylinderCollider.HalfHeight;
            if (boxMaxY < cylMinY || cylMaxY < boxMinY)
                return false;

            // 2. XZ平面：将圆柱轴心变换到Box的局部坐标系（反向Y轴旋转）
            FixedInt rotY = boxCollider.Rotation.Y;
            FixedInt cos = FixedIntMathf.Cos(-rotY * FixedIntMathf.Deg2Rad);
            FixedInt sin = FixedIntMathf.Sin(-rotY * FixedIntMathf.Deg2Rad);

            FixedInt dx = cylinderCollider.X - boxCollider.X;
            FixedInt dz = cylinderCollider.Z - boxCollider.Z;

            // 旋转到局部坐标系
            FixedInt localX = dx * cos - dz * sin;
            FixedInt localZ = dx * sin + dz * cos;

            // 3. Clamp到Box的半宽/半深范围，找到最近点
            FixedInt closestX = FixedIntMathf.Clamp(localX, -boxCollider.HalfWidth, boxCollider.HalfWidth);
            FixedInt closestZ = FixedIntMathf.Clamp(localZ, -boxCollider.HalfDepth, boxCollider.HalfDepth);

            // 4. 局部坐标系下圆心到最近点的距离与圆柱半径比较
            FixedInt diffX = localX - closestX;
            FixedInt diffZ = localZ - closestZ;
            return (diffX * diffX + diffZ * diffZ) <= (cylinderCollider.Radius * cylinderCollider.Radius);
        }

        /// <summary>
        /// 检测圆柱碰撞器与带Y轴旋转的长方体碰撞器之间的碰撞（调用顺序相反的重载）
        /// </summary>
        public static bool DetectCollision(FixedIntCylinderCollider cylinderCollider, FixedIntBoxCollider boxCollider)
        {
            return DetectCollision(boxCollider, cylinderCollider);
        }



        #endregion
        
        

        #region Helper Methods

        /// <summary>
        /// 获取旋转后的盒体的四个顶点
        /// </summary>
        private static FixedIntVector2[] GetBoxVertices(FixedIntBoxCollider2D boxCollider)
        {
            FixedIntVector2 center = boxCollider.Position;
            FixedInt halfWidth = boxCollider.HalfWidth;
            FixedInt halfHeight = boxCollider.HalfHeight;
            FixedInt rotation = boxCollider.Rotation;

            // 计算旋转的sin和cos值
            FixedInt cos = FixedIntMathf.Cos(rotation * FixedIntMathf.Deg2Rad);
            FixedInt sin = FixedIntMathf.Sin(rotation * FixedIntMathf.Deg2Rad);

            // 四个未旋转的顶点（相对于中心）
            FixedIntVector2[] localVertices = new FixedIntVector2[4]
            {
                new FixedIntVector2(-halfWidth, -halfHeight), // 左下
                new FixedIntVector2(halfWidth, -halfHeight),  // 右下
                new FixedIntVector2(halfWidth, halfHeight),   // 右上
                new FixedIntVector2(-halfWidth, halfHeight)   // 左上
            };

            // 旋转并转换到世界坐标
            FixedIntVector2[] worldVertices = new FixedIntVector2[4];
            for (int i = 0; i < 4; i++)
            {
                FixedInt localX = localVertices[i].X;
                FixedInt localY = localVertices[i].Y;

                // 应用旋转矩阵
                FixedInt rotatedX = localX * cos - localY * sin;
                FixedInt rotatedY = localX * sin + localY * cos;

                // 加上中心位置
                worldVertices[i] = new FixedIntVector2(center.X + rotatedX, center.Y + rotatedY);
            }

            return worldVertices;
        }

        /// <summary>
        /// 将3D碰撞器降维到2D，获取旋转后的盒体的四个顶点
        /// </summary>
        /// <param name="boxCollider"></param>
        /// <returns></returns>
        private static FixedIntVector2[] GetBoxVertices(FixedIntBoxCollider boxCollider)
        {

            if (boxCollider.ColliderType is FixedIntCollider3DType.WithRotation)
            {
                throw new System.NotImplementedException("不可将三维旋转盒体降维到二维进行检测");
            }
            
            FixedIntVector2 center = new FixedIntVector2(boxCollider.Position.X, boxCollider.Position.Z);
            FixedInt halfWidth = boxCollider.HalfWidth;
            FixedInt halfDepth = boxCollider.HalfDepth;
            FixedInt rotation = boxCollider.Rotation.Y;

            // 计算旋转的sin和cos值
            FixedInt cos = FixedIntMathf.Cos(rotation * FixedIntMathf.Deg2Rad);
            FixedInt sin = FixedIntMathf.Sin(rotation * FixedIntMathf.Deg2Rad);

            // 四个未旋转的顶点（相对于中心）
            FixedIntVector2[] localVertices = new FixedIntVector2[4]
            {
                new FixedIntVector2(-halfWidth, -halfDepth), // 左下
                new FixedIntVector2(halfWidth, -halfDepth),  // 右下
                new FixedIntVector2(halfWidth, halfDepth),   // 右上
                new FixedIntVector2(-halfWidth, halfDepth)   // 左上
            };

            // 旋转并转换到世界坐标
            FixedIntVector2[] worldVertices = new FixedIntVector2[4];
            for (int i = 0; i < 4; i++)
            {
                FixedInt localX = localVertices[i].X;
                FixedInt localZ = localVertices[i].Y; // 2D的Y对应3D的Z

                // 应用围绕Y轴的旋转矩阵
                // 标准Y轴旋转: newX = oldX * cos(θ) + oldZ * sin(θ)
                //              newZ = -oldX * sin(θ) + oldZ * cos(θ)
                FixedInt rotatedX = localX * cos + localZ * sin;
                FixedInt rotatedZ = -localX * sin + localZ * cos;

                // 加上中心位置
                worldVertices[i] = new FixedIntVector2(center.X + rotatedX, center.Y + rotatedZ);
            }

            return worldVertices;
        }
        
        
        

        /// <summary>
        /// 获取盒体的两个轴（边的法向量）
        /// SAT算法需要检测每个盒体的边的法向量
        /// 对于矩形，我们需要两个垂直方向的边
        /// </summary>
        private static FixedIntVector2[] GetBoxAxes(FixedIntVector2[] vertices)
        {
            // 计算两条垂直边的法向量
            // 边1: 从顶点0到顶点1（底边，水平方向）
            FixedIntVector2 edge1 = new FixedIntVector2(
                vertices[1].X - vertices[0].X,
                vertices[1].Y - vertices[0].Y
            );
            
            // 边2: 从顶点0到顶点3（左边，垂直方向）
            // 注意：使用顶点0到顶点3，而不是顶点1到顶点2
            // 这样保证我们得到两个垂直的边
            FixedIntVector2 edge2 = new FixedIntVector2(
                vertices[3].X - vertices[0].X,
                vertices[3].Y - vertices[0].Y
            );
            
            // 计算法向量（垂直于边）：对于2D向量(x, y)，其垂直向量为(-y, x)
            FixedIntVector2 axis1 = new FixedIntVector2(-edge1.Y, edge1.X);
            FixedIntVector2 axis2 = new FixedIntVector2(-edge2.Y, edge2.X);
            
            return new FixedIntVector2[2]
            {
                axis1,
                axis2
            };
        }

        /// <summary>
        /// 检查两组顶点在给定轴上的投影是否重叠
        /// </summary>
        private static bool ProjectionsOverlap(FixedIntVector2[] verticesA, FixedIntVector2[] verticesB, FixedIntVector2 axis)
        {
            // 投影A的顶点到轴上
            // 使用第一个顶点的投影作为初始值，避免使用MaxValue/MinValue（会溢出）
            FixedInt projection0 = Dot(verticesA[0], axis);
            FixedInt minA = projection0;
            FixedInt maxA = projection0;
            
            for (int i = 1; i < verticesA.Length; i++)
            {
                FixedInt projection = Dot(verticesA[i], axis);
                minA = FixedIntMathf.Min(minA, projection);
                maxA = FixedIntMathf.Max(maxA, projection);
            }

            // 投影B的顶点到轴上
            projection0 = Dot(verticesB[0], axis);
            FixedInt minB = projection0;
            FixedInt maxB = projection0;
            
            for (int i = 1; i < verticesB.Length; i++)
            {
                FixedInt projection = Dot(verticesB[i], axis);
                minB = FixedIntMathf.Min(minB, projection);
                maxB = FixedIntMathf.Max(maxB, projection);
            }

            // 检查投影是否重叠
            // 不重叠的条件：maxA < minB 或 maxB < minA
            // 重叠的条件：!(maxA < minB || maxB < minA)
            return !(maxA < minB || maxB < minA);
        }

        /// <summary>
        /// 找到旋转后的盒体上距离给定点最近的点
        /// </summary>
        private static FixedIntVector2 GetClosestPointOnRotatedBox(FixedIntBoxCollider2D boxCollider, FixedIntVector2 point)
        {
            // 1. 将点转换到盒体的局部坐标系（反向旋转）
            FixedIntVector2 center = boxCollider.Position;
            FixedInt rotation = -boxCollider.Rotation; // 反向旋转
            FixedInt cos = FixedIntMathf.Cos(rotation * FixedIntMathf.Deg2Rad);
            FixedInt sin = FixedIntMathf.Sin(rotation * FixedIntMathf.Deg2Rad);

            // 点相对于盒体中心的位置
            FixedInt dx = point.X - center.X;
            FixedInt dy = point.Y - center.Y;

            // 旋转到盒体的局部坐标系
            FixedInt localX = dx * cos - dy * sin;
            FixedInt localY = dx * sin + dy * cos;

            // 2. 在局部坐标系中，将点限制在盒体范围内
            FixedInt clampedX = FixedIntMathf.Clamp(localX, -boxCollider.HalfWidth, boxCollider.HalfWidth);
            FixedInt clampedY = FixedIntMathf.Clamp(localY, -boxCollider.HalfHeight, boxCollider.HalfHeight);

            // 3. 将最近点转换回世界坐标系（正向旋转）
            rotation = boxCollider.Rotation;
            cos = FixedIntMathf.Cos(rotation * FixedIntMathf.Deg2Rad);
            sin = FixedIntMathf.Sin(rotation * FixedIntMathf.Deg2Rad);

            FixedInt worldX = clampedX * cos - clampedY * sin + center.X;
            FixedInt worldY = clampedX * sin + clampedY * cos + center.Y;

            return new FixedIntVector2(worldX, worldY);
        }

        /// <summary>
        /// 计算两个向量的点积
        /// </summary>
        private static FixedInt Dot(FixedIntVector2 a, FixedIntVector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        #endregion
    }
}