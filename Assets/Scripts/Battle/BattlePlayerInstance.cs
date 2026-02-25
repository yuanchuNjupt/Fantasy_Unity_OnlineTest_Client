using Config;
using Fantasy;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.DataManagers;
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
        
        private readonly UserDataManager _userDataManager;
        
        private readonly BattleDataManager _battleDataManager;
        
        private readonly TP_CameraLogicManager _cameraLogicManager;

        public BattlePlayerInstance(long uid , string playerName)
        {
            this.uid = uid;
            this.playerName = playerName;
            _userDataManager = World.GetExitsDataManager<UserDataManager>();
            _battleDataManager = World.GetExitsDataManager<BattleDataManager>();
            _cameraLogicManager = World.GetExitsLogicManager<TP_CameraLogicManager>();
            
            playerType = _userDataManager.UserData.AccountId == this.uid ? PlayerType.Self : PlayerType.Other;
            
            CreateLogicLayer();
            CreateRenderLayer();
            CreateTPCamera();
            if (playerType == PlayerType.Self)
            {
                CreateInputSampleLayer();
            }
            
        }
        
        
        private void CreateLogicLayer()
        {
            logicLayer = new BattlePlayerLogicLayer(this);
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
            renderLayer.Init(this);
            InitPlayerName();
            renderLayer.OnCreate();
        }
        
        private void CreateTPCamera()
        {
            _cameraLogicManager.InitTPCamera(renderLayer.transform);
        }
        
        private void CreateInputSampleLayer()
        {
            inputSampleLayer = renderLayer.gameObject.AddComponent<BattlePlayerInputSampleLayer>();
            inputSampleLayer.Init(this);
        }

        private void InitPlayerName()
        {
            LobbyPlayerName playerNameInstance = renderLayer.gameObject.GetComponent<LobbyPlayerName>();
            playerNameInstance.Init(playerName , _cameraLogicManager.cameraControl.transform);
        }

        //应用逻辑帧输入数据到角色实例
        public void ApplyFrameInput(FrameOperationData data)
        {
            switch ((OperateTypeEnum)data.operateType)
            {
                case OperateTypeEnum.InputMove:
                    logicLayer.ApplyMoveOperation(data.inputDir);
                    renderLayer.UpdateInputDir(data.inputDir);
                    break;
                case OperateTypeEnum.ReleaseSkill:
                    logicLayer.ApplyReleaseSkillOperation();
                    break;
            }
        }
        
        
        
        


        
        






    }
}