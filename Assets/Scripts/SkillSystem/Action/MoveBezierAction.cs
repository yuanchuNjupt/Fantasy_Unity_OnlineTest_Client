using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBezierAction : ActionBehaviour
{
    private LogicObject mActionObj;
    private FixIntVector3 mStartPos;
    private FixIntVector3 mHeightPos;
    private FixIntVector3 mEndPos;
    private FixInt mMoveTime;
    /// <summary>
    /// 当前累计运行的时间
    /// </summary>
    private FixInt mAccRumTime;
    /// <summary>
    /// 当前移动的时间缩放
    /// </summary>
    private FixInt mTimeScale;
    public MoveBezierAction(LogicObject actionObj, FixIntVector3 startPos, FixIntVector3 heightPos, FixIntVector3 endPos, FixInt time, Action moveFinsihCallBack, Action updateCallBack)
    {
        Debug.Log($"startPos:{startPos} heightPos:{heightPos} endPos:{endPos}");
        //接收参数
        mActionObj = actionObj;
        mStartPos = startPos;
        mHeightPos = heightPos;
        mEndPos = endPos;
        mMoveTime = time == FixInt.Zero ? 0.1f : time;
        mActionFinishCalllBack = moveFinsihCallBack;
        mUpdateActionCallBack = updateCallBack;
    }
    /// <summary>
    /// 行动完成
    /// </summary>
    public override void OnActionFinish()
    {
        if (actionFinsih)
        {
            mActionFinishCalllBack?.Invoke();
        }
    }
    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public override void OnLogicFrameUpdate()
    {
        //计算当前累计运行时间
        mAccRumTime += LogicFrameConfig.LogicFrameIntervalms;
        //获取时间缩放比例
        mTimeScale = mAccRumTime / mMoveTime;

        if (mTimeScale >= 1)
        {
            mTimeScale = 1;
            actionFinsih = true;
        }
        mUpdateActionCallBack?.Invoke();
        //计算对象需要移动的位置
        // mActionObj.LogicPos = BezierUtils.BezierCurve(mStartPos,mHeightPos,mEndPos, mTimeScale);

    }


}