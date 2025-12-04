using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using Fantasy;
using Fantasy.Network;
using Framework.GameManagerFramework.LogicManagers;
using Framework.GameManagerFramework.WorldScripts;
using Framework.MessageManagers;
using Generate;
using UIFramework.Core;
using UIFramework.Presenter;
using UIFramework.ViewPath;
using UnityEngine;

public class RegisterPanelPresenter : BasePresenter<RegisterPanelView>
{
    private void Awake()
    {
        View.Exit.onClick.AddListener(OnExitButtonClick);
        View.Register.onClick.AddListener(OnRegisterButtonClick);
    }

    private void OnDestroy()
    {
        View.Exit.onClick.RemoveListener(OnExitButtonClick);
        View.Register.onClick.RemoveListener(OnRegisterButtonClick);
    }

    private void OnExitButtonClick()
    {
        //将自己隐藏
        UIManager.MainInstance.HideTopPanel(UILayer.Main);
    }

    private void OnRegisterButtonClick()
    {
        World.GetExitsLogicManager<LoginLogicManager>().RegisterAccount(View.account.text , View.password.text);
    }
    
}