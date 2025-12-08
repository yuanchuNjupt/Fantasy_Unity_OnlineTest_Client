using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreGravityBuff : BuffComposite
{
    public IgnoreGravityBuff(Buff buff) : base(buff) { }

    public override void BuffDelay()
    {

    }


    public override void BuffStart()
    {

    }

    public override void BuffTrigger()
    {
        mBuff.attachTarget.IgnoreGravity = true;
    }

    public override void BuffEnd()
    {
        mBuff.attachTarget.IgnoreGravity = false;
    }
}
