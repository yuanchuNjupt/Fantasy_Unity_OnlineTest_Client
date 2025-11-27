using System;
using System.Collections;
using System.Collections.Generic;
using Fantasy;
using Fantasy.Async;
using Generate;
using Lobby;
using UIFramework.Core;
using UIFramework.Presenter;
using UIFramework.ViewPath;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = Fantasy.Scene;

public class RegisterNamePanelPresenter : BasePresenter<RegisterNamePanelView>
{
    private void Awake()
    {
        View.Commit.onClick.AddListener(OnCommitButtonClick);
    }

    private void OnDestroy()
    {
        View.Commit.onClick.RemoveListener(OnCommitButtonClick);
    }

    private async void OnCommitButtonClick()
    {
        if (string.IsNullOrEmpty(View.InputName.text))
        {
            Debug.LogWarning("不可输入空的名称");
            return;
        }

        
        var res = await RegisterName(Main.MainInstance.UserData.AccountName , View.InputName.text);

        if (res.ErrorCode != 0)
        {
            Debug.LogWarning("注册失败");
            return;
        }
        
        Debug.Log("注册成功，名称：" + res.name);
        Main.MainInstance.UserData.UserName = res.name;
        //注册成功，返回登陆界面
        
        //跳转场景
        var EntryLobbyReq = new EntryLobbyRequest();
        EntryLobbyReq.accountId = Main.MainInstance.UserData.AccountId;
        var EntryLobbyRes = await NetWorkManager.Instance.Call<EntryLobbyResponse>(EntryLobbyReq);
        
 
        
        //场景跳转
        UIManager.MainInstance.HidePanel<RegisterNamePanelView>();
        SceneManager.sceneLoaded += OnLobbySceneLoaded;
        SceneManager.LoadScene("Lobby");
        
        // 定义场景加载回调方法
        void OnLobbySceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode) {
            SceneManager.sceneLoaded -= OnLobbySceneLoaded;
            LobbyPlayerManager.MainInstance.OnLocalEntryLobby(EntryLobbyRes.selfData , EntryLobbyRes.otherPlayerData);
            UIManager.MainInstance.ShowPanel<LobbyPlayerPanelView>();
        }
    }


    private async FTask<RegisterNameResponse> RegisterName(string accountName , string name)
    {
        var req = new RegisterNameRequest();
        req.accountName = accountName;
        req.name = name;
        return await NetWorkManager.Instance.Call<RegisterNameResponse>(req);
    }
}
