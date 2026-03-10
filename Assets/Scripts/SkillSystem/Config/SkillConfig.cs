using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
[System.Serializable]
public class SkillConfig
{
    [LabelText("技能图标"), LabelWidth(0.1f), PreviewField(70, ObjectFieldAlignment.Left), SuffixLabel("技能图标")]
    public Sprite skillIcon;

    [LabelText("技能id")] public int skillid;

    [LabelText("技能名称")] public string skillName;

    [LabelText("技能后摇关键帧"), Tooltip("技能释放完成后，角色不能进行其他操作的持续帧数，单位：帧")]
    public int skillShakeAfterFrame;

    [LabelText("技能冷却时间")] public int skillCdTime;

    [LabelText("技能描述"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public string skillDes;
    
}
