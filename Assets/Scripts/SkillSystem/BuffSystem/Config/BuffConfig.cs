using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff配置", menuName = "Buff配置", order = 0)]
[System.Serializable]
public class BuffConfig : ScriptableObject
{
    [LabelText("buff图标"),LabelWidth(0.1f),PreviewField(70,ObjectFieldAlignment.Left),SuffixLabel("Buff图标")]
    public Sprite buffIcon;//buff图标 
    [LabelText("buffID")]
    public int buffid;//Buff唯一id
    [LabelText("Buff名称")]
    public string buffName;
    [LabelText("延迟触发时间")]
    public int buffDelay;//如buff  1 2 3 秒之后触发buff对应的效果 ，如造成伤害 造成晕眩
    [LabelText("触发间隔")]
    public int buffIntervalms;//如Buff 每隔n秒之后回复一次生命值
    [LabelText("Buff持续时间(0表示一次 -1表示buff时间无限 直至战斗结束)")]
    public int buffDurationms;//0表示一次 -1表示buff时间无限 直至战斗结束  >0Buff真实生效时间
    [LabelText("Buff类型")]
    public BuffType buffType;//表示当前buff是什么buff 如晕眩 沉默等
    [LabelText("附加目标")]
    public BuffAttachType attachType;
    [LabelText("附加位置")]
    public BuffPosType buffPosType;
    [LabelText("Buff伤害类型")]
    public DamageType damageType;
    [LabelText("Buff伤害倍率")]
    public int damageRate;
    [LabelText("Buff 数值配置")]
    public List<BuffParams> buffParamsList;
    [LabelText("抓取数据")]
    public TargetGrabData targetGrabData;//抓取数据：如把怪物抓取某一个位置


    [LabelText("buff触发音效"),TitleGroup("技能表现","所有的表现数据会在Buff释放时和Buff触发时触发")]
    public AudioClip buffAudio;
    [LabelText("Buff触发特效"), TitleGroup("技能表现", "所有的表现数据会在Buff释放时和Buff触发时触发")]
    public BuffEffectConfig effectConfig;
    [LabelText("Buff命中特效"), TitleGroup("技能表现", "所有的表现数据会在Buff释放时和Buff触发时触发"),OnValueChanged("GetObjectPath")]
    public GameObject buffHitEffectObj;
    [ReadOnly]
    public string buffHitEffectObjPath;
    [LabelText("Buff触发动画"), TitleGroup("技能表现", "所有的表现数据会在Buff释放时和Buff触发时触发")]
    public ObjectAnimationState buffTriggerAnim= ObjectAnimationState.None;//buff触发的动画
    [LabelText("伤害/目标配置")]
    public TargetConfig targetConfig;//buff伤害检测/目标配置

    [Title("Buff描述："),HideLabel,MultiLineProperty(5)]
    public string buffDes;

#if UNITY_EDITOR
    public void GetObjectPath(GameObject obj)
    {
        buffHitEffectObjPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("skillHitEffectPath:" + buffHitEffectObjPath);
    }
    public void SaveAsset()
    {
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
    }
#endif
}
[System.Serializable,TabGroup("目标配置")]
public class TargetConfig
{
    [LabelText("是否启用")]
    public bool isOpen = false;
    [LabelText("作用目标"), ShowIf("isOpen")]
    public TargetType targetType;//作用目标
    [LabelText("伤害检测配置"),ShowIf("isOpen")]
    public SkillDamageConfig damageCfg;//伤害配置
}

 //Eidtor 1.备用
 //

/// <summary>
/// 表示当前buff触发时所需要播放的动画
/// </summary>
public enum ObjectAnimationState
{
    [LabelText("无配置")] None,
    [LabelText("受击")] BeHit,
    [LabelText("僵直")] Stiff,
}

[System.Serializable]
public class BuffEffectConfig
{
    [LabelText("特效对象"),OnValueChanged("GetObjectPath")]
    public GameObject effect;
    [ReadOnly]
    public string effectPath;
    [LabelText("特效附加类型")]
    public EffectAttachType attachType;
    [LabelText("特效位置类型")]
    public BuffEffectPosType effectPosType;

#if UNITY_EDITOR
    public void GetObjectPath(GameObject obj)
    {
        effectPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("effectPath:" + effectPath);
    }
#endif
}
[LabelText("特效位置类型")]
public enum BuffEffectPosType
{
    [LabelText("无配置")] None,
    [LabelText("跟随目标")] targetFollow,
    [LabelText("目标位置")] TargetPos,
}
[LabelText("特效附着类型")]
public enum EffectAttachType
{
    [LabelText("无配置")] None,
    [LabelText("中心")] Conter,
    [LabelText("手部")] Hand,
}
[System.Serializable]
public class TargetGrabData
{
    [LabelText("抓取到的目标位置")]
    public Vector3 garbMoveTargetPos;
    [LabelText("移动到抓取位置所需时间")]
    public int moveTimems;//表示要抓取的目标移动到抓取目标位置需要多久
}

[System.Serializable]
public class BuffParams
{
    [LabelText("参数"),PropertyTooltip("例如：造成的伤害量，击退的距离")]
    public float value;
    [LabelText("参数描述")]
    public string des;
}
[LabelText("位置类型")]
public enum BuffPosType
{
    [LabelText("无配置")] None,
    [LabelText("跟随目标")] FollowTarget,//跟随目标
    [LabelText("击中目标位置")] HitTargetPos,//如子弹命中位置
    [LabelText("施法者位置")] ReleaserPos,//UI摇杆输入位置
    [LabelText("输入位置")] UIInputPos,//UI摇杆输入位置
}
[LabelText("附加类型")]
public enum BuffAttachType
{
    [LabelText("无配置")] None,
    [LabelText("施法者")] Creator,//附加到施法者身上
    [LabelText("施法目标")] Target,//附加到施法目标身上
    [LabelText("施法者位置")] Creator_Pos,//附加到施法者所在位置
    [LabelText("施法者位置")] Target_Pos,//附加到施法目标所在位置
    [LabelText("引导位置")] Guide_Pos,//如在场景中通过技能摇杆选中的位置
}
[LabelText("buff类型")]
public enum BuffType 
{
    [LabelText("无配置")] None =0,
    [LabelText("击退")] Repel,
    [LabelText("浮空")] Floating,
    [LabelText("僵直")] Stiff,
    [LabelText("群体血量修改")] HP_Modify_Group,

    [LabelText("单体移速修改")] MoveSpeed_Modify_Single,

    [LabelText("抓取")] Grab,
    [LabelText("重力忽略")] IgnoreGravity,
    [LabelText("允许移动")] AllowMove,
    [LabelText("不允许转向")] NotAllowDir,
}