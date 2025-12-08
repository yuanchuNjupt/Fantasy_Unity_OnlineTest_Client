using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicTiemr : TimerBehaviour
{

    private FixInt mDelayTime;
    private int mLoopCount;

    private FixInt mCurLogicFrameAccTime;
    /// <summary>
    /// 总运行时间
    /// </summary>
    private FixInt mTotalTime;
    public LogicTiemr(FixInt delayTime,Action timerCallBack,int loopCount=1) //1,2
    {
        this.mDelayTime = delayTime;
        this.mLoopCount = loopCount;
        this.mTotalTime = loopCount * delayTime;
        this.mTimerFinishCalllBack = timerCallBack;
    }
    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public override void OnLogicFrameUpdate()
    {
        //毫秒时间累加
        mCurLogicFrameAccTime += LogicFrameConfig.LogicFrameInterval;
        if (mCurLogicFrameAccTime>=mDelayTime)
        {
            mTimerFinishCalllBack?.Invoke();
            mCurLogicFrameAccTime -= mDelayTime;
            mTotalTime -= mDelayTime;
            //如果循环次数<=1 说明当前计时器工作完成
            if (mLoopCount<=1|| mTotalTime<=0)
            {
                TimerFinsih = true;
                mTimerFinishCalllBack = null;
            }
        }

    }

    public override void OnTimerFinish()
    {
         
    }
}
