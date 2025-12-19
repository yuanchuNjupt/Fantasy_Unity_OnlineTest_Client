using System.Collections.Generic;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
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

        public void SendFrameOperateEventMessage(long battleId , List<FrameOperationData> frameOperationDatas)
        {
            var message = new FrameOperateEventMessage_C2G();
            message.battleId = battleId;
            message.frameOperateDataList = frameOperationDatas;
            NetWorkManager.Instance.Send(message);
        }
        
        public class FrameOperateEventMessageHandler : Message<FrameOperateEventMessage_G2C>
        {
            private BattleLogicManager _battleLogicManager;
            
            protected override async FTask Run(Session session, FrameOperateEventMessage_G2C message)
            {
                
                Debug.Log("收到服务器帧操作数据同步 消息，战斗ID：" + message.battleId + " 操作数据数量：" + message.frameOperateDataList.Count);
                
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