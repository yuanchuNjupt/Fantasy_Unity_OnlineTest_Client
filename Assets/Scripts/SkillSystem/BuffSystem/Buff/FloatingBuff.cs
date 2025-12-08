using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;
public class FloatingBuff : BuffComposite
{
    public FloatingBuff(Buff buff) : base(buff) { }
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
            FixInt floatingValue = mBuff.BuffCfg.buffParamsList[0].value;
            mBuff.attachTarget.AddRisingForce(floatingValue,mBuff.BuffCfg.buffDurationms);
        }
    }
}
