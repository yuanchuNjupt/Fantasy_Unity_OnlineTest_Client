using Framework.GameManager.Core;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using Framework.MessageManagers;

namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(LobbyWorld))]
    public class LobbyBattleLogicManager : ILogicBehaviour
    {
        
        private LobbyTeamDataManager _lobbyTeamDataManager;
        private LobbyBattleMessageManager _lobbyBattleMessageManager;
        
        public void OnCreate()
        {
            _lobbyBattleMessageManager = GameManager.Core.World.GetExitsMessageManager<LobbyBattleMessageManager>();
            _lobbyTeamDataManager = GameManager.Core.World.GetExitsDataManager<LobbyTeamDataManager>();
        }
        
        /// <summary>
        /// 按下进入副本按钮
        /// </summary>
        public void OnTeamLeaderEnterDungeon()
        {
            //只有队长才能进入副本
            //判断条件
            if (_lobbyTeamDataManager.TeamInfo == null)
            {
                UnityEngine.Debug.LogWarning("没有队伍信息，无法进入副本");
                return;
            }

            if (_lobbyTeamDataManager.TeamInfo.TeamOwner.accountId !=
                GameManager.Core.World.GetExitsDataManager<UserDataManager>().UserData.AccountId)
            {
                UnityEngine.Debug.LogWarning("只有队长才能进入副本!");
                return;
            }
            
            //一个人也能进入副本
            //发送进入副本的消息给服务器
            _lobbyBattleMessageManager.SendEnterDungeonMessage(_lobbyTeamDataManager.TeamInfo.TeamId);
        }
        
        public void OnDestroy()
        {
        }
        

    }
}