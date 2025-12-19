using System.Collections.Generic;
using System.Linq;
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

        // public BattlePlayerLogic PlayerLogic;

        public List<BattlePlayerLogic> BattlePlayerLogicList = new List<BattlePlayerLogic>();
        
        

        public void OnCreate()
        {
            _battleDataManager = World.GetExitsDataManager<BattleDataManager>();
            Debug.Log("BattlePlayerLogicManager 创建完成");
        }

        public void InitPlayer()
        {
            _battleDataManager.BattlePlayerDataList.ForEach(player =>
            {
                GameObject go = Object.Instantiate(Resources.Load<GameObject>(LoadPathConfig.BattleModelName));
                BattlePlayerRender renderLayer = go.GetComponent<BattlePlayerRender>();
                var playerLogic = new BattlePlayerLogic(player.playerId, renderLayer);
                playerLogic.PlayerId = player.playerId;
                renderLayer.SetLogicObject(playerLogic);
                //初始化
                playerLogic.OnCreate();
                playerLogic.InitActorSkill(_battleDataManager.PlayerNormalAttackConfigIdList,
                    _battleDataManager.PLayerSkillConfigIdList);
                BattlePlayerLogicList.Add(playerLogic);

                if (World.GetExitsDataManager<UserDataManager>().UserData.AccountId == player.playerId)
                {
                    // 先初始化摄像机，再调用 OnCreate
                    World.GetExitsLogicManager<TP_CameraLogicManager>().InitTPCamera(renderLayer.transform);
                    renderLayer.OnCreate();
                    renderLayer.Init(PlayerType.Self);
                }
                else
                {
                    renderLayer.OnCreate();
                    renderLayer.Init(PlayerType.Other);
                }
            });
        }

        public BattlePlayerLogic GetBattlePlayerLogic(long playerId)
        {
            return BattlePlayerLogicList.First(player => player.PlayerId == playerId);
        }
        

        public void OnLogicFrameUpdate()
        {
            BattlePlayerLogicList.ForEach(logic => logic.OnLogicFrameUpdate());
        }


        public void OnDestroy()
        {
        }
    }
}