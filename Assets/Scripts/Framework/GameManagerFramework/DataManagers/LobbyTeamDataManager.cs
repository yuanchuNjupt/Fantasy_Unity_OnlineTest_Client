using System.Collections.Generic;
using Framework.GameManagerFramework.WorldScripts;
using Lobby.TeamInfo;

namespace Framework.GameManagerFramework.DataManagers
{
    [WorldSource(typeof(LobbyWorld))]
    public class LobbyTeamDataManager : IDataBehaviour
    {
        
        
        //一个玩家的客户端只在乎自己所在的队伍信息,一个队伍就好
        public Team TeamInfo { get; private set; }
        
        
        
        public void OnCreate()
        {
            
        }


        public void CreateTeam(long timeId , long ownerId , string ownerName)
        {
            TeamInfo = new Team()
            {
                TeamId = timeId,
                TeamOwner = new TeamMemberInfo()
                {
                    accountId = ownerId,
                    memberName = ownerName,
                },
                TeamMembers = new List<TeamMemberInfo>(),
            };
        }

        public void AddTeamMember(long playerId , string playerName)
        {   
            TeamInfo.TeamMembers.Add(new TeamMemberInfo()
            {
                accountId =  playerId,
                memberName = playerName,
            });
        }

        public void RemoveTeamMember(long playerId)
        {
            if (!TeamInfo.TeamMembers.Remove(TeamInfo.TeamMembers.Find(x => x.accountId == playerId)))
            {
                UnityEngine.Debug.LogWarning("玩家不存在，无法移除，PlayerId:" + playerId);
            }
        }

        public void ClearTeam()
        {
            TeamInfo = null;
        }
        
        

        public void OnDestroy()
        {
            TeamInfo = null;
        }
    }
}