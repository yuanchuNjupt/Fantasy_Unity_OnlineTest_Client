using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbControl : MonoBehaviour
{

    private Animator _animator;
    
    [SerializeField , Header("检测")] private float _detectDistance;
    [SerializeField] private LayerMask _detectionLayer;

    private RaycastHit _hit;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CharaterClimbInput();
    }

    private bool CanClimb()
    {
        return (Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward, out _hit, _detectDistance,
            _detectionLayer, QueryTriggerInteraction.Ignore));
    }

    private void CharaterClimbInput()
    {
        if(!CanClimb())
            return;
        if (GameInputManager.MainInstance.Climb && !_animator.GetCurrentAnimatorStateInfo(0).IsTag("Climb"))
        {
            //先去获取检测到的墙体信息
            var position = Vector3.zero;
            var rotation = Quaternion.LookRotation(-_hit.normal);
            position.Set(_hit.point.x , _hit.collider.bounds.center.y + _hit.collider.bounds.extents.y, _hit.point.z);

            switch (_hit.collider.tag)
            {
                case "中等高墙":
                    ToCallEvent(position, rotation);
                    _animator.CrossFade("爬中高墙" ,0f , 0 , 0f);
                    break;
                case "高墙":
                    ToCallEvent(position, rotation);
                    _animator.CrossFade("爬高墙" ,0f , 0 , 0f);
                    break;
            }
        }
    }

    private void ToCallEvent(Vector3 position, Quaternion rotation)
    {
        GameEventManager.MainInstance.CallEvent<Vector3, Quaternion>("SetAnimatorMatchInfo" ,position, rotation);
        GameEventManager.MainInstance.CallEvent<bool>("EnableCharacterGravity" , false);
    }
}
