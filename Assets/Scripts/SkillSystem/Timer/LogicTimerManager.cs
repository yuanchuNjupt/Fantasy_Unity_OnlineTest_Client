using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using GGG.Tool.Singleton;
using UnityEngine;
public class LogicTimerManager : Singleton<LogicTimerManager>
{
    /// <summary>
    /// 计时器列表
    /// </summary>
    private List<LogicTiemr> mTimerList = new List<LogicTiemr>();

    /// <summary>
    /// 开始进行行动
    /// </summary>
    /// <param name="action"></param>
    public void DelayCall(FixInt delayTime,Action timerCallBack,int loopCount=1)
    {
        LogicTiemr timer = new LogicTiemr(delayTime,timerCallBack,loopCount);
        mTimerList.Add(timer);
    }
    /// <summary>
    /// 逻辑帧更新帧
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        //移除已经完成的行动
        for (int i = mTimerList.Count-1; i >=0 ; i--)
        {
            LogicTiemr timer = mTimerList[i];
            if (timer.TimerFinsih)
            {
                RemoveTimer(timer);
            }
        }
        //更新逻辑帧
        foreach (var item in mTimerList)
        {
            item.OnLogicFrameUpdate();
        }
    }
    /// <summary>
    /// 移除对应的行动
    /// </summary>
    /// <param name="action"></param>
    public void RemoveTimer(LogicTiemr timer)
    {
        mTimerList.Remove(timer);
    }
    /// <summary>
    /// 脚本资源释放
    /// </summary>
    public void OnDestroy()
    {
        mTimerList.Clear();
    }
}
