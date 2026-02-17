using System;
using System.Collections.Generic;
using Fantasy;
using FixedPhysics.Fixed_pointNumber.Core;
using FixMath;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using Framework.MessageManagers;
using UnityEngine;

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
            
            _battleDataManager = GameManager.Core.World.GetExitsDataManager<BattleDataManager>();
            _battlePlayerLogicManager = GameManager.Core.World.GetExitsLogicManager<BattlePlayerLogicManager>();
            _battleMessageManager = GameManager.Core.World.GetExitsMessageManager<BattleMessageManager>();
            Debug.Log("BattleLogicManager创建完成");
        }
        
        //收到开始战斗的消息
        public void OnStartBattle()
        {
            _battlePlayerLogicManager.InitPlayer();
            
        }

        //
        public void MoveFrameDataInput(FixedIntVector3 inputDir)
        {
            CacheFrameOperateData(OperateTypeEnum.InputMove, inputDir ,0 ,FixedIntVector3.zero , SkillTypeEnum.None);
        }


        //接收到服务器的逻辑帧更新消息后，发送当前帧的操作数据给服务器
        public void SendFrameOperateData()
        {
            
            if (_battleDataManager.BattleState != BattleStateEnum.Start)
            {
                //战斗未开始 / 已结束 不发送操作数据
                return;
            }
            
            var operateDataList = _battleDataManager.FrameOperationDataList;
            //将最权威的数据发送给服务器
            //找到权重最高的操作数据
            for (int i = operateDataList.Count - 1; i >= 0; i--)
            {
                if (operateDataList[i].operateType != (int)OperateTypeEnum.None)
                {
                    //找到了最新的操作数据
                    _battleMessageManager.SendFrameOperateEventMessage(_battleDataManager.BattleId ,new List<FrameOperationData>() {operateDataList[i]});
                    _battleDataManager.FrameOperationDataList.Clear();
                    return;
                }
            }
            
            //没有任何操作数据
            _battleMessageManager.SendFrameOperateEventMessage(_battleDataManager.BattleId ,new List<FrameOperationData>(){operateDataList[0]});
            _battleDataManager.FrameOperationDataList.Clear();
            
            
        }
        
        

        public void CacheFrameOperateData(OperateTypeEnum operateType , FixedIntVector3 inputDir , int skillId , FixedIntVector3 skillPos , SkillTypeEnum skillType)
        {
            if (_battleDataManager.BattleState != BattleStateEnum.Start)
            {
                //战斗未开始 / 已结束 不发送操作数据
                return;
            }
            
            FrameOperationData frameOperationData = new FrameOperationData();
            frameOperationData.operateType = (int)operateType;
            frameOperationData.playerId = GameManager.Core.World.GetExitsDataManager<UserDataManager>().UserData.AccountId;
            switch (operateType)
            {
                case OperateTypeEnum.None:
                    break;
                case OperateTypeEnum.InputMove:
                    frameOperationData.inputDir = new CSFixIntVector3(){x = inputDir.X.RenderInt, y = inputDir.Y.RenderInt, z = inputDir.Z.RenderInt};
                    break;
                case OperateTypeEnum.ReleaseSkill:
                    frameOperationData.skillId = skillId;
                    frameOperationData.skillPos = new CSFixIntVector3(){x =  inputDir.X.RenderInt, y = inputDir.Y.RenderInt, z = inputDir.Z.RenderInt};
                    frameOperationData.skillType = (int)skillType;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operateType), operateType, null);
            }
            _battleDataManager.FrameOperationDataList.Add(frameOperationData);
           
        }
        
        public void OnLogicFrameUpdateByServer(FrameOperateEventMessage_G2C message)
        {
            //将玩家当前的操作数据发送给服务器
            SendFrameOperateData();
            LogicFrameConfig.LogicFrameid = message.logicFrameId;
            _battleDataManager.BattleState = BattleStateEnum.Start;
            _battleDataManager.BattleId = message.battleId;
            
            //更新玩家输入
            message.frameOperateDataList.ForEach(data =>
            {
                var player = _battlePlayerLogicManager.GetBattlePlayerLogic(data.playerId);
                
                player.InputFrameOperate(data);
                
            });
            
            
            //调用逻辑帧接口
            _battlePlayerLogicManager.OnLogicFrameUpdate();
            
        }

       


        public void OnDestroy()
        {
        }
    }
}