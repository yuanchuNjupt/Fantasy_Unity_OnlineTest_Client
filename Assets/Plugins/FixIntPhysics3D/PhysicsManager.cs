using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;
namespace FixIntPhysics
{
    public enum ColliderType
    {
        Box,
        Cylinder,
        Shpere,
    }
    public class PhysicsManager 
    {
        public static bool IsCollision(FixIntBoxCollider boxA, FixIntBoxCollider boxB)
        {
            if (boxA.Active==false||boxB.Active==false)
                return false;

            FixIntVector3 minA = boxA.LogicPosition + boxA.Conter - boxA.Size * 0.5f;
            FixIntVector3 maxA = boxA.LogicPosition + boxA.Conter + boxA.Size * 0.5f;
            FixIntVector3 minB = boxB.LogicPosition + boxB.Conter - boxB.Size * 0.5f;
            FixIntVector3 maxB = boxB.LogicPosition + boxB.Conter + boxB.Size * 0.5f;

            if (maxA.x < minB.x || minA.x > maxB.x)
                return false;
            if (maxA.y < minB.y || minA.y > maxB.y)
                return false;
            if (maxA.z < minB.z || minA.z > maxB.z)
                return false;

            return true;
        }
        public static bool IsCollision(FixIntBoxCollider box, FixIntSphereCollider sphere)
        {
            if (box.Active == false || sphere.Active == false)
                return false;
            FixIntVector3 minA = box.LogicPosition + box.Conter - box.Size * 0.5f;
            FixIntVector3 maxA = box.LogicPosition + box.Conter + box.Size * 0.5f;
            FixIntVector3 centerB = sphere.LogicPosition + sphere.Conter;
            FixInt radiusB = sphere.Radius;

            // 计算Sphere的最近点，即将其限制在Box的边界内
            FixIntVector3 closestPoint = new FixIntVector3(
                FixIntMath.Clamp(centerB.x, minA.x, maxA.x),
                FixIntMath.Clamp(centerB.y, minA.y, maxA.y),
                FixIntMath.Clamp(centerB.z, minA.z, maxA.z)
            );

            // 检查Sphere的最近点是否在Sphere半径内
            FixInt distanceSquared = (closestPoint - centerB).sqrMagnitude;
            return distanceSquared <= radiusB * radiusB;
        }
        public static bool IsCollision(FixIntCylinderCollider cylinder, FixIntBoxCollider box)
        {
            if (cylinder.Active == false || box.Active == false)
                return false;

            //获取两个碰撞体之间的向量
            FixIntVector3 disOffset = cylinder.LogicPosition - box.LogicPosition;
            //计算向量的投影长度
            FixInt dot_disX = FixIntVector3.Dot(disOffset, cylinder.XAxis);
            FixInt dot_disZ = FixIntVector3.Dot(disOffset, cylinder.ZAxis);
            //限制投影长度在包围盒内
            FixInt clamp_x = FixIntMath.Clamp(dot_disX, -box.Size.x / 2, box.Size.x / 2);
            FixInt clamp_z = FixIntMath.Clamp(dot_disZ, -box.Size.z / 2, box.Size.z / 2);
            //计算轴向上的投影向量
            FixIntVector3 s_x = clamp_x * cylinder.XAxis;
            FixIntVector3 s_z = clamp_z * cylinder.ZAxis;
            //计算表面最近的接触点:碰撞中心位置+轴向偏移
            FixIntVector3 point = box.LogicPosition;
            point += s_x;
            point += s_z;

            FixIntVector3 po = cylinder.LogicPosition - point;
            po.y = 0;
            //判断中心点O到P点的距离如果>半径 则说明没有产生碰撞
            if (FixIntVector3.SqrMagnitude(po) > cylinder.Radius * cylinder.Radius)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsCollision(FixIntCylinderCollider collider1, FixIntCylinderCollider collider2)
        {
            if (collider1.Active == false || collider2.Active == false)
                return false;

            FixIntVector3 dir = collider1.LogicPosition - collider2.LogicPosition;
            //如果两个圆形碰撞之间的向量>两个圆形物体半径相加 说明没有碰撞
            if (FixIntVector3.SqrMagnitude(dir) > (collider1.Radius + collider2.Radius) * (collider1.Radius + collider2.Radius))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IsCollision(FixIntCylinderCollider collider1, FixIntSphereCollider collider2)
        {
            if (collider1.Active == false || collider2.Active == false)
                return false;
            FixIntVector3 dir = collider1.LogicPosition - collider2.LogicPosition;
            //如果两个圆形碰撞之间的向量>两个圆形物体半径相加 说明没有碰撞
            if (FixIntVector3.SqrMagnitude(dir) > (collider1.Radius + collider2.Radius) * (collider1.Radius + collider2.Radius))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsCollision(FixIntSphereCollider collider1, FixIntSphereCollider collider2)
        {
            if (collider1.Active == false || collider2.Active == false)
                return false;
            FixIntVector3 dir = collider1.LogicPosition - collider2.LogicPosition;
            //如果两个圆形碰撞之间的向量>两个圆形物体半径相加 说明没有碰撞
            if (FixIntVector3.SqrMagnitude(dir) > (collider1.Radius + collider2.Radius) * (collider1.Radius + collider2.Radius))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}