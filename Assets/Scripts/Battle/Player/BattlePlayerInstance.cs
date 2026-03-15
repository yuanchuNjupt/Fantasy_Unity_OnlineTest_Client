using Config;
using Fantasy;
using FixedPhysics.Bounds;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.LogicManagers;
using Lobby;
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
        public readonly long uid;
        
        //角色昵称
        public readonly string playerName;
        
        public readonly PlayerType playerType;
        
        public BattlePlayerLogicLayer logicLayer;
        
        public BattlePlayerRenderLayer renderLayer;
        
        public BattlePlayerInputSampleLayer inputSampleLayer;
        
        public readonly UserDataManager userDataManager;
        
        public readonly BattleDataManager battleDataManager;

        public readonly BattleLogicManager battleLogicManager;
        
        public readonly TP_CameraLogicManager cameraLogicManager;
        
        public readonly BattlePlayerMouseLogicManager battleMouseLogicManager;

        public BattlePlayerInstance(long uid , string playerName)
        {
            this.uid = uid;
            this.playerName = playerName;
            userDataManager = World.GetExitsDataManager<UserDataManager>();
            battleDataManager = World.GetExitsDataManager<BattleDataManager>();
            cameraLogicManager = World.GetExitsLogicManager<TP_CameraLogicManager>();
            battleMouseLogicManager = World.GetExitsLogicManager<BattlePlayerMouseLogicManager>();
            battleLogicManager = World.GetExitsLogicManager<BattleLogicManager>();
            
            playerType = userDataManager.UserData.AccountId == this.uid ? PlayerType.Self : PlayerType.Other;
            
            CreateLogicLayer();
            CreateRenderLayer();
            
            logicLayer.SetRenderObj(renderLayer);
            logicLayer.InitCollider(renderLayer.gameObject.GetComponent<CylinderColliderBounds>());
            
            
            if (playerType == PlayerType.Self)
            {
                CreateTPCamera();   // 只有本地玩家才创建跟随相机
                CreateInputSampleLayer();
            }
            InitPlayerName();       // 相机已创建后再初始化头顶名字（依赖 cameraControl）
            
        }
        
        
        private void CreateLogicLayer()
        {
            logicLayer = new BattlePlayerLogicLayer(this);
            logicLayer.OnCreate();
            
            var normalAttackConfigIdList = battleDataManager.PlayerNormalAttackConfigIdList;
            var skillConfigIdList = battleDataManager.PLayerSkillConfigIdList;
            logicLayer.InitActorSkill(normalAttackConfigIdList , skillConfigIdList);
        }

        private void CreateRenderLayer()
        {
            //实例化角色预制体
            GameObject go = Object.Instantiate(Resources.Load<GameObject>(LoadPathConfig.BattleModelName));
            renderLayer = go.GetComponent<BattlePlayerRenderLayer>();
            renderLayer.OnCreate();  // 先调用OnCreate，确保_playerAnimator等组件引用已初始化
            renderLayer.Init(this);  // 再调用Init，此时PlayAnim可以安全使用_playerAnimator
        }
        
        private void CreateTPCamera()
        {
            // 传入战斗场景对应的 CameraLook Action
            var cameraLookAction = battleMouseLogicManager.CameraLookAction;
            cameraLogicManager.InitTPCamera(renderLayer.transform, cameraLookAction);
        }
        
        private void CreateInputSampleLayer()
        {
            inputSampleLayer = renderLayer.gameObject.AddComponent<BattlePlayerInputSampleLayer>();
            inputSampleLayer.Init(this);
        }

        private void InitPlayerName()
        {
            LobbyPlayerName playerNameInstance = renderLayer.gameObject.GetComponent<LobbyPlayerName>();
            // cameraControl 只有本地玩家才会初始化，非本地玩家时跳过头顶名字朝向绑定
            if (cameraLogicManager.cameraControl == null) return;
            playerNameInstance.Init(playerName , cameraLogicManager.cameraControl.transform);
        }

        //应用逻辑帧输入数据到角色实例
        public void ApplyFrameInput(FrameOperationData data)
        {
            switch ((OperateTypeEnum)data.operateType)
            {
                case OperateTypeEnum.InputMove:
                    logicLayer.ApplyMoveOperation(data.inputDir);
                    break;
                case OperateTypeEnum.ReleaseSkill:
                    logicLayer.ApplyReleaseSkillOperation(data.skillId);
                    break;
            }
        }
        
        
        
        
        
        
        


        
        






    }
}