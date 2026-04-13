using Framework.GameManager.Core;
using Framework.GameManagerFramework.WorldScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class BattlePlayerMouseLogicManager : ILogicBehaviour
    {
        
        private GameInputAction _gameInputAction;
        
        public Vector2 MoveInput => _gameInputAction.BattlePlayerInputMap.Movement.ReadValue<Vector2>();
        
        /// <summary>
        /// 当前战斗场景的 CameraLook InputAction，供 TP_CameraControl 直接绑定
        /// </summary>
        public InputAction CameraLookAction => _gameInputAction.BattlePlayerInputMap.CameraLook;

        public bool NormalAttack => _gameInputAction.BattlePlayerInputMap.LAttack.triggered;
        
        public bool SpecialSkill1 => _gameInputAction.BattlePlayerInputMap.SpecialSkill1.triggered;
        
        
        public void OnCreate()
        {
            _gameInputAction = World.GetExitsLogicManager<UserMouseLogicManager>().GameInput;
            _gameInputAction.BattlePlayerInputMap.Enable();
            
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
        
        
        private int _showMouseCount; // 记录当前显示鼠标的请求数量
        
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
            _gameInputAction.BattlePlayerInputMap.Disable();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }
    }
}