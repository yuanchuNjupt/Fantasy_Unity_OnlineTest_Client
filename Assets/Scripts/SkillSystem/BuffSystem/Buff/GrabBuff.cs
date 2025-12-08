using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBuff : BuffComposite
{
    public GrabBuff(Buff buff) : base(buff) { }

    public override void BuffDelay()
    {

    }


    public override void BuffStart()
    {

    }

    public override void BuffTrigger()
    {
        //根据抓取数据，让当前Buff附加的目标飞向指定的位置

        //1.计算飞向的目标点
        LogicObject attachTarget = mBuff.attachTarget;
        LogicObject releaser = mBuff.releaser;
        //2.抓取怪物至角色所在位置
        attachTarget.LogicPos = releaser.LogicPos;
        //3.把怪物抓取到指定的目标点
        FixIntVector3 grabPos = new FixIntVector3(mBuff.BuffCfg.targetGrabData.garbMoveTargetPos) * releaser.LogicXAxis;
        grabPos.y = FixIntMath.Abs(grabPos.y);
        //抓取目标位置
        FixIntVector3 targetGrabPos = releaser.LogicPos + grabPos;
        //构建行动
        // MoveToAction moveToAction = new MoveToAction(attachTarget,attachTarget.LogicPos,targetGrabPos,
        //     mBuff.BuffCfg.targetGrabData.moveTimems,null,null,MoveType.target);
        // //执行行动
        // LogicActionController.Instance.RunAciton(moveToAction);
        Debug.Log("targetGrabPos:"+ targetGrabPos);
    }

    public override void BuffEnd()
    {
        Debug.Log("Grab End");
    }
}
