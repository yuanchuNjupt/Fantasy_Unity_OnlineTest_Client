using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;

public class MoveBezierAction : ActionBehaviour
{
    private LogicObject mActionObj;
    private FixedIntVector3 mStartPos;
    private FixedIntVector3 mHeightPos;
    private FixedIntVector3 mEndPos;
    private FixedInt mMoveTime;
    /// <summary>
    /// 当前累计运行的时间
    /// </summary>
    private FixedInt mAccRumTime;
    /// <summary>
    /// 当前移动的时间缩放
    /// </summary>
    private FixedInt mTimeScale;
    public MoveBezierAction(LogicObject actionObj, FixedIntVector3 startPos, FixedIntVector3 heightPos, FixedIntVector3 endPos, FixedInt time, Action moveFinsihCallBack, Action updateCallBack)
    {
        Debug.Log($"startPos:{startPos} heightPos:{heightPos} endPos:{endPos}");
        //接收参数
        mActionObj = actionObj;
        mStartPos = startPos;
        mHeightPos = heightPos;
        mEndPos = endPos;
        mMoveTime = time == 0 ? 0.1f : time;
        mActionFinishCallBack = moveFinsihCallBack;
        mUpdateActionCallBack = updateCallBack;
    }
    /// <summary>
    /// 行动完成
    /// </summary>
    public override void OnActionFinish()
    {
        if (actionFinish)
        {
            mActionFinishCallBack?.Invoke();
        }
    }
    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public override void OnLogicFrameUpdate()
    {
        //计算当前累计运行时间
        mAccRumTime += LogicFrameConfig.LogicFrameIntervalMs;
        //获取时间缩放比例
        mTimeScale = mAccRumTime / mMoveTime;

        if (mTimeScale >= 1)
        {
            mTimeScale = 1;
            actionFinish = true;
        }
        mUpdateActionCallBack?.Invoke();
        //计算对象需要移动的位置
        // mActionObj.LogicPos = BezierUtils.BezierCurve(mStartPos,mHeightPos,mEndPos, mTimeScale);

    }


}