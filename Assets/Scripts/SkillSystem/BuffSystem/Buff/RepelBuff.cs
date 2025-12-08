using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 击退buff
/// </summary>
public class RepelBuff : BuffComposite
{
    public RepelBuff(Buff buff) : base(buff) { }

    public override void BuffDelay()
    {
         
    }

    public override void BuffEnd()
    {
         
    }

    public override void BuffStart()
    {
        
    }

    public override void BuffTrigger()
    {
        if (mBuff.BuffCfg.buffParamsList.Count>0)
        {
            //获取击退距离
            FixInt repelValue = mBuff.BuffCfg.buffParamsList[0].value;
            FixIntVector3 endPos = new FixIntVector3(
                mBuff.releaser.LogicXAxis>0?mBuff.attachTarget.LogicPos.x+repelValue: mBuff.attachTarget.LogicPos.x-repelValue, 
                mBuff.attachTarget.LogicPos.y, 
                mBuff.attachTarget.LogicPos.z
                );
            // MoveToAction moveTo = new MoveToAction(mBuff.attachTarget, mBuff.attachTarget.LogicPos,
            //     endPos,mBuff.BuffCfg.buffDurationms,null,null, MoveType.X);
            //
            // LogicActionController.Instance.RunAciton(moveTo);
        }
    }
}
