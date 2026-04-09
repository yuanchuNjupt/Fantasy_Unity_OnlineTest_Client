using System.Collections.Generic;
using Fantasy;
using Framework.AdvancedLog;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.WorldScripts;
using Log = Framework.AdvancedLog.Log;

namespace Framework.GameManagerFramework.DataManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class BattleDataManager : IDataBehaviour
    {
        
        //战斗中玩家的数据列表,由服务器下发
        public List<BattlePlayerData> BattlePlayerDataList { get; private set; }
        
        // 用于在OnPlayerHit中正确判断自己，避免依赖UserData.AccountId
        public long CurrentPlayerIdInBattle { get; set; }
        
        //角色的普通攻击列表
        public List<int> PlayerNormalAttackConfigIdList = new List<int>(){1001,1002,1003,1004}; //暂时先写死
        
        //角色的技能列表
        public List<int> PLayerSkillConfigIdList = new List<int>() {}; //暂时先写死
        
        public BattleStateEnum BattleState = BattleStateEnum.None;
        
        //帧操作数据
        public List<FrameOperationData> FrameOperationDataList = new List<FrameOperationData>();

        public long BattleId;
        
        public void InitBattlePlayerData(List<BattlePlayerData> battlePlayerDataList)
        {
            BattlePlayerDataList = battlePlayerDataList;
            
            // 设置当前客户端的玩家ID（用UserData来做最终确认）
            var userDataManager = World.GetExitsDataManager<UserDataManager>();
            CurrentPlayerIdInBattle = userDataManager.UserData.AccountId;
            Log.Info(LogColor.Blue , "当前玩家UID" , CurrentPlayerIdInBattle.ToString());       
     
            
        }
        
        public void OnCreate()
        {
            BattlePlayerDataList = new List<BattlePlayerData>();
        }

        public void OnDestroy()
        {
        }
    }

    public enum BattleStateEnum
    {
        None,
        Start,
        End,
    }

    public enum OperateTypeEnum
    {
        None,
        InputMove,
        ReleaseSkill,
    }
    
    public enum SkillTypeEnum
    {
        None,
        ClickSkill,
        GuideSkill,
        StockPileSkill,
    }
    
}