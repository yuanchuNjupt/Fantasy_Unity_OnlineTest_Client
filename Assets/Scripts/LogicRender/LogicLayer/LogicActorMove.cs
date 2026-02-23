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
    private FixedIntVector3 _inputMoveDir;
    /// <summary>
    /// 逻辑帧位置更新
    /// </summary>
    public void OnLogicFrameUpdateMove()
    {
        Collider?.UpdatePosition(LogicPos);
        
        if (ActionSate != LogicObjectActionState.Idle && ActionSate != LogicObjectActionState.Move && IsForceAllowMove==false)
        {
            return;
        }
        //计算逻辑位置
        LogicPos += _inputMoveDir* LogicMoveSpeed * (FixedInt)LogicFrameConfig.LogicFrameInterval;

        //计算逻辑对象的朝向
        if (LogicDir!=_inputMoveDir && _inputMoveDir != FixedIntVector3.zero)
        {
            LogicDir = _inputMoveDir;
        }
    }

    public void InputLogicFrameEvent(FixedIntVector3 inputDir)
    {
        _inputMoveDir = inputDir;
    }
}
