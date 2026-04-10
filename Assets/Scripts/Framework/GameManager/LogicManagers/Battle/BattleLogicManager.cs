using System;
using System.Collections.Generic;
using Battle.FrameOperate;
using Fantasy;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Core;
using FixMath;
using Framework.AdvancedLog;
using Framework.GameManager.Base;
using Framework.GameManager.Core;
using Framework.GameManager.DataManagers;
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
        
        [Inject]
        private FrameCommandDataManager _frameCommandDataManager;
        
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
        
        public void SendFrameOperateData()
        {
            // if (_battleDataManager.BattleState != BattleStateEnum.Start)
            // {
            //     return;
            // }
            //
            // var operateDataList = _battleDataManager.FrameOperationDataList;
            // FrameOperationData toSend;
            //
            // if (operateDataList.Count == 0)
            // {
            //     toSend = new FrameOperationData
            //     {
            //         operateType = (int)OperateTypeEnum.None,
            //         playerId = World.GetExitsDataManager<UserDataManager>().UserData.AccountId,
            //         sampleFrameId = LogicFrameConfig.ServerLogicFrameId
            //     };
            // }
            // else
            // {
            //     toSend = GetBestOperateData();
            //     // 强制对齐当前逻辑帧，避免缓存里的旧帧号污染
            //     toSend.sampleFrameId = LogicFrameConfig.ServerLogicFrameId;
            // }
            //
            // _battleMessageManager.SendFrameOperateEventMessage(_battleDataManager.BattleId, toSend);
            //
            // // 统一清理，防止跨帧残留
            // operateDataList.Clear();
        }
        
        public void CacheFrameOperateData(OperateTypeEnum operateType, FixedIntVector3 inputDir, int skillId)
        {
            // if (_battleDataManager.BattleState != BattleStateEnum.Start)
            // {
            //     return;
            // }
            //
            // var frameOperationData = new FrameOperationData
            // {
            //     operateType = (int)operateType,
            //     playerId = World.GetExitsDataManager<UserDataManager>().UserData.AccountId,
            // };
            //
            // switch (operateType)
            // {
            //     case OperateTypeEnum.None:
            //         break;
            //     case OperateTypeEnum.InputMove:
            //         frameOperationData.inputDir = new CSFixIntVector3
            //         {
            //             x = inputDir.X.Magnification,
            //             y = inputDir.Y.Magnification,
            //             z = inputDir.Z.Magnification
            //         };
            //         break;
            //     case OperateTypeEnum.ReleaseSkill:
            //         frameOperationData.skillId = skillId;
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException(nameof(operateType), operateType, null);
            // }
            //
            // var list = _battleDataManager.FrameOperationDataList;
            // list.Add(frameOperationData);
        }

        
        //接收到的从服务器下发的权威逻辑帧更新消息
        public void OnLogicFrameUpdateByServer(FrameOperateEventMessage_G2C message)
        {
            _battleDataManager.BattleState = BattleStateEnum.Start;
            _battleDataManager.BattleId = message.battleId;

            LogicFrameConfig.ServerLogicFrameId = Math.Max(LogicFrameConfig.ServerLogicFrameId, message.endLogicFrameId);
            
            if (message.oneFrameCommandList.Count == 0) return;
            
            //补帧、追帧、同步帧、回滚
            
            long? rollbackToFrameId = null;
            
            foreach (OneFrameCommand command in message.oneFrameCommandList)
            {
                var frameId = command.frameId;
                if (_frameCommandDataManager.AddCommand(frameId, FrameCommandType.Authoritative,
                        command.frameOperateDataList) && !rollbackToFrameId.HasValue)
                {
                    //需要进行回滚
                    rollbackToFrameId = frameId;
                    
                }
            }

            if (!rollbackToFrameId.HasValue) return; // 没有新增的权威帧数据，不需要回滚
            
            //进行回滚相关的处理
        }
        
        // 本地预测的逻辑帧更新
        private void OnLogicFrameUpdateByLocalPrediction()
        {
            if (_battleDataManager.BattleState != BattleStateEnum.Start)
                return;
        
            _battlePlayerLogicManager.OnLogicFrameUpdate();
            PhysicsManager3D.Instance.OnLogicFrameUpdate();
        }

        private void SendMessageToServer()
        {
            // var operateDataList = _battleDataManager.FrameOperationDataList;
            // _battleMessageManager.SendFrameOperateEventMessage(_battleDataManager.BattleId, operateDataList);
        }

       


        public void OnDestroy()
        {
        }
    }
}