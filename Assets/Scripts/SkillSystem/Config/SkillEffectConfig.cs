using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillEffectConfig 
{
    [AssetList][LabelText("技能特效对象")][PreviewField(70, ObjectFieldAlignment.Left),OnValueChanged("GetObjectPath")]
    public GameObject skillEffect;//技能特效
    [ReadOnly]
    public string skillEffectPath;
    [LabelText("触发帧")]
    public int triggerFrame;//触发帧
    [LabelText("结束帧")]
    public int endFrame;//结束帧
    [LabelText("特效偏移位置")]
    public Vector3 effectOffsetPos;//特效偏移位置
    [LabelText("特效位置类型")]
    public EffectPosType effectPosType;//特效位置类型
    [ToggleGroup("isSetTransParent","是否设置特效父节点")]
    public bool isSetTransParent = false;//是否设置特效父节点
    [ToggleGroup("isSetTransParent", "节点类型")]
    public TransParentType transParent;//父节点类型
    [ToggleGroup("isAttachDamage", "是否附加伤害")]
    public bool isAttachDamage=false;
    [ToggleGroup("isAttachDamage", "是否附加伤害")]
    public SkillDamageConfig damageConfig;

    [ToggleGroup("isAttachAction", "是否附加行动")]
    public bool isAttachAction=false;
    [ToggleGroup("isAttachAction", "是否附加行动")]
    public SkillActionConfig actionConfig;

    [HideInInspector]
    public GameObject GameEffectObj;//游戏特效缓存对象
#if UNITY_EDITOR

    public void GetObjectPath(GameObject obj)
    {
        skillEffectPath= UnityEditor.AssetDatabase.GetAssetPath(obj);
        Debug.Log("skillEffectPath:"+ skillEffectPath);
    }

    //Editor模式下克隆的特效对象
    private GameObject mCloneEffect;
    private AnimationAgnet mAnimAgent;
    private ParticlesAgent mParticleAgent;
    //当前逻辑帧
    private int mCurLogicFrame = 0;
    /// <summary>
    /// 开始播放技能
    /// </summary>
    public void StartPlaySkill()
    {
        DestroyEffect(); 
        mCurLogicFrame = 0;
    }
    public void SkillPause()
    {
        DestroyEffect();
    }
    /// <summary>
    /// 播放技能结束
    /// </summary>
    public void PlaySkillEnd()
    {
        DestroyEffect();
    }
    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        if (mCurLogicFrame==triggerFrame)
        {
            CreateEffect();
        }
        else if (mCurLogicFrame == endFrame)
        {
            DestroyEffect();
        }
        mCurLogicFrame++;
    }
    /// <summary>
    /// 创建特效
    /// </summary>
    public void CreateEffect()
    {
        if (skillEffect!=null)
        {
            mCloneEffect= GameObject.Instantiate(skillEffect);
            mCloneEffect.transform.position = SkillComplierWindow.GetCharaterPos();
            mAnimAgent = new AnimationAgnet();
            mAnimAgent.InitPlayAnim(mCloneEffect.transform);

            mParticleAgent = new ParticlesAgent();
            mParticleAgent.InitPlayAnim(mCloneEffect.transform);
        }
    }
    /// <summary>
    /// 销毁特效
    /// </summary>
    public void DestroyEffect()
    {
        if (mCloneEffect!=null)
        {
            GameObject.DestroyImmediate(mCloneEffect);
        }
        if (mAnimAgent!=null)
        {
            mAnimAgent.OnDestroy();
            mAnimAgent = null;
        }
        if (mParticleAgent != null)
        {
            mParticleAgent.OnDestroy();
            mParticleAgent = null;
        }
    }


#endif

}

public enum TransParentType
{
    [LabelText("无配置")] None,
    [LabelText("左手")] LeftHand,//左手
    [LabelText("右手")] RightHand,//右手
}

public enum EffectPosType
{
   [LabelText("跟随角色位置和方向")] FollowPosDir,//跟随角色位置和方向
    [LabelText("跟随角色方向")] FollowDir,//只跟随角色方向
    [LabelText("屏幕中心位置")] ConterPos,//屏幕中心位置
    [LabelText("引导位置")] GuidePos,//引导位置
    [LabelText("跟随特效移动位置")] FollwEffectMovePos,//跟随特效移动位置
    [LabelText("位置归零")] Zero,//位置归零
}