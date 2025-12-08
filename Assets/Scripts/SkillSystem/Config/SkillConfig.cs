using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
[System.Serializable]
public class SkillConfig
{
    [HideInInspector]
    public bool showStockPileData = false;//是否显示蓄力技能数据
    [HideInInspector]
    public bool showSkillGuide = false;//是否显示技能引导数据


    [LabelText("技能图标"),LabelWidth(0.1f),PreviewField(70,ObjectFieldAlignment.Left),SuffixLabel("技能图标")]
    public Sprite skillIcon;

    [LabelText("技能id")]
    public int skillid;
    [LabelText("技能名称")]
    public string skillName;//技能名称
    [LabelText("技能所需蓝量")]
    public int needMgicValue = 100;//技能所需蓝量
    [LabelText("技能前摇时间")]
    public int skillShakeBeforeTimeMs;//技能前摇时间
    [LabelText("技能攻击持续时间")]
    public int skillAttackDurationMs;//技能攻击持续时间
    [LabelText("技能后摇时间")]
    public int skillShakeArfterMs;//技能后摇时间
    [LabelText("技能冷却时间")]
    public int skillCDTimeMs;//技能冷却时间
    [LabelText("技能类型"),OnValueChanged("OnSKillTypeChange")]
    public SKillType skillType;//技能类型

    [LabelText("蓄力阶段配置(若第一阶段触发时间不为0，则空档时间为动画表现时间)"),ShowIf("showStockPileData")]
    public List<StockPileStageData> stockPileStageData;//技能蓄力数据配置
    [LabelText("技能引导特效"),ShowIf("showSkillGuide")]
    public GameObject skillGuideObj;//技能引导特效
    [LabelText("技能引导范围"), ShowIf("showSkillGuide")]
    public float skillGuideRange;//技能引导范围

    [LabelText("组合技能id(衔接下一个技能的id)"), Tooltip("比如：技能A 由技能 C B D组成")]
    public int ComobinationSkillid;
    //技能渲染相关
    [LabelText("技能命中特效"),TitleGroup("技能渲染","所有英雄渲染数据会在开始释放技能时触发"),OnValueChanged("GetObjectPath")]
    public GameObject skillHitEffect;//技能命中特效
    [ReadOnly]
    public string skillHitEffectPath;
    [LabelText("技能击中特效存活时间"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public int hitEffectSurvivalTimeMs = 100;//技能击中特效存活时间
    [LabelText("技能命中音效"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public AudioClip skillHitAudio;//技能命中音效
    [LabelText("是否显示技能立绘"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public bool showSkillPortrait;//是否显示技能立绘
    [LabelText("技能立绘对象"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发"),ShowIf("showSkillPortrait")]
    public GameObject skillProtraitObj;//技能立绘对象
    [LabelText("技能描述"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public string skillDes;//技能描述
#if UNITY_EDITOR

    public void GetObjectPath(GameObject obj)
    {
        skillHitEffectPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("skillHitEffectPath:" + skillHitEffectPath);
    }
#endif
    /// <summary>
    /// 技能类型改变
    /// </summary>
    public void OnSKillTypeChange(SKillType sKillType)
    {
        showStockPileData = sKillType == SKillType.StockPile;
        showSkillGuide = sKillType == SKillType.PosGuide;
    }
}

public enum SKillType
{
    [LabelText("无配置（瞬发技能）")] None, 
    [LabelText("吟唱型技能")] Chnat,//吟唱型技能
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