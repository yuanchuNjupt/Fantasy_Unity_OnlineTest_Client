using Framework.GameManagerFramework.LogicManagers;
using UnityEngine;

public partial class LobbyPlayer
{
    private PlayerMouseLogicManager _playerMouseLogicManager;


    private void OnLobbyPlayerInputInit()
    {
        _playerMouseLogicManager = World.GetExitsLogicManager<PlayerMouseLogicManager>();
    }
    
    
    private void UpdateInput()
    {
        if(playerType is PlayerType.Self)
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
        }
    }

    private void UpdateInputState()
    {
        if (_inputDir == Vector2.zero)
        {
            state = PlayerState.Idle;
        }
        else
        {
            state = _playerMouseLogicManager.Run ? PlayerState.Sprint : PlayerState.Run;
        }
            
    }
    
    
    
    private void OnLobbyPlayerInputUpdate()
    {
        UpdateInput();
        UpdateInputState();
    }
    
}
