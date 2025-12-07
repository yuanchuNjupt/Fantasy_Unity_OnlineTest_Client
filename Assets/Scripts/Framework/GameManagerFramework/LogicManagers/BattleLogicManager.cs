using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using UnityEngine;

namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class BattleLogicManager : ILogicBehaviour
    {
        
        private BattleDataManager _battleDataManager;
        
        public void OnCreate()
        {
            _battleDataManager = World.GetExitsDataManager<BattleDataManager>();
        }
        
        //收到开始战斗的消息
        public void OnStartBattle()
        {
            
        }

       


        public void OnDestroy()
        {
        }
    }
}