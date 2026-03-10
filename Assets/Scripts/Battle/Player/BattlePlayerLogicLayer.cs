using Battle.CustomCollider;
using Fantasy;
using FixedPhysics.Bounds;
using FixedPhysics.Fixed_pointNumber.Core;
using FixMath;
using Framework.AdvancedLog;
using Framework.GameManagerFramework.DataManagers;
using UnityEngine;
using Log = Framework.AdvancedLog.Log;

namespace Battle
{
    public class BattlePlayerLogicLayer : LogicActor
    {
        
        public BattlePlayerInstance instance;
        
        public BattlePlayerLogicLayer(BattlePlayerInstance instance)
        {
            this.instance = instance;
            ObjectType = LogicObjectType.Hero;
        }
        

        public void ApplyMoveOperation(CSFixIntVector3 csInputDir)
        {
            FixedIntVector3 inputDir = new FixedIntVector3(
                FixedInt.ConstructFromMagnification(csInputDir.x),
                FixedInt.ConstructFromMagnification(csInputDir.y),
                FixedInt.ConstructFromMagnification(csInputDir.z));
            UpdateMoveDir(inputDir);
        }
        
        public void ApplyReleaseSkillOperation()
        {
            //TODO:释放技能逻辑
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
            Log.Info(LogColor.Purple , "战斗系统" , $"角色:{instance.playerName},受到伤害:{config.damageRate}");
        }
    }
}