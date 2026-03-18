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
            
            // 如果没有绑定摄像机（即其他玩家的名字），跳过旋转同步
            if (_cameraTransform == null)
                return;
            
            // 让文字完全面向相机
            // 直接使用相机的正前方向作为文字的前方
            Name.gameObject.transform.rotation = _cameraTransform.rotation;
        }
        
        
    }
}