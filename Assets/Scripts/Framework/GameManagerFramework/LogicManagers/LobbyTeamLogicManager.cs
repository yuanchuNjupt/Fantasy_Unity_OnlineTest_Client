using Fantasy.Async;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using Framework.MessageManagers;
using Lobby.TeamInfo;
using UIFramework.Core;
using UIFramework.ViewPath;
using UnityEngine;


namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(LobbyWorld))]
    public class LobbyTeamLogicManager : ILogicBehaviour
    {
        private LobbyPlayerDataManager _lobbyPlayerDataManager;
        private LobbyTeamDataManager _lobbyTeamDataManager;
        private LobbyTeamMessageManager _lobbyTeamMessageManager;
        private UserDataManager _userDataManager;

        public void OnCreate()
        {
            _userDataManager = World.GetExitsDataManager<UserDataManager>();
            _lobbyPlayerDataManager = World.GetExitsDataManager<LobbyPlayerDataManager>();
            _lobbyTeamDataManager = World.GetExitsDataManager<LobbyTeamDataManager>();
            _lobbyTeamMessageManager = World.GetExitsMessageManager<LobbyTeamMessageManager>();
        }

        public async FTask<Team> CreateTeam()
        {
            var res = await _lobbyTeamMessageManager.SendCreateTeamRequest(_userDataManager.UserData.AccountId);

            if (res.ErrorCode != 0)
            {
                Debug.LogWarning("创建队伍失败，错误码：" + res.ErrorCode);
                return null;
            }

            _lobbyTeamDataManager.CreateTeam(res.teamId, _userDataManager.UserData.AccountId,
                _userDataManager.UserData.UserName);
            Debug.Log("创建队伍成功，队伍ID：" + res.teamId);

            return _lobbyTeamDataManager.TeamInfo;
        }

        public async FTask<Team> JoinTeam(string teamId)
        {
            //处理一下Id
            if (string.IsNullOrEmpty(teamId))
            {
                return null;
            }

            // 验证输入是否为有效的数字
            if (!long.TryParse(teamId, out long roomId))
            {
                Debug.LogWarning("房间号格式错误，请输入有效的数字");
                return null;
            }

            var res = await _lobbyTeamMessageManager.SendJoinTeamRequest(_userDataManager.UserData.AccountId, roomId);

            if (res.ErrorCode != 0)
            {
                Debug.Log("加入队伍失败，错误码：" + res.ErrorCode);
                return null;
            }

            string name = _lobbyPlayerDataManager.GetOtherPlayer(res.teamOwnerId).PlayerName;
            _lobbyTeamDataManager.CreateTeam(res.teamId, res.teamOwnerId, name);


            //加入队伍成功，更新本地队伍信息

            res.teamMemberIds.ForEach(id =>
            {
                name = _lobbyPlayerDataManager.GetOtherPlayer(id)?.PlayerName;
                if (name == null)
                {
                    //说明是自己
                    name = _userDataManager.UserData.UserName;
                }

                _lobbyTeamDataManager.AddTeamMember(id, name);
            });
            Debug.Log("加入小队成功，队伍ID : " + res.teamId);

            return _lobbyTeamDataManager.TeamInfo;
        }

        public void AddTeamMember(long playerId)
        {
            string name = _lobbyPlayerDataManager.GetOtherPlayer(playerId)?.PlayerName;
            if (name == null)
            {
                Debug.LogError("无法获取玩家名称，PlayerId:" + playerId);
                return;
            }
            _lobbyTeamDataManager.AddTeamMember(playerId, name);
            UIManager.MainInstance.GetPanel<LobbyPlayerPanelView>().GetComponent<LobbyPlayerPanelPresenter>()
                .AddMember(playerId, name);
        }

        public void RemoveTeamMember(long playerId)
        {
            //UI响应
            UIManager.MainInstance.GetPanel<LobbyPlayerPanelView>().GetComponent<LobbyPlayerPanelPresenter>()
                .RemoveMember(playerId);

            _lobbyTeamDataManager.RemoveTeamMember(playerId);
        }

        public void TeamOwnerDissolve()
        {
            UIManager.MainInstance.GetPanel<LobbyPlayerPanelView>().GetComponent<LobbyPlayerPanelPresenter>()
                .ClearMembers();
            _lobbyTeamDataManager.ClearTeam();
        }


        public void LeaveTeam()
        {
            _lobbyTeamMessageManager.SendLeaveTeamMessage();
            //本地清除队伍信息
            _lobbyTeamDataManager.ClearTeam();
        }


        public void OnDestroy()
        {
        }
    }
}