using Battle;
using Config;
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

        public BattlePlayerLogic PlayerLogic;
        
        public void OnCreate()
        {
            _battleDataManager = World.GetExitsDataManager<BattleDataManager>();
        }
        
        public void InitPlayer()
        {
            _battleDataManager.BattlePlayerDataList.ForEach(player =>
            {
                GameObject go = Object.Instantiate(Resources.Load<GameObject>(LoadPathConfig.BattleModelName));
                BattlePlayerRender renderLayer = go.GetComponent<BattlePlayerRender>();
                PlayerLogic = new BattlePlayerLogic();
                PlayerLogic.PlayerId = player.playerId;
                renderLayer.SetLogicObject(PlayerLogic);
                //初始化
                PlayerLogic.OnCreate();
                renderLayer.OnCreate();
                renderLayer.Init(World.GetExitsDataManager<UserDataManager>().UserData.AccountId == player.playerId
                    ? PlayerType.Self
                    : PlayerType.Other);
            });   
            
            
           

            
        }
        
        public void OnLogicFrameUpdate()
        {
            PlayerLogic.OnLogicFrameUpdate();
        }
        
        

        

        public void OnDestroy()
        {
        }
    }
}