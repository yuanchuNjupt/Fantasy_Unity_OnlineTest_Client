using System.Collections.Generic;
using Fantasy;
using Framework.GameManagerFramework.WorldScripts;

namespace Framework.GameManagerFramework.DataManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class BattleDataManager : IDataBehaviour
    {
        
        public List<BattlePlayerData> BattlePlayerDataList { get; private set; }
        
        
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
}