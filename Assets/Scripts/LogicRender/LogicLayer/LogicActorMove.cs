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
        //判断是否能修改移动方向
        if (ActionState is LogicObjectActionState.ReleasingSkillBefore)
        {
            // 前摇阶段：锁定移动，忽略输入
            return;
        }
        else if (ActionState is LogicObjectActionState.ReleasingSkillAfter)
        {
            // 后摇阶段：允许移动输入，并强制结束当前技能后摇
            // SKillEnd 内部会将 ActionState 置为 Idle，本帧不再做状态同步
            // 直接记录方向后返回，下一帧走正常移动流程
            _inputMoveDir = inputDir;
            currentSkill?.SKillEnd();
            return;
        }

        _inputMoveDir = inputDir;

        // 根据输入方向同步动作状态
        if (inputDir != FixedIntVector3.zero)
        {
            if (ActionState is LogicObjectActionState.Idle)
                ActionState = LogicObjectActionState.Move;
        }
        else
        {
            if (ActionState is LogicObjectActionState.Move)
                ActionState = LogicObjectActionState.Idle;
        }
    }
}
