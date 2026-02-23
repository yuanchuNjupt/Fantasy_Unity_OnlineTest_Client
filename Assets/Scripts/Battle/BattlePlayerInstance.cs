using Config;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.DataManagers;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// 角色分为三个层面
    /// 逻辑层
    /// 渲染层
    /// 输入采样层
    /// </summary>
    public class BattlePlayerInstance
    {
        //唯一ID
        private readonly long _uid;
        
        public BattlePlayerLogicLayer logicLayer;
        
        public BattlePlayerRenderLayer renderLayer;
        
        public BattlePlayerInputSampleLayer inputSampleLayer;
        
        private readonly UserDataManager _userDataManager;
        
        private readonly BattleDataManager _battleDataManager;

        public BattlePlayerInstance(long uid)
        {
            _uid = uid;
            _userDataManager = World.GetExitsDataManager<UserDataManager>();
            _battleDataManager = World.GetExitsDataManager<BattleDataManager>();
            
            
            CreateLogicLayer();
            CreateRenderLayer();
        }
        
        
        private void CreateLogicLayer()
        {
            logicLayer = new BattlePlayerLogicLayer();
            logicLayer.OnCreate();
            
            var normalAttackConfigIdList = _battleDataManager.PlayerNormalAttackConfigIdList;
            var skillConfigIdList = _battleDataManager.PLayerSkillConfigIdList;
            logicLayer.InitActorSkill(normalAttackConfigIdList , skillConfigIdList);
        }

        private void CreateRenderLayer()
        {
            //实例化角色预制体
            GameObject go = Object.Instantiate(Resources.Load<GameObject>(LoadPathConfig.BattleModelName));
            renderLayer = go.GetComponent<BattlePlayerRenderLayer>();
            
            PlayerType playerType = _userDataManager.UserData.AccountId == _uid ? PlayerType.Self : PlayerType.Other;
            renderLayer.Init(playerType , this);
            renderLayer.OnCreate();
            
            // if (World.GetExitsDataManager<UserDataManager>().UserData.AccountId == player.playerId)
            // {
            //     // 先初始化摄像机，再调用 OnCreate
            //     World.GetExitsLogicManager<TP_CameraLogicManager>().InitTPCamera(renderLayerLayer.transform);
            //     renderLayerLayer.OnCreate();
            //     renderLayerLayer.Init(PlayerType.Self);
            // }
            // else
            // {
            //     renderLayerLayer.OnCreate();
            //     renderLayerLayer.Init(PlayerType.Other);
            // }
            
            
            
            
        }
        
        
        


        
        






    }
}