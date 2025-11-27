using System;
using TMPro;
using UnityEngine;

namespace Lobby
{
    public class LobbyPlayerName : MonoBehaviour
    {
        public TextMeshPro Name;
        private bool _initialized = false;
        
        private Transform _cameraTransform;
        public void Init(string name , Transform cameraTransform)
        {
            Name.text = name;
            _cameraTransform = cameraTransform;
            _initialized = true;
        }

        private void LateUpdate()
        {
            
            if(!_initialized)
                return;
            
            // 让文字完全面向相机（所有轴向）
            // 使用位置差计算方向
            Vector3 directionToCamera = _cameraTransform.position - Name.gameObject.transform.position;
            
            // 直接设置旋转，让文字朝向相机
            Name.gameObject.transform.rotation = Quaternion.LookRotation(-directionToCamera);
        }
        
        
    }
}