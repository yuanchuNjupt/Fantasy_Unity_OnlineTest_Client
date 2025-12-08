using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffComposite 
{

    protected Buff mBuff;

    public BuffComposite(Buff buff)
    {
        this.mBuff = buff;
    }

    /// <summary>
    /// buff延迟触发接口
    /// </summary>
    public abstract void BuffDelay();
    /// <summary>
    /// Buff开始流程
    /// </summary>
    public abstract void BuffStart();
    /// <summary>
    /// buff逻辑触发，可以执行晕眩逻辑或属性修改逻辑
    /// </summary>
    public abstract void BuffTrigger();
    /// <summary>
    /// buff执行完成
    /// </summary>
    public abstract void BuffEnd();
}
