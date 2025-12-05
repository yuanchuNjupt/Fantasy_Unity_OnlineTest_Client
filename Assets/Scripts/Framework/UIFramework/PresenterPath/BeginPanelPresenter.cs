using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using Fantasy;
using Fantasy.Network;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.LogicManagers;
using Framework.MessageManagers;
using Generate;
using Lobby;
using UIFramework.Core;
using UIFramework.Presenter;
using UIFramework.ViewPath;


public class BeginPanelPresenter : BasePresenter<BeginPanelView>
{
    private void Awake()
    {
        View.Login.onClick.AddListener(OnLoginClick);
        View.Register.onClick.AddListener(OnRegisterClick);
    }
    private void OnLoginClick()
    {
        World.GetExitsLogicManager<LoginLogicManager>().LoginGame(View.account.text , View.password.text);
    }
    private void OnRegisterClick()
    {
        UIManager.MainInstance.ShowPanel<RegisterPanelView>();
    }

    private void OnDestroy()
    {
        View.Login.onClick.RemoveListener(OnLoginClick);
        View.Register.onClick.RemoveListener(OnRegisterClick);
    }
}
