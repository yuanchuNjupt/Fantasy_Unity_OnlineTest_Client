using System;
using System.Collections;
using System.Collections.Generic;
using Fantasy;
using Generate;
using Lobby;
using UIFramework.Core;
using UIFramework.Presenter;
using UIFramework.ViewPath;
using UnityEngine;

public class LobbyPlayerPanelPresenter : BasePresenter<LobbyPlayerPanelView>
{
    private bool _showTeamPanel = false;
    

    private void Awake()
    {
        View.TeamButton.onClick.AddListener(OnTeamButtonClick);
        View.CreateTeamButton.onClick.AddListener(OnCreateTeamButtonClick);
        View.JoinTeamButton.onClick.AddListener(OnJoinTeamButtonClick);
        View.LevelTeamButton.onClick.AddListener(OnLevelTeamButtonClick);
    }

    private void OnDestroy()
    {
        View.TeamButton.onClick.RemoveListener(OnTeamButtonClick);
        View.CreateTeamButton.onClick.RemoveListener(OnCreateTeamButtonClick);
        View.JoinTeamButton.onClick.RemoveListener(OnJoinTeamButtonClick);
        View.LevelTeamButton.onClick.RemoveListener(OnLevelTeamButtonClick);
    }

    private void OnTeamButtonClick()
    {
        _showTeamPanel = !_showTeamPanel;

        if (_showTeamPanel)
        {
            View.TeamBackground.gameObject.SetActive(_showTeamPanel);
            //显示鼠标
            LobbyPlayerManager.MainInstance.mouseManager.ShowMousePartial();
        }
        else
        {
            View.TeamBackground.gameObject.SetActive(_showTeamPanel);
            //隐藏鼠标
            LobbyPlayerManager.MainInstance.mouseManager.HideMousePartial();
        }
    }

    private async void OnCreateTeamButtonClick()
    {
        //TODO:向服务器发送创建房间请求
        var res = await LobbyPlayerManager.MainInstance.teamManager.CreateTeam();
        if (res != 0)
        {
            Debug.LogWarning("创建小队失败，错误码：" + res);
            return;
        }
        Debug.Log("创建小队成功，队伍ID : " + LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamId);
        

        //UI响应
        UIManager.MainInstance.AddPanel<MemberView>(View.MemberPrefab, UILayer.Main,
            Main.MainInstance.UserData.AccountId.ToString() , false);
        var panel = UIManager.MainInstance.ShowPanel<MemberView>(Main.MainInstance.UserData.AccountId.ToString());
        panel.gameObject.transform.SetParent(View.TeamMember ,false);
        panel.MemberName.text = Main.MainInstance.UserData.UserName;
        
        //设置房间号，显示退出按钮
        
        SwitchTeamState(true);
        View.RoomId.text = "房间号:" + LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamId;
    }

    private async void OnJoinTeamButtonClick()
    {
        if (string.IsNullOrEmpty(View.RoomInput.text))
        {
            return;
        }
        
        // 验证输入是否为有效的数字
        if (!long.TryParse(View.RoomInput.text, out long roomId))
        {
            Debug.LogWarning("房间号格式错误，请输入有效的数字");
            return;
        }
        
        //TODO:向服务器发送加入小队请求
        var res = await LobbyPlayerManager.MainInstance.teamManager.JoinTeam(Main.MainInstance.UserData.AccountId, roomId);
        if (res != 0)
            return;
        
        Debug.Log("加入小队成功，队伍ID : " + LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamId);
        
        //UI响应
        
        SwitchTeamState(true);
        View.RoomId.text = "房间号:" + LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamId;
        
        
        
        
    }

    private void OnLevelTeamButtonClick()
    {
        SwitchTeamState(false);
        
        //删除所有成员UI
        LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamMembers.ForEach(x =>
            UIManager.MainInstance.RemovePanel(x.accountId.ToString()));
        UIManager.MainInstance.RemovePanel(LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamOwner.accountId.ToString());
        
        
        LobbyPlayerManager.MainInstance.teamManager.LevelTeam();
    }


    public void AddMember(long playerId)
    {
        UIManager.MainInstance.AddPanel<MemberView>(View.MemberPrefab, UILayer.Main,
            playerId.ToString() , false);
        var panel = UIManager.MainInstance.ShowPanel<MemberView>(playerId.ToString());
        panel.gameObject.transform.SetParent(View.TeamMember ,false);
        panel.MemberName.text = LobbyPlayerManager.MainInstance.otherPlayers[playerId].PlayerName;
    }
    
    private void SwitchTeamState(bool inTeam)
    {
        if (inTeam)
        {
            View.CreateTeamButton.gameObject.SetActive(false);
            View.JoinTeamButton.gameObject.SetActive(false);
            View.LevelTeamButton.gameObject.SetActive(true);
        
            View.RoomInput.gameObject.SetActive(false);
            View.RoomIdBackground.gameObject.SetActive(true);
        }
        else
        {
            View.CreateTeamButton.gameObject.SetActive(true);
            View.JoinTeamButton.gameObject.SetActive(true);
            View.LevelTeamButton.gameObject.SetActive(false);
        
            View.RoomInput.gameObject.SetActive(true);
            View.RoomIdBackground.gameObject.SetActive(false);
            View.RoomId.text = "";
        }
    }
    
    
}