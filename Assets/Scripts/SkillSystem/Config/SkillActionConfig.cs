using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
/// <summary>
/// 行动类型
/// </summary>
public enum MoveActionType
{
    [LabelText("指定目标位置")]TargetPos,
    [LabelText("引导位置")] GuidePos,
    [LabelText("贝塞尔移动")] BezierPos,
}
/// <summary>
/// 行动完成后的操作
/// </summary>
public enum MoveActionFinishOpation
{ 
    None,
    Skill,
    Buff,
}

[System.Serializable]
public class SkillActionConfig
{
    //是否显示移动位置
    private bool mIsShowMovePos;
    //是否显示移动完成参数
    private bool mIsShowFinishParam;
    //是否显示贝塞尔数据
    private bool mIsShowBezierPos;

    [LabelText("触发帧")]
    public int triggerFrame;
    [LabelText("移动方式"),OnValueChanged("OnMoveActionTypeChange")]
    public MoveActionType moveActionType;
    [LabelText("最高点位置"),ShowIf("mIsShowBezierPos")]
    public Vector3 heightPos;
    [LabelText("移动位置"), ShowIf("mIsShowMovePos")]
    public Vector3 movePos;
    [LabelText("移动所需时间(毫米奥)")]
    public int durationMs;

    [LabelText("移动完成操作"), OnValueChanged("OnMoveActionFinishOpationChange")]
    public MoveActionFinishOpation actionFinishOpation;

    [LabelText("触发参数"), ShowIf("mIsShowFinishParam")]
    public List<int> actionFinishidList;

    public void OnMoveActionTypeChange(MoveActionType value)
    {
        mIsShowMovePos = value == MoveActionType.TargetPos || value == MoveActionType.BezierPos;
        mIsShowBezierPos = value == MoveActionType.BezierPos;
    }
    public void OnMoveActionFinishOpationChange(MoveActionFinishOpation value)
    {
        mIsShowFinishParam = value != MoveActionFinishOpation.None;
    }
}
