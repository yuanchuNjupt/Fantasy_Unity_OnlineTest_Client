using FixMath;
using System.Collections;
using System.Collections.Generic;
using FixedPhysics.Fixed_pointNumber.Core;
using Framework.AdvancedLog;
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
        
        if (ActionState != LogicObjectActionState.Idle && ActionState != LogicObjectActionState.Move && IsForceAllowMove==false)
        {
            return;
        }
        //计算逻辑位置
        LogicPos += _inputMoveDir* LogicMoveSpeed * (FixedInt)LogicFrameConfig.LogicFrameInterval;

        //计算逻辑对象的朝向
        if (LogicForwardDir!=_inputMoveDir && _inputMoveDir != FixedIntVector3.zero)
        {
            LogicForwardDir = _inputMoveDir;
        }
    }

    public void UpdateMoveDir(FixedIntVector3 inputDir)
    {
        
        if (ActionState is LogicObjectActionState.ReleasingSkillBefore)
        {
            // 前摇阶段：锁定移动，忽略输入
            return;
        }

        _inputMoveDir = inputDir;
        
        
        if (inputDir != FixedIntVector3.zero)
        {
            //有输入
            if (ActionState is LogicObjectActionState.ReleasingSkillAfter)
            {
                // 后摇阶段有移动输入：结束后摇，状态由 OnSkillReleaseEnd 置为 Idle
                // 本帧直接 return，下一个采样帧再走正常 Idle→Move 流程
                // 避免同帧内 Idle→Move 触发 SwitchState 导致动画被反复打断
                currentSkill?.SKillEnd();
                return;
            }
            if (ActionState is LogicObjectActionState.Idle)
                ActionState = LogicObjectActionState.Move;
        }
        else
        {
            //无输入
            if (ActionState is LogicObjectActionState.Move)
                ActionState = LogicObjectActionState.Idle;
        }
    }
}
