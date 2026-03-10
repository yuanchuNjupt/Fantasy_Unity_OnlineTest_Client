using FixedPhysics.Bounds;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders._2D;
using FixedPhysics.FixedCollider.Colliders._3D;
using FixedPhysics.FixedCollider.Colliders.Types;
using FixedPhysics.FixedCollider.Core;
using UnityEngine;

namespace Framework.FixedPhysics.TestScripts
{
    public class TestCollider2DMain : MonoBehaviour
    {
        public BoxCollider2D boxCollider2D;

        public CircleCollider2D circleCollider2D;

        private FixedIntBoxCollider2D _fixedIntBoxCollider2D;

        private FixedIntCircleCollider2D _fixedIntCircleCollider2D;


        private void Start()
        {
            // 重置物理管理器，防止上次 Play 遗留的僵尸碰撞体干扰
            PhysicsManager2D.Instance.Reset();

            _fixedIntBoxCollider2D = new FixedIntBoxCollider2D(boxCollider2D.gameObject.transform.position,
                FixedIntVector2.zero, boxCollider2D.size, boxCollider2D.gameObject.transform.rotation.eulerAngles.z,
                FixedIntCollider2DType.WithRotation);


            _fixedIntCircleCollider2D = new FixedIntCircleCollider2D(circleCollider2D.gameObject.transform.position,
                FixedIntVector2.zero, circleCollider2D.radius);


            PhysicsManager2D.Instance.AddCollider2D(_fixedIntBoxCollider2D);
            PhysicsManager2D.Instance.AddCollider2D(_fixedIntCircleCollider2D);

            _fixedIntBoxCollider2D.OnCollisionEnter2DCallBack += OnEnter;
            _fixedIntBoxCollider2D.OnCollisionStay2DCallBack += OnStay;
            _fixedIntBoxCollider2D.OnCollisionExit2DCallBack += OnExit;
        }

        private void Update()
        {
            _fixedIntBoxCollider2D.UpdatePosition(boxCollider2D.gameObject.transform.position);
            _fixedIntBoxCollider2D.UpdateRotation(boxCollider2D.gameObject.transform.rotation.eulerAngles.z);

            _fixedIntCircleCollider2D.UpdatePosition(circleCollider2D.gameObject.transform.position);


            PhysicsManager2D.Instance.OnLogicFrameUpdate();
        }

        private void OnEnter(FixedIntCollider2D other)
        {
            Debug.Log("碰撞开始");
        }

        private void OnStay(FixedIntCollider2D other)
        {
            Debug.Log("碰撞中");
        }

        private void OnExit(FixedIntCollider2D other)
        {
            Debug.Log("碰撞结束");
        }

        private void OnDestroy()
        {
            if (_fixedIntBoxCollider2D != null)
            {
                PhysicsManager2D.Instance.RemoveCollider2D(_fixedIntBoxCollider2D);
                _fixedIntBoxCollider2D = null;
            }

            if (_fixedIntCircleCollider2D != null)
            {
                PhysicsManager2D.Instance.RemoveCollider2D(_fixedIntCircleCollider2D);
                _fixedIntCircleCollider2D = null;
            }
        }
    }
}