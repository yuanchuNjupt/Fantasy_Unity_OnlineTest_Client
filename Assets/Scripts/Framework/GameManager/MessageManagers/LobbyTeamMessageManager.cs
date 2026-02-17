using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.LogicManagers;
using Framework.GameManagerFramework.WorldScripts;
using Generate;
using UnityEngine;

namespace Framework.MessageManagers
{
    [WorldSource(typeof(LobbyWorld))]
    public class LobbyTeamMessageManager : IMessageBehaviour
    {
        public void OnCreate()
        {
        }

        public async FTask<CreateTeamResponse> SendCreateTeamRequest(long playerId)
        {
            var req = new CreateTeamRequest();
            req.playerId = playerId;
            return await NetWorkManager.Instance.Call<CreateTeamResponse>(req);
        }

        public async FTask<JoinTeamResponse> SendJoinTeamRequest(long playerId, long teamId)
        {
            var req = new JoinTeamRequest();
            req.playerId = playerId;
            req.teamId = teamId;
            return await NetWorkManager.Instance.Call<JoinTeamResponse>(req);
        }

        public void SendLeaveTeamMessage()
        {
            //向服务器发送解散队伍 / 退出队伍信息
            var req = new TeamStateChangeMessage();
            //看看自己是不是队长 3 : 解散 2 : 退出
            req.teamState = GameManager.Core.World.GetExitsDataManager<UserDataManager>().UserData.AccountId ==
                            GameManager.Core.World.GetExitsDataManager<LobbyTeamDataManager>().TeamInfo.TeamOwner.accountId
                ? 3
                : 2;
            req.playerId = GameManager.Core.World.GetExitsDataManager<UserDataManager>().UserData.AccountId;

            NetWorkManager.Instance.Send(req);
        }
        
        //处理队伍状态变更信息 
        public class TeamStateChangeHandler : Message<TeamStateChangeMessage>
        {
            protected override async FTask Run(Session session, TeamStateChangeMessage message)
            {
                
                LobbyTeamLogicManager lobbyTeamLogicManager = GameManager.Core.World.GetExitsLogicManager<LobbyTeamLogicManager>();
                
                Debug.Log("收到队伍状态变更消息，类型：" + message.teamState + " 玩家ID：" + message.playerId);
                switch (message.teamState)
                {
                    case 1:
                        Debug.Log("收到新玩家加入队伍消息");
                        lobbyTeamLogicManager.AddTeamMember(message.playerId);
                        break;

                    case 2:
                        Debug.Log("收到玩家退出队伍消息 , 玩家ID：" + message.playerId);
                        //从本地队伍信息中移除该成员
                        lobbyTeamLogicManager.RemoveTeamMember(message.playerId);
                        break;

                    case 3:
                        Debug.Log("队长解散了队伍！");
                        lobbyTeamLogicManager.TeamOwnerDissolve();
                        break;
                    default:
                        Debug.Log("未知的队伍状态变更类型：" + message.teamState);
                        break;
                }


                await FTask.CompletedTask;
            }
        }


        public void OnDestroy()
        {
        }
    }
}