using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using Fantasy;
using Helper;
using Lobby;
using UnityEngine;
using UnityEngine.InputSystem;


public enum PlayerType
{
    Self,
    Other,
}

public enum PLayerState
{
    Idle,
    Run,
}


public class LobbyPlayer : MonoBehaviour
{
    
    public PlayerType playerType;
    
    public PLayerState state;
    
    public long PlayerId;
    
    private Animator _animator;
    
    private Vector2 _inputDir;
    
    public float smoothPosSpeed = 10f;
    
    public float smoothRotSpeed = 10f;
    
    private Vector3 _renderDir = new Vector3(0 , 0 , 1);
    
    private Transform _playerCameraTransform;
    private Vector2 _cameraForward = new Vector2(0 , 0);
    
    

    #region 状态同步数据
    
    public Vector3 syncTargetPos;

    public Vector3 syncTargetDir;
    
    /// <summary>
    /// 当前状态同步计数
    /// </summary>
    private int _syncStateCurrentCount;
    
    private Vector2 _lastInput;
    

    #endregion
    
    
    
    public PlayerInput playerInput;





    public void Init(long playerId , PlayerType type)
    {
        PlayerId = playerId;
        playerType = type;
        _animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        _playerCameraTransform = CameraInit.MainInstance._cameraControl.transform;
        if (type is PlayerType.Other)
        {
            //远程玩家 不需要输入组件
            playerInput.enabled = false;
        }
        state = PLayerState.Idle;
        PlayAnimation("Idle");
        
    }

    public void PlayAnimation(string animName)
    {
        _animator.CrossFade(animName , 0.2f);
    }

    public void SyncPos(CSVector3 position, CSVector3 inputDir)
    {
        syncTargetPos = position.ToVector3();
        syncTargetDir = inputDir.ToVector3();
    }
    
    //初始化生成的位置
    public void InitPos(CSVector3 position, CSVector3 renderDir)
    {
        syncTargetPos = position.ToVector3();
        _renderDir = renderDir.ToVector3();
    }

    public void UpdatePos()
    {
        transform.position = Vector3.Lerp(transform.position , syncTargetPos , Time.deltaTime * smoothPosSpeed);
    }

    public void UpdateDir()
    {
        // 如果有移动方向
        if (syncTargetDir != Vector3.zero)
        {
            _renderDir = syncTargetDir;
        }
        
        // 计算目标旋转角度（朝向移动方向）
        Quaternion targetRotation = Quaternion.LookRotation(_renderDir);
        
        // 平滑插值到目标旋转
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotSpeed);
    }

    public void UpdateState()
    {
        if (syncTargetDir != Vector3.zero && state is not PLayerState.Run)
        {
            PlayAnimation("Run");
            state = PLayerState.Run;
        }
        else if (syncTargetDir == Vector3.zero && state is not PLayerState.Idle)
        {
            PlayAnimation("Idle");
            state = PLayerState.Idle;
        }
    }

    private void UpdateInput()
    {
        if(playerType is PlayerType.Self)
        {
            Vector2 input = playerInput.actions["Movement"].ReadValue<Vector2>();
            if (input != Vector2.zero)
            {
                // 获取相机前方向（XZ平面投影）
                Vector3 cameraForward = _playerCameraTransform.forward;
                cameraForward.y = 0;
                cameraForward.Normalize();
            
                // 获取相机右方向（XZ平面投影）
                Vector3 cameraRight = _playerCameraTransform.right;
                cameraRight.y = 0;
                cameraRight.Normalize();
            
                // 基于相机坐标系计算移动方向
                // input.y = 前后(W/S), input.x = 左右(A/D)
                Vector3 moveDirection = cameraForward * input.y + cameraRight * input.x;
            
                // 转换为Vector2（XZ平面）
                _inputDir = new Vector2(moveDirection.x, moveDirection.z);
            }
            else
            {
                _inputDir = Vector2.zero;
            }
            // _inputDir = input;
        }
        
        
        
        
    }
    
    
    
    
    

    private void Update()
    {
        
        UpdateInput();
        
        
        UpdatePos();
        UpdateDir();
        UpdateState();
    }
    


    private void FixedUpdate()
    {
        _syncStateCurrentCount++;
        
        //每隔100ms同步一次位置和方向
        if (_syncStateCurrentCount == StateSyncConfig.MaxSyncStateCount)
        {
    
            _syncStateCurrentCount = 0;
            

            if (Vector2.Equals(_lastInput, Vector2.zero) && Vector2.Equals(_inputDir, Vector2.zero))
            {
                //没有输入 不需要同步
                return;
            }
            


    
            // _syncPacketId++;
            
            stateSyncData stateSyncData = new stateSyncData()
            {
                inputDir = new CSVector3()
                {
                    x = _inputDir.x,
                    y = 0,
                    z = _inputDir.y
                }
            };
            
            //发送状态同步数据请求
            LobbyPlayerManager.MainInstance.SyncRoleState(stateSyncData);
            
            
            // StateSyncData stateSyncData = new StateSyncData()
            // {
            //     inputDir = _inputDir.ToCSVector3(),
            // };
            //
            // //发送状态同步数据请求
            // _hallRoleLogicCtrl.SyncRoleState(stateSyncData , _syncPacketId);
            //
            //记录上一次的输入
            _lastInput = _inputDir;
            
        }
    }
    
    
}
