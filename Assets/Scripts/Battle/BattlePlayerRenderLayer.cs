using FixedPhysics.Fixed_pointNumber.Core;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.LogicManagers;
using UnityEngine;

namespace Battle
{
    public class BattlePlayerRenderLayer : RenderObject
    {
        //在这里进行输入监听
        private Vector2 _inputDir = Vector2.zero;
        
        private Animator _playerAnimator;
        
        private PlayerType _playerType;

        private BattlePlayerLogicLayer _logicLayerLayer;
        
        private PlayerMouseLogicManager _playerMouseLogicManager;
        
        private Transform _playerCameraTransform;

        private PlayerState _renderState;
        
        private BattleLogicManager _battleLogicManager;

        private BattlePlayerInstance _instance;
        
        
        public void Init(PlayerType playerType , BattlePlayerInstance instance)
        {
            _playerType = playerType;
            _instance = instance;
            PlayAnim("Idle");
            _renderState = PlayerState.Idle;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            
            
            
            
            _logicLayerLayer = logicObject as BattlePlayerLogicLayer;
            _playerAnimator = GetComponent<Animator>();
            _playerMouseLogicManager = World.GetExitsLogicManager<PlayerMouseLogicManager>();
            _battleLogicManager = World.GetExitsLogicManager<BattleLogicManager>();
            
            //设置TP相机
            var cameraLogicManager = World.GetExitsLogicManager<TP_CameraLogicManager>(); 
            cameraLogicManager.InitTPCamera(transform);
            if (cameraLogicManager.cameraControl != null)
            {
                _playerCameraTransform = cameraLogicManager.cameraControl.transform;
            }
        }


        public override void Update()
        {
  
            UpdateInput();
            UpdateState();
            
            
            base.Update();
        }


        private void UpdateInput()
        {
            if(_playerType is PlayerType.Self)
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
                    _logicLayerLayer.InputLogicFrameEvent(new FixedIntVector3( _inputDir.x, 0 , _inputDir.y ));
                else
                    _battleLogicManager.MoveFrameDataInput(new FixedIntVector3( _inputDir.x, 0 , _inputDir.y ));
                
                
                
                
                //检测攻击
                if (_playerMouseLogicManager.NormalAttack)
                {
                    //释放技能
                    _logicLayerLayer.ReleaseNormalAttack();
                    
                }
                
                
            }
        }

        public override void PlayAnim(string clipName)
        {
            _playerAnimator.CrossFade(clipName , 0.2f);
        }

        private void UpdateState()
        {
            if(_inputDir != Vector2.zero && _renderState != PlayerState.Run)
            {
                PlayAnim("RunStart");
                _renderState = PlayerState.Run;
            }
            else if (_inputDir == Vector2.zero && _renderState != PlayerState.Idle)
            {
                PlayAnim("Idle");
                _renderState = PlayerState.Idle;
            }
        }

        public override void UpdateNetInputDir(FixedIntVector3 netInputDir)
        {
            if (_playerType != PlayerType.Self)
            {
                _inputDir = netInputDir.ToVector3();
            }
        }
    }
}