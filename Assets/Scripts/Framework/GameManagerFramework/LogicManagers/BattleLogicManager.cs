using System;
using Fantasy;
using FixMath;
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
            
            _battleDataManager = World.GetExitsDataManager<BattleDataManager>();
            _battlePlayerLogicManager = World.GetExitsLogicManager<BattlePlayerLogicManager>();
            _battleMessageManager = World.GetExitsMessageManager<BattleMessageManager>();
            Debug.Log("BattleLogicManager创建完成");
        }
        
        //收到开始战斗的消息
        public void OnStartBattle()
        {
            _battlePlayerLogicManager.InitPlayer();
            
        }

        //
        public void MoveFrameDataInput(FixIntVector3 inputDir)
        {
            SendFrameOperateData(OperateTypeEnum.InputMove, inputDir ,0 ,FixIntVector3.zero , SkillTypeEnum.None);
        }
        
        
        
        

        public void SendFrameOperateData(OperateTypeEnum operateType , FixIntVector3 inputDir , int skillId , FixIntVector3 skillPos , SkillTypeEnum skillType)
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
                    frameOperationData.inputDir = new CSFixIntVector3(){x = inputDir.x.IntValue, y = inputDir.y.IntValue, z = inputDir.z.IntValue};
                    break;
                case OperateTypeEnum.ReleaseSkill:
                    frameOperationData.skillId = skillId;
                    frameOperationData.skillPos = new CSFixIntVector3(){x =  inputDir.x.IntValue, y = inputDir.y.IntValue, z = inputDir.z.IntValue};
                    frameOperationData.skillType = (int)skillType;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operateType), operateType, null);
            }
            _battleDataManager.FrameOperationDataList.Add(frameOperationData);
            _battleMessageManager.SendFrameOperateEventMessage(_battleDataManager.BattleId , _battleDataManager.FrameOperationDataList);
            _battleDataManager.FrameOperationDataList.Clear();

        }
        
        public void OnLogicFrameUpdateByServer(FrameOperateEventMessage_G2C message)
        {
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