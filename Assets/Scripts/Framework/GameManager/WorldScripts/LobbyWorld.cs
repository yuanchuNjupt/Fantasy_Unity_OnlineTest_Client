using Framework.GameManagerFramework.LogicManagers;

namespace Framework.GameManagerFramework.WorldScripts
{
    public class LobbyWorld : GameManager.Core.World
    {
        public override void OnCreate()
        {
            //创建了就是进入大厅
            GetExitsLogicManager<LobbyPlayerLogicManager>().EntryLobby();
        }
    }
}