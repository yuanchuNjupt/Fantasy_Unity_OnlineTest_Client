
using System;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.LogicManagers;
using Lobby;
using Lobby.TeamInfo;
using UIFramework.Core;
using UIFramework.Presenter;
using UIFramework.ViewPath;
using UnityEngine;

public class LobbyPlayerPanelPresenter : BasePresenter<LobbyPlayerPanelView>
{
    private bool _showTeamPanel = false;
    private LobbyPlayerMouseLogicManager _lobbyPlayerMouseLogicManager;
    private LobbyTeamLogicManager _lobbyTeamLogicManager;
    private UserDataManager _userDataManager;
    
    private Team _teamInfo;

    private void Awake()
    {
        View.TeamButton.onClick.AddListener(OnTeamButtonClick);
        View.CreateTeamButton.onClick.AddListener(OnCreateTeamButtonClick);
        View.JoinTeamButton.onClick.AddListener(OnJoinTeamButtonClick);
        View.LevelTeamButton.onClick.AddListener(OnLevelTeamButtonClick);
        View.BattleButton.onClick.AddListener(OnEnterDungeonButtonClick);
    }

    private void Start()
    {
        _userDataManager = Framework.GameManager.Core.World.GetExitsDataManager<UserDataManager>();
        _lobbyPlayerMouseLogicManager = Framework.GameManager.Core.World.GetExitsLogicManager<LobbyPlayerMouseLogicManager>();
        _lobbyTeamLogicManager = Framework.GameManager.Core.World.GetExitsLogicManager<LobbyTeamLogicManager>();
    }

    private void OnDestroy()
    {
        View.TeamButton.onClick.RemoveListener(OnTeamButtonClick);
        View.CreateTeamButton.onClick.RemoveListener(OnCreateTeamButtonClick);
        View.JoinTeamButton.onClick.RemoveListener(OnJoinTeamButtonClick);
        View.LevelTeamButton.onClick.RemoveListener(OnLevelTeamButtonClick);
        View.BattleButton.onClick.RemoveListener(OnEnterDungeonButtonClick);
    }

    #region 组队系统

    

    private void OnTeamButtonClick()
    {
        _showTeamPanel = !_showTeamPanel;

        if (_showTeamPanel)
        {
            View.TeamBackground.gameObject.SetActive(_showTeamPanel);
            //显示鼠标
            _lobbyPlayerMouseLogicManager.ShowMousePartial();
        }
        else
        {
            View.TeamBackground.gameObject.SetActive(_showTeamPanel);
            //隐藏鼠标
            _lobbyPlayerMouseLogicManager.HideMousePartial();
        }
    }

    private async void OnCreateTeamButtonClick()
    {
       
        _teamInfo = await _lobbyTeamLogicManager.CreateTeam();
        
        if(_teamInfo == null)
            return;
        AddMember(_userDataManager.UserData.AccountId , _userDataManager.UserData.UserName);
        //设置房间号，显示退出按钮
        SwitchTeamState(true);
        View.RoomId.text = "房间号:" + _teamInfo.TeamId;
    }

    private async void OnJoinTeamButtonClick()
    {
        _teamInfo = await _lobbyTeamLogicManager.JoinTeam(View.RoomInput.text);
        
        if(_teamInfo == null)
            return;
        
        //UI响应
        //首先是队长
        AddMember(_teamInfo.TeamOwner.accountId , _teamInfo.TeamOwner.memberName);
        
        _teamInfo.TeamMembers.ForEach(member =>
        {
            AddMember(member.accountId, member.memberName);
        });
        
        SwitchTeamState(true);
        View.RoomId.text = "房间号:" + _teamInfo.TeamId;
    }

    private void OnLevelTeamButtonClick()
    {
        ClearMembers();
        _lobbyTeamLogicManager.LeaveTeam();
    }


    public void AddMember(long playerId , string playerName)
    {
        UIManager.MainInstance.AddPanel<MemberView>(View.MemberPrefab, UILayer.Main,
            playerId.ToString() , false);
        var panel = UIManager.MainInstance.ShowPanel<MemberView>(playerId.ToString());
        panel.gameObject.transform.SetParent(View.TeamMember ,false);
        panel.MemberName.text = playerName;
    }
    
    public void RemoveMember(long playerId)
    {
        UIManager.MainInstance.RemovePanel(playerId.ToString());
    }
    
    public void ClearMembers()
    {
        SwitchTeamState(false);
        _teamInfo.TeamMembers.ForEach(member =>
        {
            UIManager.MainInstance.RemovePanel(member.accountId.ToString());
        });
        UIManager.MainInstance.RemovePanel(_teamInfo.TeamOwner.accountId.ToString());
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
            View.RoomInput.text = string.Empty;
            View.RoomId.text = "";
        }
    }
    
    #endregion


    #region 进入战斗相关

    private void OnEnterDungeonButtonClick()
    {
        Framework.GameManager.Core.World.GetExitsLogicManager<LobbyBattleLogicManager>().OnTeamLeaderEnterDungeon();
    }

    #endregion
    
}