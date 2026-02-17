using System.Collections.Generic;
using System.Linq;
using Framework.GameManagerFramework.LogicManagers;
using UnityEngine;

public partial class LobbyPlayer
{
    private PlayerMouseLogicManager _playerMouseLogicManager;
    private List<Vector2> _inputDirList = new List<Vector2>();
    private List<bool> _inputRunList = new List<bool>();
    
    

    private void OnLobbyPlayerInputInit()
    {
        _playerMouseLogicManager = Framework.GameManager.Core.World.GetExitsLogicManager<PlayerMouseLogicManager>();
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
                input = new Vector2(moveDirection.x, moveDirection.z);
            }
            else
            {
                input = Vector2.zero;
            }
            _inputDirList.Add(input);
        }
    }

    //收集跑步状态输入
    private void UpdateInputState()
    {
        bool isRun = _playerMouseLogicManager.Run;
        _inputRunList.Add(isRun);
            
    }
    
    
    
    private void OnLobbyPlayerInputUpdate()
    {
        UpdateInput();
        UpdateInputState();
    }
    
    private (Vector2 inputDir , PlayerState State) GetPredictionInputDirAndState()
    {
        Vector2 resDir = Vector2.zero;
        //从输入列表中获取最新的输入方向
        for (int i = _inputDirList.Count - 1; i >= 0; i--)
        {
            if(_inputDirList[i] != Vector2.zero)
            {
                resDir = _inputDirList[i];
                _inputDirList.Clear();
                break;   
            }
        }
        
        bool isRun = _inputRunList.FirstOrDefault(x => x);
        PlayerState state = PlayerState.Idle;
        if (resDir != Vector2.zero)
        {
            state = isRun ? PlayerState.Sprint : PlayerState.Run;
        }
        
        _inputRunList.Clear();
        _inputDirList.Clear();
        return (resDir , state);
    }
    
}
