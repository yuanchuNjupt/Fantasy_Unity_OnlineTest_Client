using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusModify_Buff_Single : BuffComposite
{
    public StatusModify_Buff_Single(Buff buff) : base(buff) { }


    public override void BuffDelay()
    {

    }
    public override void BuffStart()
    {
    }
    public override void BuffTrigger()
    {
        ModifyStatus(true);
    }
    /// <summary>
    /// –ﬁ∏ƒ Ù–‘
    /// </summary>
    /// <param name="value"></param>
    public void ModifyStatus(bool value)
    {
        switch (mBuff.BuffCfg.buffType)
        {
            case BuffType.AllowMove:
                mBuff.attachTarget.IsForceAllowMove = value;
                break;
            case BuffType.NotAllowDir:
                mBuff.attachTarget.IsForceNotAlllowModifyDir = value;
                break;
            default:
                break;
        }

    }
    public override void BuffEnd()
    {
        ModifyStatus(false);
    }
}