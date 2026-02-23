using Fantasy;
using FixedPhysics.Fixed_pointNumber.Core;
using FixMath;
using Framework.GameManagerFramework.DataManagers;

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

        // public void InputFrameOperate(FrameOperationData data)
        // {
        //     if ((OperateTypeEnum)data.operateType == OperateTypeEnum.InputMove)
        //     {
        //         var CSinputDir = data.inputDir;
        //         FixedIntVector3 inputDir = new FixedIntVector3(new FixedInt((long)CSinputDir.x) ,new FixedInt((long)CSinputDir.y) ,new FixedInt((long)CSinputDir.z));
        //         UpdateMoveDir(inputDir);
        //         
        //     }
        //     else if((OperateTypeEnum)data.operateType == OperateTypeEnum.ReleaseSkill)
        //     {
        //         //TODO 释放技能逻辑
        //     }
        // }

        public void ApplyMoveOperation(CSFixIntVector3 csInputDir)
        {
            FixedIntVector3 inputDir = new FixedIntVector3(new FixedInt((long)csInputDir.x) ,new FixedInt((long)csInputDir.y) ,new FixedInt((long)csInputDir.z));
            UpdateMoveDir(inputDir);
        }
        
        public void ApplyReleaseSkillOperation()
        {
            //TODO:释放技能逻辑
            
            
        }
        
        
        
        
    }
}