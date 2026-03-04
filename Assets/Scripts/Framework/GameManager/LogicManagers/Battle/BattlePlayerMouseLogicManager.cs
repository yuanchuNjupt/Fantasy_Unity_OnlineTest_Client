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
        
        
        public void OnCreate()
        {
            _gameInputAction = World.GetExitsLogicManager<UserMouseLogicManager>().GameInput;
            _gameInputAction.BattlePlayerInputMap.Enable();
            
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