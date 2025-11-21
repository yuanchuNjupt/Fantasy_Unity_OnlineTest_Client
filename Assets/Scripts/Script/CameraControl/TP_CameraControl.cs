using System;
using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using UnityEngine;

public class TP_CameraControl : MonoBehaviour
{
    
    //相机的移动速度
    [SerializeField, Header("相机参数配置")] private float _controlSpeed;  //摄像机移动速度
    [SerializeField] private Vector2 _cameraVerticalMaxAngle;  //限制相机上下最大旋转角度
    [SerializeField] private float _smoothSpeed;   //摄像机平滑速度
    [SerializeField] private float _positionOffset;   //摄像机与目标物体的距离偏移
    [SerializeField] private float _positionSmoothTime;    //摄像机位置平滑时间
    
    
    public Transform _lookTarget;
    private Vector3 _smoothDampVelocity = Vector3.zero;
    private Vector2 _input;    //相机的输入 旋转角度
    private Vector3 _cameraRotation;   //当前摄像机的旋转角度
    
    private void Update()
    {
        if(_lookTarget == null)
            return;
        CameraInput();
    }

    private void LateUpdate()
    {
        if(_lookTarget == null)
            return;
        UpdateCameraRotation();
        CameraPosition();
    }

    private void CameraInput()
    {
        _input.y += GameInputManager.MainInstance.CameraLook.x * _controlSpeed;
        _input.x -= GameInputManager.MainInstance.CameraLook.y * _controlSpeed;
        
        _input.x = Mathf.Clamp(_input.x, _cameraVerticalMaxAngle.x, _cameraVerticalMaxAngle.y);
    }
    
    //更新相机的旋转
    private void UpdateCameraRotation()
    {
        _cameraRotation = Vector3.SmoothDamp(_cameraRotation , new Vector3(_input.x, _input.y, 0), ref _smoothDampVelocity, _smoothSpeed);
        transform.eulerAngles = _cameraRotation;
    }

    private void CameraPosition()
    {
        var newPos = (_lookTarget.position + (-transform.forward * _positionOffset));
        transform.position = Vector3.Lerp(transform.position , newPos , DevelopmentToos.UnTetheredLerp(_positionSmoothTime));
    }
}
