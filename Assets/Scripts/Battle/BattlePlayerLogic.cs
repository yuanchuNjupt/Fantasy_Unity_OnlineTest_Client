using Fantasy;
using FixMath;
using Framework.GameManagerFramework.DataManagers;

namespace Battle
{
    public class BattlePlayerLogic : LogicActor
    {
        public long PlayerId;
        
        public BattlePlayerLogic(long id , RenderObject renderObject)
        {
            PlayerId = id;
            RenderObj = renderObject;
            ObjectType = LogicObjectType.Hero;
        }

        public void InputFrameOperate(FrameOperationData data)
        {
            if ((OperateTypeEnum)data.operateType == OperateTypeEnum.InputMove)
            {
                var CSinputDir = data.inputDir;
                FixIntVector3 inputDir = new FixIntVector3(new FixInt((long)CSinputDir.x) ,new FixInt((long)CSinputDir.y) ,new FixInt((long)CSinputDir.z));
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