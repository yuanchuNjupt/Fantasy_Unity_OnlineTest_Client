using System.Collections.Generic;
using System.Linq;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Generate;
using UIFramework.Core;
using UIFramework.ViewPath;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Lobby.TeamInfo
{
    public class TeamManagerComponent
    {
        //管理队伍相关，依附在大厅管理器之下

        //一个玩家的客户端只在乎自己所在的队伍信息,一个队伍就好
        public Team teamInfo;

        public async FTask<uint> CreateTeam()
        {
            var req = new CreateTeamRequest();
            req.playerId = Main.MainInstance.UserData.AccountId;

            var res = await NetWorkManager.Instance.Call<CreateTeamResponse>(req);

            if (res.ErrorCode != 0)
            {
                return res.ErrorCode;
            }

            teamInfo = new Team()
            {
                TeamId = res.teamId,
                TeamOwner = new TeamMemberInfo()
                {
                    accountId = Main.MainInstance.UserData.AccountId,
                    memberName = Main.MainInstance.UserData.AccountName,
                },
                TeamMembers = new List<TeamMemberInfo>(),
            };

            return 0;
        }

        public async FTask<uint> JoinTeam(long playerId, long teamId)
        {
            var req = new JoinTeamRequest();
            req.playerId = playerId;
            req.teamId = teamId;
            var res = await NetWorkManager.Instance.Call<JoinTeamResponse>(req);

            if (res.ErrorCode != 0)
            {
                Debug.Log("加入队伍失败，错误码：" + res.ErrorCode);
                return res.ErrorCode;
            }

            //加入队伍成功，更新本地队伍信息
            teamInfo = new Team()
            {
                TeamId = res.teamId,
                TeamOwner = new TeamMemberInfo()
                {
                    accountId = res.teamOwnerId,
                    memberName = LobbyPlayerManager.MainInstance.otherPlayers[res.teamOwnerId].PlayerName,
                },
            };
            //这里的成员是包含自己的
            List<TeamMemberInfo> members = new List<TeamMemberInfo>();
            res.teamMemberIds.ForEach(id =>
            {
                if (!LobbyPlayerManager.MainInstance.otherPlayers.TryGetValue(id, out var player))
                {
                    //说明是自己
                    //获取自己的信息
                    player = LobbyPlayerManager.MainInstance.selfPlayer;
                }
                
                members.Add(new TeamMemberInfo()
                {
                    accountId = id,
                    memberName = player.PlayerName,
                });
            });
            teamInfo.TeamMembers = members;
            return 0;



        }

        public void LeaveTeam()
        {
            //向服务器发送解散队伍 / 退出队伍信息
            var req = new TeamStateChangeMessage();
            
            //看看自己是不是队长 3 : 解散 2 : 退出
            req.teamState = Main.MainInstance.UserData.AccountId == teamInfo.TeamOwner.accountId ? 3 : 2;
            req.playerId = Main.MainInstance.UserData.AccountId;            
            
            NetWorkManager.Instance.Send(req);
            
            //本地清除队伍信息
            teamInfo = null;
        }

        private void AddMember(long playerId)
        {
            teamInfo.TeamMembers.Add(new TeamMemberInfo()
            {
                accountId = playerId,
                memberName = LobbyPlayerManager.MainInstance.otherPlayers[playerId].PlayerName,
            });

            //更新UI
            var panel = UIManager.MainInstance.GetPanel<LobbyPlayerPanelView>();
            panel.GetComponent<LobbyPlayerPanelPresenter>().AddMember(playerId);
        }

        private void OnOtherMemberLeave(long playerId)
        {
            //队伍中有其他成员退出
            var teamMemberInfo = teamInfo.TeamMembers.First(x => x.accountId == playerId);
            teamInfo.TeamMembers.Remove(teamMemberInfo);
            //更新UI
            UIManager.MainInstance.GetPanel<LobbyPlayerPanelView>()
                .GetComponent<LobbyPlayerPanelPresenter>().RemoveMember(playerId);
        }
        
        private void OnTeamOwnerDissolve()
        {
            //队长解散队伍
            //本地清除队伍信息

            UIManager.MainInstance.GetPanel<LobbyPlayerPanelView>()
                .GetComponent<LobbyPlayerPanelPresenter>().ClearMembers();
            
            //等待UI响应完毕后再清除数据
            //因为UI响应过程中会访问teamInfo数据
            teamInfo = null;
        }


        //处理队伍状态变更信息 
        public class TeamStateChangeHandler : Message<TeamStateChangeMessage>
        {
            protected override async FTask Run(Session session, TeamStateChangeMessage message)
            {
                
                Debug.Log("收到队伍状态变更消息，类型：" + message.teamState + " 玩家ID：" + message.playerId);
                switch (message.teamState)
                {
                    case 1:
                        Debug.Log("收到新玩家加入队伍消息");
                        LobbyPlayerManager.MainInstance.teamManager.AddMember(message.playerId);

                        break;

                    case 2:
                        Debug.Log("收到玩家退出队伍消息 , 玩家ID：" + message.playerId);
                        //从本地队伍信息中移除该成员
                        LobbyPlayerManager.MainInstance.teamManager.OnOtherMemberLeave(message.playerId);
                        break;

                    case 3:
                        Debug.Log("队长解散了队伍！");
                        LobbyPlayerManager.MainInstance.teamManager.OnTeamOwnerDissolve();
                        break;
                    default:
                        Debug.Log("未知的队伍状态变更类型：" + message.teamState);
                        break;
                }


                await FTask.CompletedTask;
            }
        }
    }
}