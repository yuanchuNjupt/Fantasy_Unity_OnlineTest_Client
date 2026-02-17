using FixMath;
using System.Collections;
using System.Collections.Generic;
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;

/// <summary>
/// 处理演员对象移动逻辑脚本
/// </summary>
public partial class LogicActor
{
    private FixedIntVector3 mInputMoveDir;
    private long testPlayerid;
    /// <summary>
    /// 逻辑帧位置更新
    /// </summary>
    public void OnLogicFrameUpdateMove()
    {
        // Collider?.UpdateColliderInfo(LogicPos, Collider.Size);
        Collider?.UpdatePosition(LogicPos);
        
        if (ActionSate != LogicObjectActionState.Idle && ActionSate != LogicObjectActionState.Move && IsForceAllowMove==false)
        {
            return;
        }
        //计算逻辑位置
        LogicPos += mInputMoveDir* LogicMoveSpeed * (FixedInt)LogicFrameConfig.LogicFrameInterval;

        //计算逻辑对象的朝向
        if (LogicDir!=mInputMoveDir && mInputMoveDir != FixedIntVector3.zero)
        {
            LogicDir = mInputMoveDir;
        }
    }

    public void InputLogicFrameEvent(FixedIntVector3 inputDir,long playerid=0)
    {
        mInputMoveDir = inputDir;
        testPlayerid=playerid;
    }
}
