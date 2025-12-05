using Fantasy;
using Framework.GameManagerFramework.WorldScripts;
using Generate;

namespace Framework.MessageManagers
{
    [WorldSource(typeof(LobbyWorld))]
    public class LobbyBattleMessageManager : IMessageBehaviour
    {
        public void OnCreate()
        {
            
            
        }
        
        public void SendEnterDungeonMessage(long teamId)
        {
            var message = new EnterDungeonMessage();
            message.teamId = teamId;
            NetWorkManager.Instance.Send(message);
        }

        public void SendLoadProgressMessage(long teamId , long playerId , float progress)
        {
            var message = new LoadDungeonProgressMessage();
            message.teamId = teamId;
            message.playerId = playerId;
            message.progress = progress;
            NetWorkManager.Instance.Send(message);
        }
        

        public void OnDestroy()
        {
            
        }
    }
}