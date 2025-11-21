using System;
using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    //最小值和最大值 偏移量
    //Layer 障碍物层级
    
    [SerializeField , Header("最大最小偏移量")] private Vector2 _maxDistanceOffset;
    [SerializeField , Header("障碍物层级") , Space(10)] private LayerMask _whatIsWall;
    [SerializeField , Header("射线长度")] private float _rayLength;
    [SerializeField , Header("平滑时间")] private float _smoothTime;
    
    
    //开始的时候 保存一下起始点和起始的偏移量
    private Vector3 _originPosition;
    private float _originOffsetDistance;
    private Transform _cameraTransform;


    private void Awake()
    {
        _cameraTransform = transform.GetChild(0);
    }

    private void Start()
    {
        // _originPosition = transform.position.normalized;
        _originPosition = new Vector3(0, 0, -1);
        _originOffsetDistance = _maxDistanceOffset.y;
    }

    private void Update()
    {
        UpdateCameraCollider();
    }

    private void UpdateCameraCollider()
    {
        var detectionDirection = transform.TransformPoint(_originPosition * _rayLength);
        if (Physics.Linecast(transform.position, detectionDirection, out var hit, _whatIsWall,
                QueryTriggerInteraction.Ignore))
        {
            //如果打到东西了
            _originOffsetDistance = Mathf.Clamp(hit.distance * 0.8f , _maxDistanceOffset.x, _maxDistanceOffset.y);
        }
        else
        {
            //没打到 默认最大值
            _originOffsetDistance = _maxDistanceOffset.y;
        
        }
        //更新相机位置
        _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition,
            _originPosition * (_originOffsetDistance - 0.1f), DevelopmentToos.UnTetheredLerp(_smoothTime));

    }
}
