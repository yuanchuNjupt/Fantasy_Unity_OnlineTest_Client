using System;
using System.Collections;
using System.Collections.Generic;
using ARPG.Movement;
using GGG.Tool;
using UnityEngine;

namespace ARPG.Movement
{
    public class PlayerMovementControl : CharacterMovementControlBase
    {
        private float _rotationAngle;
        private float _angleVolocity = 0f;
        [SerializeField] private float _rotationSmoothTime;

        private Transform _mainCamera;

        public Transform LockPos;

        //脚步声
        private float _nextStepTime;
        [SerializeField] private float _slowFootTime;
        [SerializeField] private float _fastFootTime;

        [SerializeField, Header("是否开启转身跑"), Space(10)]
        private bool _canTurnAndRun;


        //目标朝向
        private Vector3 _characterTargetDirection;

        public void OnStartLocalPlayer()
        {
            // _mainCamera = Camera.main.transform;
            // _mainCamera.transform.position = this.transform.position - this.transform.forward * 2f;
            // _mainCamera.transform.parent.gameObject.GetComponent<TP_CameraControl>()._lookTarget = LockPos;
            Camera.main.transform.position = transform.position + new Vector3(0, 10, -10);
            Camera.main.transform.LookAt(transform.position);
            Camera.main.transform.parent = this.transform;
        }


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            _mainCamera = Camera.main.transform;
        }

        protected override void Update()
        {
            float v = Input.GetAxis("Vertical") * Time.deltaTime;
            _control.Move(new Vector3(0, 0, v));
        }

        // private void LateUpdate()
        // {
        //
        //     CharacterRotationControl();
        //     if (_mainCamera == null)
        //     {
        //         Debug.Log("Main Camera is null");
        //     }
        //
        //     UpdateAnimator();
        // }


        private void CharacterRotationControl()
        {
            if (!_characterIsOnGround) return;

            if (_animator.GetBool(AnimationID.HasInput))
            {
                _rotationAngle =
                    Mathf.Atan2(GameInputManager.MainInstance.Movement.x, GameInputManager.MainInstance.Movement.y) *
                    Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
            }


            if (_animator.GetBool(AnimationID.HasInput) && _animator.AnimationAtTag("Motion"))
            {
                if (_canTurnAndRun)
                    _animator.SetFloat(AnimationID.DeltaAngle,
                        DevelopmentToos.GetDeltaAngle(transform, _characterTargetDirection.normalized));
                if (_canTurnAndRun)
                {
                    if (_animator.GetFloat(AnimationID.DeltaAngle) < -135f &&
                        _animator.GetBool(AnimationID.Run)) return;
                    if (_animator.GetFloat(AnimationID.DeltaAngle) > 135f && _animator.GetBool(AnimationID.Run)) return;
                }

                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, _rotationAngle,
                    ref _angleVolocity, _rotationSmoothTime);

                if (_canTurnAndRun)
                {
                    //得到我们要转到的目标方向
                    _characterTargetDirection = Quaternion.Euler(0, _rotationAngle, 0) * Vector3.forward;
                }
            }
            // if(_canTurnAndRun)
            //     _animator.SetFloat(AnimationID.DeltaAngle , DevelopmentToos.GetDeltaAngle(transform, _characterTargetDirection.normalized));
        }


        private void UpdateAnimator()
        {
            if (!_characterIsOnGround) return;

            _animator.SetBool(AnimationID.HasInput, GameInputManager.MainInstance.Movement != Vector2.zero);

            if (_animator.GetBool(AnimationID.HasInput))
            {
                if (GameInputManager.MainInstance.Run)
                {
                    _animator.SetBool(AnimationID.Run, true);
                }

                _animator.SetFloat(AnimationID.Movement,
                    (_animator.GetBool(AnimationID.Run) ? 2f : GameInputManager.MainInstance.Movement.sqrMagnitude),
                    0.25f, Time.deltaTime);
                SetCharacterSound();
            }
            else
            {
                _animator.SetFloat(AnimationID.Movement, 0f, 0.25f, Time.deltaTime);
                if (_animator.GetFloat(AnimationID.Movement) < 0.2f)
                {
                    _animator.SetBool(AnimationID.Run, false);
                }
            }
        }


        /// <summary>
        /// 脚步声
        /// </summary>
        private void SetCharacterSound()
        {
            if (_characterIsOnGround && _animator.GetFloat(AnimationID.Movement) > 0.5f &&
                _animator.AnimationAtTag("Motion"))
            {
                //在地面 在移动 确定是在Movement动画中
                _nextStepTime -= Time.deltaTime;
                if (_nextStepTime <= 0f)
                {
                    PlayFootSound();
                }
            }
            else
            {
                _nextStepTime = 0f;
            }
        }

        private void PlayFootSound()
        {
            GamePoolManager.MainInstance.TryGetPoolItem("FootSound", transform.position, Quaternion.identity);
            _nextStepTime = (_animator.GetFloat(AnimationID.Movement) > 1.1f ? _fastFootTime : _slowFootTime);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody rb;
            if (hit.transform.TryGetComponent<Rigidbody>(out rb))
            {
                rb.AddForce(transform.forward * 20f, ForceMode.Force);
            }
        }
    }
}