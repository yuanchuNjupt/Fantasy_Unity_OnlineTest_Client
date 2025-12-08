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
        if (LogicDir!=mInputMoveDir)
        {
            LogicDir = mInputMoveDir;
        }
        //计算逻辑轴向
        if (LogicDir.x!=FixInt.Zero&& IsForceNotAlllowModifyDir == false)
        {
            LogicXAxis = LogicDir.x > 0 ? 1 : -1;
        }
        Debug.Log("LogicPos:"+LogicPos + " LogicFrameid：" + LogicFrameConfig.LogicFrameid+ " playerid:"+ testPlayerid);
     }

    public void InputLoigcFrameEvent(FixIntVector3 inputDir,long playerid=0)
    {
        mInputMoveDir = inputDir;
        testPlayerid=playerid;
    }
}
