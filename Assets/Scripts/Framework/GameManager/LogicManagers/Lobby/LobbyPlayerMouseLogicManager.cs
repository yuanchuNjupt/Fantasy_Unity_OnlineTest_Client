using Framework.GameManager.Core;
using Framework.GameManagerFramework.WorldScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(LobbyWorld))]
    public class LobbyPlayerMouseLogicManager : ILogicBehaviour
    {
        
        //管理角色的鼠标操作逻辑

        private GameInputAction _gameInputAction;
        private int _showMouseCount;
        
        
        public Vector2 MoveInput => _gameInputAction.LobbyPlayerInputMap.Movement.ReadValue<Vector2>();
        
        /// <summary>
        /// 当前大厅场景的 CameraLook InputAction，供 TP_CameraControl 直接绑定
        /// </summary>
        public InputAction CameraLookAction => _gameInputAction.LobbyPlayerInputMap.CameraLook;

        public bool Run => _gameInputAction.LobbyPlayerInputMap.Run.phase == InputActionPhase.Performed;

        public void OnCreate()
        {
            _gameInputAction = World.GetExitsLogicManager<UserMouseLogicManager>().GameInput;
            _gameInputAction.LobbyPlayerInputMap.Enable();
            
            // 订阅 CallMouse 按键事件
            _gameInputAction.LobbyPlayerInputMap.CallMouse.started += OnCallMousePressed;
            _gameInputAction.LobbyPlayerInputMap.CallMouse.canceled += OnCallMouseReleased;
            
            _showMouseCount = 0;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void OnCallMousePressed(InputAction.CallbackContext context)
        {
            ShowMousePartial();
        }
        
        private void OnCallMouseReleased(InputAction.CallbackContext context)
        {
            HideMousePartial();
        }
        
        /// <summary>
        /// 显示鼠标（只禁用移动和摄像机，保留其他输入）
        /// </summary>
        public void ShowMousePartial()
        {

            _showMouseCount++;
            if(_showMouseCount > 1)
                return;
            
            // _gameInputAction.LobbyPlayerInputMap.Disable();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            Debug.Log("Showing mouse : " + _showMouseCount);
        }
        
        /// <summary>
        /// 隐藏鼠标（重新启用移动和摄像机输入）
        /// </summary>
        public void HideMousePartial()
        {
            
            
            _showMouseCount--;
            Debug.Log("Hide mouse : " + _showMouseCount);
            if(_showMouseCount > 0)                                                                                 
                return;
            
            // _gameInputAction.LobbyPlayerInputMap.Enable();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        

        public void OnDestroy()
        {
            // 取消订阅 CallMouse 按键事件
            _gameInputAction.LobbyPlayerInputMap.CallMouse.started -= OnCallMousePressed;
            _gameInputAction.LobbyPlayerInputMap.CallMouse.canceled -= OnCallMouseReleased;
            _gameInputAction.LobbyPlayerInputMap.Disable();
        }
        
    }
}