using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 移动类型
/// </summary>
public enum MoveType
{
    target,
    X,
    Y,
    Z,
}

public class MoveToAction : ActionBehaviour
{
    private LogicObject mActionObj;
    private FixIntVector3 mStartPos;
    private FixInt mMoveTime;
    private MoveType mMoveType;
    /// <summary>
    /// 移动的向量
    /// </summary>
    private FixIntVector3 mMoveDistance;
    /// <summary>
    /// 当前累计运行的时间
    /// </summary>
    private FixInt mAccRumTime;
    /// <summary>
    /// 当前移动的时间缩放
    /// </summary>
    private FixInt mTimeScale;
    public MoveToAction(LogicObject actionObj,FixIntVector3 startPos,FixIntVector3 targerPos,
        FixInt time,Action moveFinsihCallBack,Action updateCallBack,MoveType moveType)
    {
        //接收参数
        mActionObj = actionObj;
        mStartPos = startPos;
        mMoveTime = time==FixInt.Zero? 0.1f:time;
        mMoveType = moveType;
        mActionFinishCalllBack = moveFinsihCallBack;
        mUpdateActionCallBack = updateCallBack;
        //目标位置-起始位置=移动移动向量
        mMoveDistance = targerPos - startPos;
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

        if (mTimeScale>=1)
        {
            mTimeScale = 1;
            actionFinsih = true;
        }
        mUpdateActionCallBack?.Invoke();
        //计算对象需要移动的位置
        FixIntVector3 addDistance=FixIntVector3.zero;//添加的一个向量距离
        if (mMoveType== MoveType.target)
        {
            addDistance = mMoveDistance * mTimeScale;
            mActionObj.LogicPos = mStartPos + addDistance;
        }
        else if (mMoveType== MoveType.X)
        {
            addDistance.x = mMoveDistance.x * mTimeScale;
            mActionObj.LogicPos = new FixIntVector3(mStartPos.x + addDistance.x,mActionObj.LogicPos.y,mActionObj.LogicPos.z);
        }
        else if (mMoveType == MoveType.Y)
        {
            addDistance.y =  mMoveDistance.y * mTimeScale;
            mActionObj.LogicPos = new FixIntVector3(mActionObj.LogicPos.x, mStartPos.y + addDistance.y, mActionObj.LogicPos.z);
        }
        else if (mMoveType == MoveType.Z)
        {
            addDistance.z = mMoveDistance.z * mTimeScale;
            mActionObj.LogicPos = new FixIntVector3(mActionObj.LogicPos.x,mActionObj.LogicPos.y, mStartPos.z + addDistance.z);
        }
       
    }
     

}
