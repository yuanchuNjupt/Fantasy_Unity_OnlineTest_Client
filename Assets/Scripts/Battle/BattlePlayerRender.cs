using Framework.GameManagerFramework.LogicManagers;
using UnityEngine;

namespace Battle
{
    public class BattlePlayerRender : RenderObject
    {
        //在这里进行输入监听
        private Vector2 _inputDir = Vector2.zero;
        
        private Animator _playerAnimator;
        
        private PlayerType _playerType;

        private BattlePlayerLogic _logicLayer;
        
        private PlayerMouseLogicManager _playerMouseLogicManager;
        
        private Transform _playerCameraTransform;

        public void Init(PlayerType playerType)
        {
            _playerType = playerType;
            PlayAnim("Idle");
        }

        public override void OnCreate()
        {
            base.OnCreate();
            _logicLayer = logicObject as BattlePlayerLogic;
            _playerAnimator = GetComponent<Animator>();
            _playerMouseLogicManager = World.GetExitsLogicManager<PlayerMouseLogicManager>();
            _playerCameraTransform = World.GetExitsLogicManager<TP_CameraLogicManager>().cameraControl.transform;
        }


        public override void Update()
        {
            UpdateInput();
            base.Update();
        }


        private void UpdateInput()
        {
            if(_playerType is PlayerType.Self)
            {
                Vector2 input = _playerMouseLogicManager.MoveInput;
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
                
                //检测攻击
                if (_playerMouseLogicManager.NormalAttack)
                {
                    //释放技能
                    _logicLayer.ReleaseNormalAttack();
                    
                    
                }
                
                
            }
        }

        public override void PlayAnim(string clipName)
        {
            _playerAnimator.CrossFade(clipName , 0.2f);
        }
    }
}