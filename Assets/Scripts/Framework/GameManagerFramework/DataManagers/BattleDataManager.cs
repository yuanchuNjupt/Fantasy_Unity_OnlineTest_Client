using System.Collections.Generic;
using Fantasy;
using Framework.GameManagerFramework.WorldScripts;

namespace Framework.GameManagerFramework.DataManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class BattleDataManager : IDataBehaviour
    {
        
        public List<BattlePlayerData> BattlePlayerDataList { get; private set; }
        
        //角色的普通攻击列表
        public List<int> PlayerNormalAttackConfigIdList = new List<int>(){1001,1002}; //暂时先写死
        
        //角色的技能列表
        public List<int> PLayerSkillConfigIdList = new List<int>() {}; //暂时先写死
        
        public BattleStateEnum BattleState = BattleStateEnum.None;
        
        //帧操作数据
        public List<FrameOperationData> FrameOperationDataList = new List<FrameOperationData>();

        public long BattleId;
        
        public void InitBattlePlayerData(List<BattlePlayerData> battlePlayerDataList)
        {
            BattlePlayerDataList = battlePlayerDataList;
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