using System;
using System.Collections;
using System.Collections.Generic;
using Fantasy;
using Fantasy.Network;
using Generate;
using Lobby;
using UIFramework.Core;
using UIFramework.Presenter;
using UIFramework.ViewPath;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class BeginPanelPresenter : BasePresenter<BeginPanelView>
{
    private void Awake()
    {
        View.Login.onClick.AddListener(OnLoginClick);
        View.Register.onClick.AddListener(OnRegisterClick);
    }
    
    private async void OnLoginClick()
    {
        if(String.IsNullOrEmpty(View.account.text) || String.IsNullOrEmpty(View.password.text))
        {
            Debug.LogWarning("账号或密码不能为空");
            return;
        }
        
        //连接服务器并发送登录请求
        NetWorkManager.Instance.Connect("192.168.101.193:20000", NetworkProtocolType.KCP);
        
        LoginRequest req = new LoginRequest();
        req.account = View.account.text;
        req.pass = View.password.text;

        var res = await NetWorkManager.Instance.Call<LoginResponse>(req);

        if (res.ErrorCode != 0)
        {
            Debug.LogError("登录失败 错误码：" + res.ErrorCode);
            return;
        }
        Debug.Log("登录成功 玩家ID：" + res.selfData.playerId);
        
        Main.MainInstance.SetUserData(View.account.text, res.selfData.playerId);
        
        //场景跳转
        UIManager.MainInstance.HidePanel<BeginPanelView>();
        SceneManager.sceneLoaded += OnLobbySceneLoaded;
        SceneManager.LoadScene("Lobby");
        // LobbyPlayerManager.MainInstance.OnLocalEntryLobby();
        
        void OnLobbySceneLoaded(Scene scene, LoadSceneMode mode) {
            SceneManager.sceneLoaded -= OnLobbySceneLoaded;
            LobbyPlayerManager.MainInstance.OnLocalEntryLobby(res.selfData , res.otherPlayerData);
        }
    }
    


    private void OnRegisterClick()
    {
        //TODO:实现注册功能
        UIManager.MainInstance.ShowPanel<RegisterPanelView>();

    }

    private void OnDestroy()
    {
        View.Login.onClick.RemoveListener(OnLoginClick);
        View.Register.onClick.RemoveListener(OnRegisterClick);
    }
}
