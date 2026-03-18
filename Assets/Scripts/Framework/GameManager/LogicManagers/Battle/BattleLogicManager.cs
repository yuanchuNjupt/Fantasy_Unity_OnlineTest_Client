using System;
using System.Collections.Generic;
using Fantasy;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Core;
using FixMath;
using Framework.AdvancedLog;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using Framework.MessageManagers;
using UIFramework.Core;
using UIFramework.ViewPath;
using UnityEngine;
using Log = Framework.AdvancedLog.Log;


namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class BattleLogicManager : ILogicBehaviour
    {
        
        private BattleDataManager _battleDataManager;
        private BattlePlayerLogicManager _battlePlayerLogicManager;
        private BattleMessageManager _battleMessageManager;
        
        public void OnCreate()
        {
            
            _battleDataManager = World.GetExitsDataManager<BattleDataManager>();
            _battlePlayerLogicManager = World.GetExitsLogicManager<BattlePlayerLogicManager>();
            _battleMessageManager = World.GetExitsMessageManager<BattleMessageManager>();
            Debug.Log("BattleLogicManager创建完成");
        }
        
        //收到开始战斗的消息
        public void OnStartBattle()
        {
            UIManager.MainInstance.AddPanel<BattleMainPanelView>(Resources.Load<GameObject>("UI/BattleMainPanel"), UILayer.Main , true);
            UIManager.MainInstance.ShowPanel<BattleMainPanelView>();
            _battlePlayerLogicManager.InitPlayer();
            
        }

        //
        public void MoveFrameDataInput(FixedIntVector3 inputDir)
        {
            CacheFrameOperateData(OperateTypeEnum.InputMove, inputDir ,0);
        }
        
        public void ReleaseSkillFrameData(int skillId)
        {
            CacheFrameOperateData(OperateTypeEnum.ReleaseSkill, FixedIntVector3.zero , skillId);
        }


        /// <summary>
        /// 操作类型权重，数值越大优先级越高
        /// ReleaseSkill(攻击) > InputMove(移动) > None
        /// </summary>
        private static int GetOperateWeight(OperateTypeEnum type)
        {
            switch (type)
            {
                case OperateTypeEnum.ReleaseSkill: return 2;
                case OperateTypeEnum.InputMove:    return 1;
                default:                           return 0;
            }
        }

        // 接收到服务器的逻辑帧更新消息后，发送当前帧的操作数据给服务器
        // public void SendFrameOperateData()
        // {
        //     if (_battleDataManager.BattleState != BattleStateEnum.Start)
        //     {
        //         // 战斗未开始 / 已结束 不发送操作数据
        //         return;
        //     }
        //
        //     var operateDataList = _battleDataManager.FrameOperationDataList;
        //     
        //     if (operateDataList.Count == 0)
        //     {
        //         // 没有操作数据，发送空操作列表
        //         _battleMessageManager.SendFrameOperateEventMessage(
        //             _battleDataManager.BattleId,
        //             new FrameOperationData()
        //             {
        //                 operateType = (int)(OperateTypeEnum.None),
        //                 playerId = World.GetExitsDataManager<UserDataManager>().UserData.AccountId,
        //                 sampleFrameId = LogicFrameConfig.LogicFrameId
        //             });
        //         return;
        //     }
        //     
        //     _battleMessageManager.SendFrameOperateEventMessage(
        //         _battleDataManager.BattleId,
        //         GetBestOperateData());
        //     
        //     _battleDataManager.FrameOperationDataList.Clear();
        // }
        
        public void SendFrameOperateData()
        {
            if (_battleDataManager.BattleState != BattleStateEnum.Start)
            {
                return;
            }
        
            var operateDataList = _battleDataManager.FrameOperationDataList;
            FrameOperationData toSend;
        
            if (operateDataList.Count == 0)
            {
                toSend = new FrameOperationData
                {
                    operateType = (int)OperateTypeEnum.None,
                    playerId = World.GetExitsDataManager<UserDataManager>().UserData.AccountId,
                    sampleFrameId = LogicFrameConfig.LogicFrameId
                };
            }
            else
            {
                toSend = GetBestOperateData();
                // 强制对齐当前逻辑帧，避免缓存里的旧帧号污染
                toSend.sampleFrameId = LogicFrameConfig.LogicFrameId;
            }
        
            _battleMessageManager.SendFrameOperateEventMessage(_battleDataManager.BattleId, toSend);
        
            // 统一清理，防止跨帧残留
            operateDataList.Clear();
        }
        

        private FrameOperationData GetBestOperateData()
        {
            var operateDataList = _battleDataManager.FrameOperationDataList;

            // 遍历找到权重最高的操作数据
            // 同权重时取最新（靠后）的一条
            FrameOperationData best = operateDataList[0];
            int bestWeight = GetOperateWeight((OperateTypeEnum)best.operateType);

            for (int i = 1; i < operateDataList.Count; i++)
            {
                int w = GetOperateWeight((OperateTypeEnum)operateDataList[i].operateType);
                if (w >= bestWeight)
                {
                    best = operateDataList[i];
                    bestWeight = w;
                }
            }
            
            return  best;
        }
        
        
        

        // public void CacheFrameOperateData(OperateTypeEnum operateType , FixedIntVector3 inputDir , int skillId)
        // {
        //     if (_battleDataManager.BattleState != BattleStateEnum.Start)
        //     {
        //         //战斗未开始 / 已结束 不发送操作数据
        //         return;
        //     }
        //     
        //     FrameOperationData frameOperationData = new FrameOperationData();
        //     frameOperationData.operateType = (int)operateType;
        //     frameOperationData.playerId = World.GetExitsDataManager<UserDataManager>().UserData.AccountId;
        //     frameOperationData.sampleFrameId = LogicFrameConfig.LogicFrameId;
        //     switch (operateType)
        //     {
        //         case OperateTypeEnum.None:
        //             break;
        //         case OperateTypeEnum.InputMove:
        //             frameOperationData.inputDir = new CSFixIntVector3()
        //             {
        //                 x = inputDir.X.Magnification,
        //                 y = inputDir.Y.Magnification,
        //                 z = inputDir.Z.Magnification
        //             };
        //             break;
        //         case OperateTypeEnum.ReleaseSkill:
        //             frameOperationData.skillId = skillId;
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(operateType), operateType, null);
        //     }
        //     _battleDataManager.FrameOperationDataList.Add(frameOperationData);
        //    
        // }
        
        
        public void CacheFrameOperateData(OperateTypeEnum operateType, FixedIntVector3 inputDir, int skillId)
        {
            if (_battleDataManager.BattleState != BattleStateEnum.Start)
            {
                return;
            }
        
            var frameOperationData = new FrameOperationData
            {
                operateType = (int)operateType,
                playerId = World.GetExitsDataManager<UserDataManager>().UserData.AccountId,
                sampleFrameId = LogicFrameConfig.LogicFrameId
            };
        
            switch (operateType)
            {
                case OperateTypeEnum.None:
                    break;
                case OperateTypeEnum.InputMove:
                    frameOperationData.inputDir = new CSFixIntVector3
                    {
                        x = inputDir.X.Magnification,
                        y = inputDir.Y.Magnification,
                        z = inputDir.Z.Magnification
                    };
                    break;
                case OperateTypeEnum.ReleaseSkill:
                    frameOperationData.skillId = skillId;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operateType), operateType, null);
            }
        
            var list = _battleDataManager.FrameOperationDataList;
        
            // 同一采样帧只保留“最权威”一条，避免一个帧上报多条
            if (list.Count == 0)
            {
                list.Add(frameOperationData);
                return;
            }
        
            // 找到同 sampleFrameId 的最后一条进行权重覆盖
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].sampleFrameId != frameOperationData.sampleFrameId)
                {
                    continue;
                }
        
                int oldW = GetOperateWeight((OperateTypeEnum)list[i].operateType);
                int newW = GetOperateWeight((OperateTypeEnum)frameOperationData.operateType);
        
                if (newW >= oldW)
                {
                    list[i] = frameOperationData;
                }
        
                return;
            }
        
            // 当前缓存里没有同帧数据，追加
            list.Add(frameOperationData);
        }
        
        
        
        
        //服务器下发收集到的所有玩家的上一帧的操作帧，将这些操作帧应用到玩家逻辑层，并调用逻辑帧更新接口
        // public void OnLogicFrameUpdateByServer(FrameOperateEventMessage_G2C message)
        // {
        //     //将玩家这一帧的操作数据发送给服务器
        //     SendFrameOperateData();
        //     
        //     LogicFrameConfig.LogicFrameId = message.logicFrameId;
        //     
        //     _battleDataManager.BattleState = BattleStateEnum.Start;
        //     
        //     _battleDataManager.BattleId = message.battleId;
        //     
        //     // 调试日志：记录接收到的所有操作
        //     string receivedOpsInfo = "接收操作列表: ";
        //     foreach (var data in message.frameOperateDataList)
        //     {
        //         receivedOpsInfo += $"[{(OperateTypeEnum)data.operateType}(pid:{data.playerId})] ";
        //     }
        //     Log.Info(LogColor.Green, "网络同步", 
        //         $"[LogicFrame:{LogicFrameConfig.LogicFrameId}] 共接收 {message.frameOperateDataList.Count} 个操作: {receivedOpsInfo}");
        //     
        //     //更新玩家输入
        //     message.frameOperateDataList.ForEach(data =>
        //     {
        //         var player = _battlePlayerLogicManager.GetBattlePlayerInstance(data.playerId);
        //         
        //         player.ApplyFrameInput(data);
        //         
        //     });
        //     
        //     
        //     //调用逻辑帧接口
        //     _battlePlayerLogicManager.OnLogicFrameUpdate();
        //     PhysicsManager3D.Instance.OnLogicFrameUpdate();
        //     
        // }
        
        public void OnLogicFrameUpdateByServer(FrameOperateEventMessage_G2C message)
        {
            _battleDataManager.BattleState = BattleStateEnum.Start;
            _battleDataManager.BattleId = message.battleId;
        
            // 关键：先对齐服务器逻辑帧，再发送本地输入

            if (message.logicFrameId != LogicFrameConfig.LogicFrameId + 1)
            {
                Log.Error("网络同步", $"逻辑帧对齐异常！服务器下发的逻辑帧ID={message.logicFrameId}，本地当前逻辑帧ID={LogicFrameConfig.LogicFrameId}");
                Application.Quit();
            }
            
            LogicFrameConfig.LogicFrameId = message.logicFrameId;
        
            // 将“本逻辑帧采样结果”上报给服务器
            SendFrameOperateData();
        
            // string receivedOpsInfo = "接收操作列表: ";
            // foreach (var data in message.frameOperateDataList)
            // {
            //     receivedOpsInfo += $"[{(OperateTypeEnum)data.operateType}(pid:{data.playerId}) sf:{data.sampleFrameId}] ";
            // }
        
            // Log.Info(LogColor.Green, "网络同步",
            //     $"[LogicFrame:{LogicFrameConfig.LogicFrameId}] 共接收 {message.frameOperateDataList.Count} 个操作: {receivedOpsInfo}");
        
            // 应用服务器下发操作
            message.frameOperateDataList.ForEach(data =>
            {
                var player = _battlePlayerLogicManager.GetBattlePlayerInstance(data.playerId);
                if (player == null)
                {
                    Log.Warning(LogColor.Yellow, "网络同步", $"找不到玩家实例，pid={data.playerId}");
                    return;
                }
        
                player.ApplyFrameInput(data);
            });
        
            
            _battlePlayerLogicManager.OnLogicFrameUpdate();
            PhysicsManager3D.Instance.OnLogicFrameUpdate();
        }
        

       


        public void OnDestroy()
        {
        }
    }
}