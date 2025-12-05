using Framework.GameManagerFramework.WorldScripts;
using Lobby.TeamInfo;

namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(LobbyWorld))]
    public class LobbyTeamLogicManager : ILogicBehaviour
    {
        //一个玩家的客户端只在乎自己所在的队伍信息,一个队伍就好
        public Team teamInfo;
        
        public void OnCreate()
        {
            
        }

        public void OnDestroy()
        {
            
        }
    }
}