using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;

namespace FixIntPhysics
{
    public class AABBTest : MonoBehaviour
    {
        public Transform boxTrans1;
        public Transform boxTrans2;
        public FixIntBoxCollider box1;
        public FixIntBoxCollider box2;

        public Transform cylinder1;
        public Transform cylinder2;
        public FixIntCylinderCollider cylinderCollider1;
        public FixIntCylinderCollider cylinderCollider2;

        public Transform sphere1;
        public Transform sphere2;
        public FixIntSphereCollider shpereColider1;
        public FixIntSphereCollider shpereColider2;


        public void Awake()
        {
            box1 = new FixIntBoxCollider(boxTrans1.localScale,Vector3.zero);
            box2 = new FixIntBoxCollider(boxTrans2.localScale, Vector3.zero);
    
            shpereColider1 = new FixIntSphereCollider(sphere1.localScale.x,Vector3.zero);
            shpereColider2 = new FixIntSphereCollider(sphere1.localScale.x, Vector3.zero);

            cylinderCollider1 = new FixIntCylinderCollider(cylinder1.localScale.x,Vector3.zero);
            cylinderCollider2 = new FixIntCylinderCollider(cylinder2.localScale.x, Vector3.zero);

            box1.SetBoxData(Vector3.zero,boxTrans1.localScale);
            box2.SetBoxData(Vector3.zero, boxTrans2.localScale);
            shpereColider1.SetBoxData(sphere1.localScale.x, Vector3.zero);
            shpereColider2.SetBoxData(sphere1.localScale.x, Vector3.zero);
            cylinderCollider1.SetBoxData(cylinder1.localScale.x, Vector3.zero);
            cylinderCollider2.SetBoxData(cylinder2.localScale.x, Vector3.zero);
        }
        void Update()
        {
            //Update Collider Info
            box1.UpdateColliderInfo(new FixIntVector3(boxTrans1.position),new FixIntVector3(boxTrans1.localScale));
            box2.UpdateColliderInfo(new FixIntVector3(boxTrans2.position), new FixIntVector3(boxTrans2.localScale));

            shpereColider1.UpdateColliderInfo(new FixIntVector3(sphere1.position),radius:sphere1.localScale.x);
            shpereColider2.UpdateColliderInfo(new FixIntVector3(sphere2.position), radius: sphere2.localScale.x);

            cylinderCollider1.UpdateColliderInfo(new FixIntVector3(cylinder1.position), radius: cylinder1.localScale.x);
            cylinderCollider2.UpdateColliderInfo(new FixIntVector3(cylinder2.position), radius: cylinder2.localScale.x);



            bool isCollider = PhysicsManager.IsCollision(box1, box2);
            if (isCollider)
            {
                Debug.Log("box1 与   box2 发生碰撞");
            }

            bool isCollider2 = PhysicsManager.IsCollision(cylinderCollider1, box1);
            if (isCollider2)
            {
                Debug.Log("box1 与   cylinder1 发生碰撞");
            }
            bool isCollider3 = PhysicsManager.IsCollision(cylinderCollider1, cylinderCollider2);
            if (isCollider3)
            {
                Debug.Log("cylinder1 与   cylinder2 发生碰撞");
            }

            bool isCollider4 = PhysicsManager.IsCollision(shpereColider1, shpereColider2);
            if (isCollider4)
            {
                Debug.Log("sphere1 与   sphere2 发生碰撞");
            }
            bool isCollider5 = PhysicsManager.IsCollision(box1, shpereColider2);
            if (isCollider5)
            {
                Debug.Log("box1 与   sphere2 发生碰撞");
            }


            bool isCollider6 = PhysicsManager.IsCollision(cylinderCollider1, shpereColider2);
            if (isCollider6)
            {
                Debug.Log("cylinder1 与   sphere2 发生碰撞");
            }
        }
    }
}