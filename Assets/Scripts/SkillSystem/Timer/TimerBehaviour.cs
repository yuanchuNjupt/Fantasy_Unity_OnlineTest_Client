using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimerBehaviour
{
    /// <summary>
    /// 是否移动完成
    /// </summary>
    public bool TimerFinsih = false;
    /// <summary>
    /// 移动完成回调
    /// </summary>
    protected Action mTimerFinishCalllBack;
    /// <summary>
    /// 更新行动回调
    /// </summary>
    protected Action mUpdateTiemrCallBack;
    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public abstract void OnLogicFrameUpdate();
    /// <summary>
    /// 行动完成
    /// </summary>
    public abstract void OnTimerFinish();
}
