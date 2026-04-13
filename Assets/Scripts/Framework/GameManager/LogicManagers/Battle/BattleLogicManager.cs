using System;
using System.Collections.Generic;
using Battle.FrameCommand;
using Fantasy;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Core;
using FixMath;
using Framework.AdvancedLog;
using Framework.GameManager.Base;
using Framework.GameManager.Core;
using Framework.GameManager.DataManagers;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.LogicManagers.FrameCommand;
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
        [Inject] private UserDataManager _userDataManager; 
        
        
        [Inject] private FrameCommandDataManager _frameCommandDataManager;
        [Inject] private FrameCommandLogicManager _frameCommandLogicManager;
        
        public void OnCreate()
        {
            
            _battleDataManager = World.GetExitsDataManager<BattleDataManager>();
            _battlePlayerLogicManager = World.GetExitsLogicManager<BattlePlayerLogicManager>();
            _battleMessageManager = World.GetExitsMessageManager<BattleMessageManager>();
            
            _frameCommandLogicManager.Init();
            
            Debug.Log("BattleLogicManager创建完成");
        }
        
        //收到开始战斗的消息
        public void OnStartBattle()
        {
            UIManager.MainInstance.AddPanel<BattleMainPanelView>(Resources.Load<GameObject>("UI/BattleMainPanel"), UILayer.Main , true);
            UIManager.MainInstance.ShowPanel<BattleMainPanelView>();
            _battlePlayerLogicManager.InitPlayer();
            
        }

        
        public void NoneFrameDataInput()
        {
            CacheFrameOperateData(OperateTypeEnum.None, FixedIntVector3.zero ,0);
        }
        
        public void MoveFrameDataInput(FixedIntVector3 inputDir)
        {
            CacheFrameOperateData(OperateTypeEnum.InputMove, inputDir ,0);
        }
        
        public void ReleaseSkillFrameData(int skillId)
        {
            CacheFrameOperateData(OperateTypeEnum.ReleaseSkill, FixedIntVector3.zero , skillId);
        }
        
        
        public void CacheFrameOperateData(OperateTypeEnum operateType, FixedIntVector3 inputDir, int skillId)
        {
            if (_battleDataManager.BattleState != BattleStateEnum.Start)
            {
                return;
            }
            
            var frameOperationData = new FrameOperationData
            {
                operateType = (int)operateType,
                playerId =_userDataManager.UserData.AccountId,
                skillId = skillId,
            };
            
            switch (operateType)
            {
                case OperateTypeEnum.None:
                    frameOperationData.inputDir = new CSFixIntVector3
                    {
                        x = 0,
                        y = 0,
                        z = 0
                    };
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
                    frameOperationData.inputDir = new CSFixIntVector3()
                    {
                        x = 0,
                        y = 0,
                        z = 0
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operateType), operateType, null);
            }

            var list = _frameCommandDataManager.currentFrameOperationData;
            list.Add(frameOperationData);
        }

        
        //接收到的从服务器下发的权威逻辑帧更新消息
        public void OnLogicFrameUpdateByServer(FrameOperateEventMessage_G2C message)
        {
            _battleDataManager.BattleState = BattleStateEnum.Start;
            _battleDataManager.BattleId = message.battleId;

            LogicFrameConfig.ServerLogicFrameId = Math.Max(LogicFrameConfig.ServerLogicFrameId, message.endLogicFrameId);
            _frameCommandLogicManager.OnInputFrameOperateData(message.serverTick);
            
            
            
            
            
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
            
            //取预测失败的帧的前一帧快照进行回滚
            if (_frameCommandLogicManager.Restore(rollbackToFrameId.Value - 1))
            {
                //回滚成功后，开始重新执行逻辑帧更新，直到追上服务器权威帧
                ExecuteRollback(rollbackToFrameId.Value);
            }
            
        }

        private void ExecuteRollback(long frameId)
        {
            //从第frameId帧开始，重播到客户端最新的预测帧数
            for (long replayerFrameId = frameId; replayerFrameId <= LogicFrameConfig.LocalPredictedLogicFrameId; replayerFrameId++)
            {
                _frameCommandLogicManager.ExecuteFrameCommand(_frameCommandDataManager[replayerFrameId]);
            }
            
            //全部执行完毕后再执行渲染更新
            //这里暂时每次更新都渲染，后续加个统一渲染接口
        }


        
        
        
        // 本地预测的逻辑帧更新
        public void OnLogicFrameUpdateByLocalPrediction(float deltaTime)
        {
            if (_battleDataManager.BattleState != BattleStateEnum.Start)
                return;

            int frameCount = _frameCommandLogicManager.FrameUpdate(deltaTime);
            // frameCount = Math.Min(frameCount, LogicFrameConfig.PreMaxMoveLogicFrameCount);
            
            
            
            for (int i = 0; i < frameCount; i++)
            {
                //执行逻辑帧更新
                LogicFrameConfig.LocalPredictedLogicFrameId++;
                
                //获取当前采集的输入数据，进行本地预测
                _frameCommandLogicManager.ExecutePredictionFrameCommand();
            }


        }


        public void OnDestroy()
        {
        }
    }
}