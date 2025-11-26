using System;
using System.Collections;
using System.Collections.Generic;
using Lobby;
using UIFramework.Presenter;
using UIFramework.ViewPath;
using UnityEngine;

public class LobbyPlayerPanelPresenter : BasePresenter<LobbyPlayerPanelView>
{
    private bool _showTeamPanel = false;
    
    private List<TeamMemberInfo> _teamMembers = new List<TeamMemberInfo>();
    private void Awake()
    {
        View.TeamButton.onClick.AddListener(OnTeamButtonClick);
        View.CreateTeamButton.onClick.AddListener(OnCreateTeamButtonClick);
        View.JoinTeamButton.onClick.AddListener(OnJoinTeamButtonClick);
    }

    private void OnDestroy()
    {   
        View.TeamButton.onClick.RemoveListener(OnTeamButtonClick);
        View.CreateTeamButton.onClick.RemoveListener(OnCreateTeamButtonClick);
        View.JoinTeamButton.onClick.RemoveListener(OnJoinTeamButtonClick);
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

    private void OnCreateTeamButtonClick()
    {
        //TODO:向服务器发送创建房间请求
    }

    private void OnJoinTeamButtonClick()
    {
        if (string.IsNullOrEmpty(View.RoomInput.text))
        {
            return;
        }
        
        //TODO:向服务器发送加入小队请求
        
        
        
    }
    
    // public void 
    
    


}
