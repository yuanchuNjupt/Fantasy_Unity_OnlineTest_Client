using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using UIFramework.Core;
using UIFramework.ViewPath;
using UnityEngine;

namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class BattlePlayerLogicManager : ILogicBehaviour
    {
        private BattleDataManager _battleDataManager;
        
        public void OnCreate()
        {
            _battleDataManager = World.GetExitsDataManager<BattleDataManager>();
        }
        
        public void InitPlayer()
        {
            _battleDataManager.BattlePlayerDataList.ForEach(player =>
            {
                GameObject go = Object.Instantiate(Resources.Load<GameObject>("PlayerModel_NotSword"));
                




            });   
            
            
        }

        

        public void OnDestroy()
        {
        }
    }
}