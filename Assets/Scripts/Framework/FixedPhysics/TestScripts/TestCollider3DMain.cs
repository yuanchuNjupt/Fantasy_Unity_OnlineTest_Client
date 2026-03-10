using System;
using FixedPhysics.Bounds;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders._3D;
using FixedPhysics.FixedCollider.Colliders.Types;
using FixedPhysics.FixedCollider.Core;
using UnityEngine;

namespace Framework.FixedPhysics.TestScripts
{
    public class TestCollider3DMain : MonoBehaviour
    {
        public BoxColliderBounds BoxColliderPrefab;
        
        public CylinderColliderBounds CylinderColliderPrefab;

        private FixedIntBoxCollider _boxCollider;
        
        private FixedIntCylinderCollider _cylinderCollider;
        
        
        private void Start()
        {
            // 重置物理管理器，防止上次 Play 遗留的僵尸碰撞体干扰
            PhysicsManager3D.Instance.Reset();
            
            _boxCollider = new FixedIntBoxCollider(BoxColliderPrefab.gameObject.transform.position,
                FixedIntVector3.zero, BoxColliderPrefab.size, 0,
                FixedIntCollider3DType.OnlyYRotation);
            _cylinderCollider = new FixedIntCylinderCollider(CylinderColliderPrefab.radius ,CylinderColliderPrefab.height,CylinderColliderPrefab.gameObject.transform.position,
                  FixedIntVector3.zero);
                
            PhysicsManager3D.Instance.AddCollider3D(_boxCollider);
            PhysicsManager3D.Instance.AddCollider3D(_cylinderCollider);
            
            _boxCollider.OnCollisionEnterCallBack += OnEnter;
            _boxCollider.OnCollisionStayCallBack += OnStay;
            _boxCollider.OnCollisionExitCallBack += OnExit;
        }

        private void Update()
        {
            _boxCollider.UpdatePosition(BoxColliderPrefab.gameObject.transform.position);
            _boxCollider.UpdateRotation(BoxColliderPrefab.gameObject.transform.rotation.eulerAngles.y);
            
            _cylinderCollider.UpdatePosition(CylinderColliderPrefab.gameObject.transform.position);
            
            
            
            
            PhysicsManager3D.Instance.OnLogicFrameUpdate();
        }

        private void OnEnter(FixedIntCollider3D other)
        {
            Debug.Log("碰撞开始");
        }

        private void OnStay(FixedIntCollider3D other)
        {
            Debug.Log("碰撞中");
        }

        private void OnExit(FixedIntCollider3D other)
        {
            Debug.Log("碰撞结束");
        }

        private void OnDestroy()
        {
            if (_boxCollider != null)
            {
                PhysicsManager3D.Instance.RemoveCollider3D(_boxCollider);
                _boxCollider.OnDestroy();
                _boxCollider = null;
            }

            if (_cylinderCollider != null)
            {
                PhysicsManager3D.Instance.RemoveCollider3D(_cylinderCollider);
                _cylinderCollider.OnDestroy();
                _cylinderCollider = null;
            }
        }
    }
}