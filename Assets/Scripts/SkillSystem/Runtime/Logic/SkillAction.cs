using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using FixedPhysics.Fixed_pointNumber.Core;
using Framework.AdvancedLog;
using UnityEngine;

public partial class Skill 
{
    
    /// <summary>
    /// 行动逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdateAction()
    {
        if (_skillData.actionCfgList == null || _skillData.actionCfgList.Count == 0) return;

        foreach (var item in _skillData.actionCfgList)
        {
            switch (item.moveActionType)
            {
                // ── TargetPos：在触发帧启动插值 Action ──────────────────────────
                case MoveActionType.TargetPos:
                    if (item.triggerFrame == _curLogicFrame)
                        AddMoveAction(item, skillCharacter);
                    break;

                // ── DeltaPos：遍历每条增量数据，命中当前帧就直接叠加位移 ──────
                case MoveActionType.DeltaPos:
                    if (item.deltaMoveData == null) break;
                    foreach (var deltaData in item.deltaMoveData)
                    {
                        if (deltaData.triggerFrame != _curLogicFrame) continue;

                        // 将局部增量（以角色朝向为 +Z 轴）转换到世界坐标
                        // Forward = LogicForwardDir，Right = LogicRightDir，Up = LogicUpDir
                        FixedIntVector3 worldDelta =
                            skillCharacter.LogicRightDir   * (FixedInt)deltaData.deltaPos.x +
                            skillCharacter.LogicUpDir      * (FixedInt)deltaData.deltaPos.y +
                            skillCharacter.LogicForwardDir * (FixedInt)deltaData.deltaPos.z;

                        skillCharacter.LogicPos += worldDelta;
                        
                        Log.Info(LogColor.Purple , "技能系统" , $"移动增量触发帧:{deltaData.triggerFrame}");
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 添加移动行动（TargetPos 插值模式）
    /// </summary>
    public void AddMoveAction(SkillActionConfig item, LogicObject logicMoveObj)
    {
        // 目标点 = 当前位置 + 配置偏移（以角色朝向为 Z 轴转换到世界坐标）



        //     FixedIntVector3 offset = (FixedIntVector3)item.moveData;
        //     FixedIntVector3 worldOffset =
        //         logicMoveObj.LogicRightDir   * offset.X +
        //         logicMoveObj.LogicUpDir      * offset.Y +
        //         logicMoveObj.LogicForwardDir * offset.Z;
        //
        //     FixedIntVector3 startPos  = logicMoveObj.LogicPos;
        //     FixedIntVector3 targetPos = startPos + worldOffset;
        //
        //     MoveToAction action = new MoveToAction(
        //         logicMoveObj,
        //         startPos,
        //         targetPos,
        //         item.durationFrame * LogicFrameConfig.LogicFrameIntervalMs,
        //         OnActionFinish,
        //         moveUpdateCallBack,
        //         MoveType.target);
        //
        //     LogicActionController.Instance.RunAciton(action);
        // 
    }
}


