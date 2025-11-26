using System;
using GGG.Tool.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    /// <summary>
    /// 负责管理鼠标相关功能
    /// </summary>
    public class MouseManager : MonoBehaviour
    {
        //提供两个功能： 显示鼠标和隐藏鼠标
                
        private PlayerInput _playerInput;
        
        private int _showMouseCount = 0;

        //初始化角色的时候调用
        public void Init()
        {
            _playerInput = GetComponent<PlayerInput>();
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
            
            // 只禁用移动和摄像机相关的输入
            if (_playerInput != null && _playerInput.actions != null)
            {
                var movement = _playerInput.actions.FindAction("Movement");
                var cameraLook = _playerInput.actions.FindAction("CameraLook");
                
                if (movement != null) movement.Disable();
                if (cameraLook != null) cameraLook.Disable();
            }
            
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
            // 重新启用移动和摄像机输入
            if (_playerInput != null && _playerInput.actions != null)
            {
                var movement = _playerInput.actions.FindAction("Movement");
                var cameraLook = _playerInput.actions.FindAction("CameraLook");
                
                if (movement != null) movement.Enable();
                if (cameraLook != null) cameraLook.Enable();
            }
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ShowMousePartial();
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                HideMousePartial();
            }
        }
    }
}