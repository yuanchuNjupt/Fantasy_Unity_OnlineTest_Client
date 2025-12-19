using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理演员对象移动逻辑脚本
/// </summary>
public partial class LogicActor
{
    private FixIntVector3 mInputMoveDir;
    private long testPlayerid;
    /// <summary>
    /// 逻辑帧位置更新
    /// </summary>
    public void OnLogicFrameUpdateMove()
    {
        Collider?.UpdateColliderInfo(LogicPos, Collider.Size);
        if (ActionSate != LogicObjectActionState.Idle && ActionSate != LogicObjectActionState.Move && IsForceAllowMove==false)
        {
            return;
        }
        //计算逻辑位置
        LogicPos += mInputMoveDir* LogicMoveSpeed * (FixInt)LogicFrameConfig.LogicFrameInterval;

        //计算逻辑对象的朝向
        if (LogicDir!=mInputMoveDir && mInputMoveDir != FixIntVector3.zero)
        {
            LogicDir = mInputMoveDir;
        }
    }

    public void InputLogicFrameEvent(FixIntVector3 inputDir,long playerid=0)
    {
        mInputMoveDir = inputDir;
        testPlayerid=playerid;
    }
}
