using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if  UNITY_EDITOR
public class SkillComplierWindow : OdinEditorWindow
{
    [TabGroup("Skill","Chararcter", SdfIconType.PersonFill, TextColor ="orange")]
    public SkillCharacterConfig character = new SkillCharacterConfig();
    [TabGroup("SKillComplier","Skill",SdfIconType.Robot,TextColor ="lightmagenta")]
    public SkillConfig skill = new SkillConfig();
    

    [TabGroup("SKillComplier", "Damage", SdfIconType.At, TextColor = "lightmagenta")]
    public List<SkillDamageConfig> damageList = new List<SkillDamageConfig>();

    [TabGroup("SKillComplier", "Effect", SdfIconType.OpticalAudio, TextColor = "blue")]
    public List<SkillEffectConfig> effectList = new List<SkillEffectConfig>();

    [TabGroup("SKillComplier", "Audio", SdfIconType.OpticalAudio, TextColor = "blue")]
    public List<SkillAudioConfig> audioList = new List<SkillAudioConfig>();

    [TabGroup("SKillComplier", "Action", SdfIconType.OpticalAudio, TextColor = "cyan")]
    public List<SkillActionConfig> actionList = new List<SkillActionConfig>();
#if UNITY_EDITOR
    //是否开始播放技能
    private bool isStartPlaySkill = false;
    [MenuItem("SKill/技能编译器")]
    public static SkillComplierWindow ShowWindow()
    {
       return GetWindowWithRect<SkillComplierWindow>(new Rect(0,0,1000,600));
    }
    public void SaveSKillData()
    {
        SkillDataConfig.SaveSkillData(character, skill, damageList, effectList, audioList, actionList);
        Close();
    }
    /// <summary>
    /// 加载技能数据
    /// </summary>
    /// <param name="skillData"></param>
    public void LoadSkillData(SkillDataConfig skillData)
    {
        this.character = skillData.character;
        this.skill = skillData.skillCfg;
        this.damageList = skillData.damageCfgList;
        this.effectList = skillData.effectCfgList;
        this.audioList = skillData.audioCfgList;
        this.actionList = skillData.actionCfgList;
        character.Init();
    }
    public static SkillComplierWindow GetWindow()
    {
        
        return GetWindow<SkillComplierWindow>();
    }
    /// <summary>
    /// 获取Editor模式下角色位置
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetCharacterPos()
    {
        if (!HasOpenInstances<SkillComplierWindow>())
        {
            return Vector3.zero;
        }
        SkillComplierWindow window = GetWindow<SkillComplierWindow>();
  
        if (window.character.skillCharacter!=null)
        {
            return window.character.skillCharacter.transform.position;
        }
        return Vector3.zero;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (var item in damageList)
        {
            item.OnInit();
        }
        EditorApplication.update += OnEditorUpdate;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (var item in damageList)
        {
            item.OnRelease();
        }
        EditorApplication.update -= OnEditorUpdate;
    }
    /// <summary>
    /// 开始播放技能
    /// </summary>
    public void StartPlaySkill()
    {
        foreach (var item in effectList)
        {
            item.StartPlaySkill();
        }
        foreach (var item in damageList)
        {
            item.PlaySkillStart();
        }
        mAccLogicRuntime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;
        isStartPlaySkill = true;
    }
    /// <summary>
    /// 技能暂停
    /// </summary>
    public void SkillPause()
    {
        foreach (var item in effectList)
        {
            item.PlaySkillEnd();
        }
        foreach (var item in damageList)
        {
            item.PlaySkillEnd();
        }
    }
    /// <summary>
    /// 播放技能结束
    /// </summary>
    public void PlaySkillEnd()
    {
        foreach (var item in effectList)
        {
            item.PlaySkillEnd();
        }
        foreach (var item in damageList)
        {
            item.PlaySkillEnd();
        }
        isStartPlaySkill = false;
        mAccLogicRuntime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;
    }

    public void OnEditorUpdate()
    {
        try
        {
            character.OnUpdate(()=> {
                //刷新当前窗口
                Focus();
            });
            if (isStartPlaySkill)
            {
                OnLogicUpdate();
            }
 
        }
        catch (System.Exception)
        {

        }
    }

    private float mAccLogicRuntime;//逻辑帧累计运行时间
    private float mNextLogicFrameTime;//下一个逻辑帧的时间
    private float mDeltaTime;//动画缓动时间 当前帧的增量时间
    private double mLastUpdateTime;//上次更新的时间
    /// <summary>
    /// 逻辑Update
    /// </summary>
    public void OnLogicUpdate()
    {
        //模拟帧同步更新 以0.066秒的间隔进行更新
        if (mLastUpdateTime==0)
        {
            mLastUpdateTime = EditorApplication.timeSinceStartup;
        }
        //计算逻辑帧累计运行时间
        mAccLogicRuntime =(float)(EditorApplication.timeSinceStartup - mLastUpdateTime);
        while (mAccLogicRuntime>mNextLogicFrameTime)
        {
            OnLogicFrameUpdate();
            //下一个逻辑帧的时间
            mNextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
        }
    }
    public void OnLogicFrameUpdate()
    {
        foreach (var item in effectList)
        {
            item.OnLogicFrameUpdate();
        }
        foreach (var item in damageList)
        {
            item.OnLogicFrameUpdate();
        }
    }
#endif
}
#endif