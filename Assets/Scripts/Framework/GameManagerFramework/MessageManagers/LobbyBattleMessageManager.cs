using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using Generate;
using UIFramework.Core;
using UIFramework.ViewPath;

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

        public class EnterDungeonMessageHandler : Message<EnterDungeonMessage>
        {
            protected override async FTask Run(Session session, EnterDungeonMessage message)
            {
                //收到进入副本消息
                var panel = UIManager.MainInstance.ShowPanel<LoadingPanelView>();
                LoadSceneManager.MainInstance.LoadSceneAsync("Battle" , (progress) =>
                {
                    panel.SetProgress(progress);
                    //发送加载进度消息给服务器
                    World.GetExitsMessageManager<LobbyBattleMessageManager>().SendLoadProgressMessage(
                        World.GetExitsDataManager<LobbyTeamDataManager>().TeamInfo.TeamId,
                        World.GetExitsDataManager<UserDataManager>().UserData.AccountId,
                        progress / 100f
                        );
                });
                
                
                await FTask.CompletedTask;
            }
        }

        public class StartDungeonBattleMessageHandler : Message<StartDungeonBattleMessage>
        {
            protected override async FTask Run(Session session, StartDungeonBattleMessage message)
            {
                //收到开始战斗消息
                WorldManager.CreateWorld<BattleWorld>();
                UIManager.MainInstance.HideAllPanel("Main");
                //显示战斗界面UI
                UIManager.MainInstance.HidePanel<LoadingPanelView>();
                
                await FTask.CompletedTask;
            }
        }
        
        
    }
}