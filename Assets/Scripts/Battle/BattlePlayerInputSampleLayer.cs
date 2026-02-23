using System;
using FixedPhysics.Fixed_pointNumber.Core;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.LogicManagers;
using UnityEngine;

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

        private PlayerMouseLogicManager _playerMouseLogicManager;
        private BattleLogicManager _battleLogicManager;
        

        private BattlePlayerInstance _instance;
        



        public void Init(BattlePlayerInstance instance)
        {
            _instance = instance;
            _accInputSampleRuntime = 0f;
            
            _playerMouseLogicManager = World.GetExitsLogicManager<PlayerMouseLogicManager>();
            _battleLogicManager = World.GetExitsLogicManager<BattleLogicManager>();
            var cameraLogicManager = World.GetExitsLogicManager<TP_CameraLogicManager>(); 
            if (cameraLogicManager.cameraControl != null)
            {
                _playerCameraTransform = cameraLogicManager.cameraControl.transform;
            }
        }


        private void OnInputSampleFrameUpdate()
        {
            Vector2 input = _playerMouseLogicManager.MoveInput;
            if (input != Vector2.zero && _playerCameraTransform != null)
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
                
            if(LogicFrameConfig.IsUseLocalLogicFrame)
                _instance.logicLayer.UpdateMoveDir(new FixedIntVector3( _inputDir.x, 0 , _inputDir.y ));
            else
                _battleLogicManager.MoveFrameDataInput(new FixedIntVector3( _inputDir.x, 0 , _inputDir.y ));
                
                
                
                
            //检测攻击
            if (_playerMouseLogicManager.NormalAttack)
            {
                // 释放技能
                 _instance.logicLayer.ReleaseNormalAttack();
                    
            }
        }
        
        
                
        private void Update()
        {
            _accInputSampleRuntime += Time.deltaTime;

            if (_accInputSampleRuntime >= LogicFrameConfig.InputSampleInterval)
            {
                OnInputSampleFrameUpdate();
                _accInputSampleRuntime -= LogicFrameConfig.InputSampleInterval;
            }
            
        }
        
        
    }
}