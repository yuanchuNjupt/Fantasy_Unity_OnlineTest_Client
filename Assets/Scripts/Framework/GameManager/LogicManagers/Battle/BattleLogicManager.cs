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
        public void SendFrameOperateData()
        {
            if (_battleDataManager.BattleState != BattleStateEnum.Start)
            {
                // 战斗未开始 / 已结束 不发送操作数据
                return;
            }

            var operateDataList = _battleDataManager.FrameOperationDataList;
            
            // 调试日志
            if (operateDataList.Count > 0)
            {
                string operateInfo = "操作列表: ";
                foreach (var op in operateDataList)
                {
                    operateInfo += $"[{(OperateTypeEnum)op.operateType}] ";
                }
                Log.Info(LogColor.Yellow, "网络同步", 
                    $"[LogicFrame:{LogicFrameConfig.LogicFrameId}] 发送 {operateDataList.Count} 个操作: {operateInfo}");
            }
            
            if (operateDataList.Count == 0)
            {
                // 没有操作数据，发送空操作列表
                _battleMessageManager.SendFrameOperateEventMessage(
                    _battleDataManager.BattleId,
                    new List<FrameOperationData>());
                return;
            }

            // 按权重排序操作（高权重优先发送）
            operateDataList.Sort((a, b) =>
                GetOperateWeight((OperateTypeEnum)b.operateType).CompareTo(
                    GetOperateWeight((OperateTypeEnum)a.operateType)));

            // 发送所有操作数据（不再丢弃）
            _battleMessageManager.SendFrameOperateEventMessage(
                _battleDataManager.BattleId,
                new List<FrameOperationData>(operateDataList));
            
            _battleDataManager.FrameOperationDataList.Clear();
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
        
        
        

        public void CacheFrameOperateData(OperateTypeEnum operateType , FixedIntVector3 inputDir , int skillId)
        {
            if (_battleDataManager.BattleState != BattleStateEnum.Start)
            {
                //战斗未开始 / 已结束 不发送操作数据
                return;
            }
            
            FrameOperationData frameOperationData = new FrameOperationData();
            frameOperationData.operateType = (int)operateType;
            frameOperationData.playerId = World.GetExitsDataManager<UserDataManager>().UserData.AccountId;
            switch (operateType)
            {
                case OperateTypeEnum.None:
                    break;
                case OperateTypeEnum.InputMove:
                    frameOperationData.inputDir = new CSFixIntVector3()
                    {
                        x = inputDir.X.Magnification,
                        y = inputDir.Y.Magnification,
                        z = inputDir.Z.Magnification
                    };
                    Log.Info(LogColor.Cyan, "网络同步", 
                        $"[采样帧] 缓存移动操作: [{inputDir.X.Magnification}, {inputDir.Z.Magnification}]");
                    break;
                case OperateTypeEnum.ReleaseSkill:
                    frameOperationData.skillId = skillId;
                    Log.Info(LogColor.Cyan, "网络同步", 
                        $"[采样帧] 缓存攻击操作: skillId={skillId}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operateType), operateType, null);
            }
            _battleDataManager.FrameOperationDataList.Add(frameOperationData);
           
        }
        
        
        //服务器下发收集到的所有玩家的上一帧的操作帧，将这些操作帧应用到玩家逻辑层，并调用逻辑帧更新接口
        public void OnLogicFrameUpdateByServer(FrameOperateEventMessage_G2C message)
        {
            //将玩家这一帧的操作数据发送给服务器
            SendFrameOperateData();
            
            LogicFrameConfig.LogicFrameId = message.logicFrameId;
            
            _battleDataManager.BattleState = BattleStateEnum.Start;
            
            _battleDataManager.BattleId = message.battleId;
            
            // 调试日志：记录接收到的所有操作
            string receivedOpsInfo = "接收操作列表: ";
            foreach (var data in message.frameOperateDataList)
            {
                receivedOpsInfo += $"[{(OperateTypeEnum)data.operateType}(pid:{data.playerId})] ";
            }
            Log.Info(LogColor.Green, "网络同步", 
                $"[LogicFrame:{LogicFrameConfig.LogicFrameId}] 共接收 {message.frameOperateDataList.Count} 个操作: {receivedOpsInfo}");
            
            //更新玩家输入
            message.frameOperateDataList.ForEach(data =>
            {
                var player = _battlePlayerLogicManager.GetBattlePlayerInstance(data.playerId);
                
                player.ApplyFrameInput(data);
                
            });
            
            
            //调用逻辑帧接口
            _battlePlayerLogicManager.OnLogicFrameUpdate();
            PhysicsManager3D.Instance.OnLogicFrameUpdate();
            
        }

       


        public void OnDestroy()
        {
        }
    }
}