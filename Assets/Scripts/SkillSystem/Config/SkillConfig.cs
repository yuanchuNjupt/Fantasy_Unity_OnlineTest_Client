using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
[System.Serializable]
public class SkillConfig
{
    [LabelText("技能图标"),LabelWidth(0.1f),PreviewField(70,ObjectFieldAlignment.Left),SuffixLabel("技能图标")]
    public Sprite skillIcon;

    [LabelText("技能id")]
    public int skillid;
    
    [LabelText("技能名称")]
    public string skillName;//技能名称

    [LabelText("技能后摇关键帧"),Tooltip("技能释放完成后，角色不能进行其他操作的持续帧数，单位：帧")]
    public int skillShakeAfterFrame;//技能后摇的关键帧
    
    [LabelText("技能冷却时间")]
    public int skillCdTime;//技能冷却时间

    [LabelText("组合技能id(衔接下一个技能的id)"), Tooltip("比如：技能A 由技能 C B D组成")]
    public int combinationSkillId;
    
    //技能渲染相关
    [LabelText("技能命中特效"),TitleGroup("技能渲染","所有英雄渲染数据会在开始释放技能时触发"),OnValueChanged("GetObjectPath")]
    public GameObject skillHitEffect;//技能命中特效
    
    [ReadOnly]
    public string skillHitEffectPath;
    
    [LabelText("技能击中特效存活时间"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public int hitEffectSurvivalTimeMs = 100;//技能击中特效存活时间
    
    [LabelText("技能命中音效"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public AudioClip skillHitAudio;//技能命中音效
    
    [LabelText("技能描述"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public string skillDes;//技能描述
#if UNITY_EDITOR

    public void GetObjectPath(GameObject obj)
    {
        skillHitEffectPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("skillHitEffectPath:" + skillHitEffectPath);
    }
#endif
}

public enum SKillType
{
    [LabelText("无配置（瞬发技能）")] None, 
    [LabelText("吟唱型技能")] Chant,//吟唱型技能
    [LabelText("弹道型技能")] Ballistic,//弹道型技能
    [LabelText("蓄力技能")] StockPile,//蓄力技能 
    [LabelText("位置引导技能")] PosGuide,//位置引导技能
}

/// <summary>
/// 蓄力阶段数据
/// </summary>
[System.Serializable]
public class StockPileStageData
{
    [LabelText("蓄力阶段id")]
    public int stage;//蓄力阶段id
    [LabelText("当前蓄力阶段触发的技能id")]
    public int skillid;//当前蓄力阶段触发的技能id
    [LabelText("当前阶段触发开始时间")]
    public int startTimeMs;//当前阶段触发开始时间
    [LabelText("当前阶段结束时间")]
    public int endTimeMs;//当前阶段结束时间
}