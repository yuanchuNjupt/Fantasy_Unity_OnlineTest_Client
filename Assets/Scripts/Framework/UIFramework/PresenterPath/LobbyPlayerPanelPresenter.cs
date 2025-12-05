
using System;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.LogicManagers;
using Lobby;
using UIFramework.Core;
using UIFramework.Presenter;
using UIFramework.ViewPath;
using UnityEngine;

public class LobbyPlayerPanelPresenter : BasePresenter<LobbyPlayerPanelView>
{
    private bool _showTeamPanel = false;
    private PlayerMouseLogicManager _playerMouseLogicManager;
    private LobbyTeamLogicManager _lobbyTeamLogicManager;

    private void Awake()
    {
        View.TeamButton.onClick.AddListener(OnTeamButtonClick);
        View.CreateTeamButton.onClick.AddListener(OnCreateTeamButtonClick);
        View.JoinTeamButton.onClick.AddListener(OnJoinTeamButtonClick);
        View.LevelTeamButton.onClick.AddListener(OnLevelTeamButtonClick);
    }

    private void Start()
    {
        _playerMouseLogicManager = World.GetExitsLogicManager<PlayerMouseLogicManager>();
        _lobbyTeamLogicManager = World.GetExitsLogicManager<LobbyTeamLogicManager>();
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
            _playerMouseLogicManager.ShowMousePartial();
        }
        else
        {
            View.TeamBackground.gameObject.SetActive(_showTeamPanel);
            //隐藏鼠标
            _playerMouseLogicManager.HideMousePartial();
        }
    }

    private async void OnCreateTeamButtonClick()
    {
        var res = await LobbyPlayerManager.MainInstance.teamManager.CreateTeam();
        if (res != 0)
        {
            Debug.LogWarning("创建小队失败，错误码：" + res);
            return;
        }
        Debug.Log("创建小队成功，队伍ID : " + LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamId);
        

        //UI响应
        UIManager.MainInstance.AddPanel<MemberView>(View.MemberPrefab, UILayer.Main,
            World.GetExitsDataManager<UserDataManager>().UserData.AccountId.ToString() , false);
        var panel = UIManager.MainInstance.ShowPanel<MemberView>(World.GetExitsDataManager<UserDataManager>().UserData.AccountId.ToString());
        panel.gameObject.transform.SetParent(View.TeamMember ,false);
        panel.MemberName.text = World.GetExitsDataManager<UserDataManager>().UserData.UserName;
        
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
        
        var res = await LobbyPlayerManager.MainInstance.teamManager.JoinTeam(World.GetExitsDataManager<UserDataManager>().UserData.AccountId, roomId);
        if (res != 0)
            return;
        
        Debug.Log("加入小队成功，队伍ID : " + LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamId);
        
        //UI响应
        //首先是队长
        UIManager.MainInstance.AddPanel<MemberView>(View.MemberPrefab, UILayer.Main,
            LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamOwner.accountId.ToString() , false);
        var panel = UIManager.MainInstance.ShowPanel<MemberView>(LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamOwner.accountId.ToString() );
        panel.gameObject.transform.SetParent(View.TeamMember ,false);
        panel.MemberName.text = LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamOwner.memberName;
        
        //然后是队员
        LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamMembers.ForEach(member =>
        {
            UIManager.MainInstance.AddPanel<MemberView>(View.MemberPrefab, UILayer.Main,
                member.accountId.ToString() , false);
            panel = UIManager.MainInstance.ShowPanel<MemberView>(member.accountId.ToString());
            panel.gameObject.transform.SetParent(View.TeamMember ,false);
            panel.MemberName.text = member.memberName;
        });
        
        
        
        SwitchTeamState(true);
        View.RoomId.text = "房间号:" + LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamId;
    }

    private void OnLevelTeamButtonClick()
    {
        
        ClearMembers();
        
        
        LobbyPlayerManager.MainInstance.teamManager.LeaveTeam();
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
        LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamMembers.ForEach(x =>
            UIManager.MainInstance.RemovePanel(x.accountId.ToString()));
        UIManager.MainInstance.RemovePanel(LobbyPlayerManager.MainInstance.teamManager.teamInfo.TeamOwner.accountId.ToString());
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
    
    
}