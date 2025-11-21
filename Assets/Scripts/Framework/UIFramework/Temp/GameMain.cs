using System;
using UIFramework.Core;
using UIFramework.ViewPath;
using UnityEngine;

namespace UIFramework.Temp
{
    public class GameMain : MonoBehaviour
    {
        
        public bool IsHidePointer = true;
        
        
        
        private void Start()
        {
            // MainPanelView mainPanelView = UIManager.Instance.GetPanel<MainPanelView>();
            // mainPanelView.gameObject.SetActive(false);

            if (IsHidePointer)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        
        
        }

        
        
       
    }
}