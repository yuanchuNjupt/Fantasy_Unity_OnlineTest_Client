using Framework.GameManagerFramework.WorldScripts;
using UnityEngine;

namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(LobbyWorld))]
    public class PlayerMouseLogicManager : ILogicBehaviour
    {
        
        //管理角色的鼠标操作逻辑

        private GameInputAction _gameInputAction;
        private int _showMouseCount;
        
        
        public Vector2 MoveInput => _gameInputAction.CharacterInput.Movement.ReadValue<Vector2>();
        
        
        public void OnCreate()
        {
            _gameInputAction = World.GetExitsLogicManager<UserMouseLogicManager>().GameInput;
            _gameInputAction.CharacterInput.Enable();
            _showMouseCount = 0;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        /// <summary>
        /// 显示鼠标（只禁用移动和摄像机，保留其他输入）
        /// </summary>
        public void ShowMousePartial()
        {

            _showMouseCount++;
            if(_showMouseCount > 1)
                return;
            
            _gameInputAction.CharacterInput.Disable();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        /// <summary>
        /// 隐藏鼠标（重新启用移动和摄像机输入）
        /// </summary>
        public void HideMousePartial()
        {
            _showMouseCount--;
            if(_showMouseCount > 0)
                return;
            
            _gameInputAction.CharacterInput.Enable();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        

        public void OnDestroy()
        {
            _gameInputAction.CharacterInput.Disable();
        }
        
    }
}