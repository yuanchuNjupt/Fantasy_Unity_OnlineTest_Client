using System;
using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

public class TP_CameraControl : MonoBehaviour
{
    //相机的移动速度
    [SerializeField, Header("相机参数配置")] private float _controlSpeed;
    [SerializeField] private Vector2 _cameraVerticalMaxAngle;
    [SerializeField] private float _smoothSpeed;
    [SerializeField] private float _positionOffset;
    [SerializeField] private float _positionSmoothTime;

    /// <summary>
    /// 由外部（LobbyPlayerMouseLogicManager 或 BattlePlayerMouseLogicManager）注入
    /// 当前场景激活的 CameraLook InputAction
    /// </summary>
    private InputAction _cameraLookAction;

    public Transform _lookTarget;
    private Vector3 _smoothDampVelocity = Vector3.zero;

    [SerializeField]
    private Vector2 _input;
    private Vector3 _cameraRotation;

    /// <summary>
    /// 绑定当前激活场景的 CameraLook Action，由对应场景的 MouseLogicManager 调用
    /// </summary>
    public void BindCameraLookAction(InputAction cameraLookAction)
    {
        _cameraLookAction = cameraLookAction;
    }

    private void Start()
    {
        // 不再在 Start 中主动获取 Manager，改为外部注入
    }

    private void Update()
    {
        if (_lookTarget == null || _cameraLookAction == null)
            return;
        CameraInput();
    }

    private void LateUpdate()
    {
        if (_lookTarget == null)
            return;
        UpdateCameraRotation();
        CameraPosition();
    }

    private void CameraInput()
    {
        Vector2 look = _cameraLookAction.ReadValue<Vector2>();
        _input.y += look.x * _controlSpeed;
        _input.x -= look.y * _controlSpeed;
        _input.x = Mathf.Clamp(_input.x, _cameraVerticalMaxAngle.x, _cameraVerticalMaxAngle.y);
    }

    private void UpdateCameraRotation()
    {
        _cameraRotation = Vector3.SmoothDamp(_cameraRotation, new Vector3(_input.x, _input.y, 0), ref _smoothDampVelocity, _smoothSpeed);
        transform.eulerAngles = _cameraRotation;
    }

    private void CameraPosition()
    {
        var newPos = _lookTarget.position + (-transform.forward * _positionOffset);
        transform.position = Vector3.Lerp(transform.position, newPos, DevelopmentToos.UnTetheredLerp(_positionSmoothTime));
    }
}
