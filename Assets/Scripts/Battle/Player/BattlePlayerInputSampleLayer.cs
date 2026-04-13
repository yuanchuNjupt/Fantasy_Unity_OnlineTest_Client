using System;
using Fantasy;
using FixedPhysics.Fixed_pointNumber.Core;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.LogicManagers;
using UnityEngine;
using Log = Framework.AdvancedLog.Log;

namespace Battle
{
    /// <summary>
    /// 输入采样层
    /// </summary>
    public class BattlePlayerInputSampleLayer : MonoBehaviour
    {
        
        private Vector2 _inputDir = Vector2.zero;
        
        private Transform _playerCameraTransform;
        
        //输入采样累计运行时间
        private float _accInputSampleRuntime;
        private BattlePlayerInstance _instance;

        private Vector2 newInput;

        // 缓存攻击输入，避免因采样频率低而丢失 triggered 事件
        private bool _pendingNormalAttack;
        



        public void Init(BattlePlayerInstance instance)
        {
            _instance = instance;
            _accInputSampleRuntime = 0f;
            if (instance.cameraLogicManager.cameraControl != null)
            {
                _playerCameraTransform = instance.cameraLogicManager.cameraControl.transform;
            }
        }


        private void OnInputSampleFrameUpdate()
        {
            
            if(_instance.logicLayer.ObjectState is LogicObjectState.Death) //不采集输入
                return;
            
            
            //检测攻击
            if (_pendingNormalAttack)
            {
                _pendingNormalAttack = false;
                // 释放技能
                if (LogicFrameConfig.IsUseLocalLogicFrame)
                {
                    _instance.logicLayer.ReleaseNormalAttack();
                }
                else
                {
                    _instance.logicLayer.ReleaseNormalAttack();
                }
                
                return;
            }
            
            
            newInput = _instance.battleMouseLogicManager.MoveInput;
            if (newInput != Vector2.zero && _playerCameraTransform != null)
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
                Vector3 moveDirection = cameraForward * newInput.y + cameraRight * newInput.x;
            
                // 转换为Vector2（XZ平面）
                _inputDir = new Vector2(moveDirection.x, moveDirection.z);
                
                if (LogicFrameConfig.IsUseLocalLogicFrame)
                {
                    _instance.logicLayer.UpdateMoveDir(new FixedIntVector3( _inputDir.x, 0 , _inputDir.y ));
                }
                else
                    _instance.battleLogicManager.MoveFrameDataInput(new FixedIntVector3( _inputDir.x, 0 , _inputDir.y ));
                
                
                return;
            }
            
            // 没有输入时，发送零输入
            if(!LogicFrameConfig.IsUseLocalLogicFrame)
                _instance.battleLogicManager.NoneFrameDataInput();
            

    
            
            
        }
        
        
                
        private void Update()
        {
            // 输入缓冲
            if (_instance.battleMouseLogicManager.NormalAttack)
            {
                _pendingNormalAttack = true;
            }

            _accInputSampleRuntime += Time.deltaTime;

            if (_accInputSampleRuntime >= LogicFrameConfig.InputSampleInterval)
            {
                OnInputSampleFrameUpdate();
                _accInputSampleRuntime -= LogicFrameConfig.InputSampleInterval;
            }
            
        }
        
        
    }
}