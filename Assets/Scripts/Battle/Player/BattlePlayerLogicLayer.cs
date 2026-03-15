using Battle.CustomCollider;
using Fantasy;
using FixedPhysics.Bounds;
using FixedPhysics.Fixed_pointNumber.Core;
using FixMath;
using Framework.AdvancedLog;
using Framework.GameManagerFramework.DataManagers;
using UIFramework.Core;
using UIFramework.ViewPath;
using UnityEngine;
using Log = Framework.AdvancedLog.Log;

namespace Battle
{
    public class BattlePlayerLogicLayer : LogicActor
    {
        
        public readonly BattlePlayerInstance instance;
        private readonly BattleMainPanelPresenter _battleMainPanelPresenter;
        
        public BattlePlayerLogicLayer(BattlePlayerInstance instance)
        {
            this.instance = instance;
            ObjectType = LogicObjectType.Hero;
            _battleMainPanelPresenter = UIManager.MainInstance.GetPanel<BattleMainPanelView>()
                .GetComponent<BattleMainPanelPresenter>();
            
            InitHp(100);
        }
        

        public void ApplyMoveOperation(CSFixIntVector3 csInputDir)
        {
            FixedIntVector3 inputDir = new FixedIntVector3(
                FixedInt.ConstructFromMagnification(csInputDir.x),
                FixedInt.ConstructFromMagnification(csInputDir.y),
                FixedInt.ConstructFromMagnification(csInputDir.z));
            Log.Info(LogColor.Green, "操作应用", 
                $"[{instance.playerName}] 应用移动操作: [{csInputDir.x}, {csInputDir.z}]");
            UpdateMoveDir(inputDir);
        }
        
        public void ApplyReleaseSkillOperation(int skillId)
        {
            Log.Info(LogColor.Green, "操作应用", 
                $"[{instance.playerName}] 应用攻击操作: skillId={skillId}");
            ReleaseSKill(skillId);
        }

        public void InitCollider(CylinderColliderBounds bound)
        {
            Collider = new BattlePlayerCollider(bound.radius,bound.height,LogicPos,bound.offset , this);
            
            Debug.Assert(Collider != null , "Collider != null");
            (Collider as BattlePlayerCollider).HostingCollider();
        }

        public override void OnHit(SkillDamageConfig config)
        {
            base.OnHit(config);
            if (instance.playerType is PlayerType.Self)
            {
                _battleMainPanelPresenter.UpdatePlayerHp(HP.RenderFloat , MAXHP.RenderFloat);
            }
            else
            {
                _battleMainPanelPresenter.UpdateEnemyHp(instance.uid , HP.RenderFloat , MAXHP.RenderFloat);
            }
            Log.Info(LogColor.Red , "收到攻击" , $"角色:{instance.playerName},受到伤害:{config.damageRate}");
        }

        public override void ReleaseNormalAttack()
        {
            if (LogicFrameConfig.IsUseLocalLogicFrame)
            {
                ReleaseSKill(normalSkillIdArr[curNormalComboIndex]);
            }
            else
            {
                //输入技能释放操作
                instance.battleLogicManager.ReleaseSkillFrameData(normalSkillIdArr[curNormalComboIndex]);
                
                
            }
            
        }
    }
}