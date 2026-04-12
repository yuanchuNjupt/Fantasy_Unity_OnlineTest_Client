using System.Collections.Generic;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.LogicManagers;
using Framework.GameManagerFramework.WorldScripts;
using Generate;
using UnityEngine;

namespace Framework.MessageManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class BattleMessageManager : IMessageBehaviour
    {
        public void OnCreate()
        {
            
        }

        public void SendFrameOperateEventMessage(long battleId ,long frameId ,List<FrameOperationData> frameOperationData)
        {
            var message = new FrameOperateEventMessage_C2G();
            message.battleId = battleId;
            message.frameOperateDataList = frameOperationData;
            message.lastLogicFrameId = LogicFrameConfig.ServerLogicFrameId;
            message.predictLogicFrameId = frameId;
            NetWorkManager.Instance.Send(message);
        }
        
        public class FrameOperateEventMessageHandler : Message<FrameOperateEventMessage_G2C>
        {
            private BattleLogicManager _battleLogicManager;
            
            protected override async FTask Run(Session session, FrameOperateEventMessage_G2C message)
            {
                if (_battleLogicManager == null)
                {
                    _battleLogicManager = World.GetExitsLogicManager<BattleLogicManager>();
                }
                _battleLogicManager.OnLogicFrameUpdateByServer(message);

                await FTask.CompletedTask;
            }
        }
        
        
        
        

        public void OnDestroy()
        {
        }
    }
}