using Fantasy;
using FixedPhysics.Fixed_pointNumber.Core;
using FixMath;
using Framework.GameManagerFramework.DataManagers;

namespace Battle
{
    public class BattlePlayerLogicLayer : LogicActor
    {
        
        public BattlePlayerInstance instance;
        
        public BattlePlayerLogicLayer()
        {
            ObjectType = LogicObjectType.Hero;
        }

        public void InputFrameOperate(FrameOperationData data)
        {
            if ((OperateTypeEnum)data.operateType == OperateTypeEnum.InputMove)
            {
                var CSinputDir = data.inputDir;
                FixedIntVector3 inputDir = new FixedIntVector3(new FixedInt((long)CSinputDir.x) ,new FixedInt((long)CSinputDir.y) ,new FixedInt((long)CSinputDir.z));
                InputLogicFrameEvent(inputDir);
                
                //更新其他玩家渲染层输入
                RenderObj.UpdateNetInputDir(inputDir);

            }
            else if((OperateTypeEnum)data.operateType == OperateTypeEnum.ReleaseSkill)
            {
                //TODO 释放技能逻辑
            }
        }
        
        
    }
}