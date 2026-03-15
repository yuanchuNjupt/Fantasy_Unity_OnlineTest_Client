using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;

// public enum SkillState
// {
//     None,
//     Before,
//     After,
//     End,
// }

public partial class Skill
{
    //技能ID
    public readonly int skillId;

    //释放技能的角色
    public readonly LogicActor skillCharacter;
    
    //技能数据
    private readonly SkillDataConfig _skillData;

    //外界访问技能数据接口
    public SkillConfig SKillCfg { get { return _skillData.skillCfg; } }

    public List<SkillDamageConfig> damageCfgList { get { return _skillData.damageCfgList; } }

    
    public Action onReleaseAfter;

    
    public Action onReleaseSkillEnd;
    
    public Action<bool> onForceEnd;

    

    
    private int _curLogicFrame = 0;

    /// <summary>
    /// 当前技能是否正在释放中，SKillEnd 后置 false 阻止后续帧继续执行
    /// </summary>
    public bool IsReleasing { get; private set; } = false;
    

    
    public Skill(int skillId, LogicActor skillCharacter)
    {
        this.skillId = skillId;
        this.skillCharacter = skillCharacter;
        
        //加载技能数据
        _skillData = Resources.Load<ScriptableObject>(LoadPathConfig.SkillLoadPath + skillId) as SkillDataConfig;
        
        // 检查加载是否成功
        if (_skillData == null)
        {
            Debug.LogError($"技能数据加载失败！技能ID: {skillId}");
        }
        // _skillData = ZMAsset.LoadScriptableObject<SkillDataConfig>(AssetPathConfig.SKILL_DATA_PATH + skillId + ".asset");
    }

    public void ReleaseSKill(Action releaseAfterCallBack , Action releaseSkillEnd , Action<bool> forceEnd = null)
    {
        onReleaseAfter = releaseAfterCallBack;
        onReleaseSkillEnd = releaseSkillEnd;
        onForceEnd = forceEnd;
        IsReleasing = true;
        SkillStart();
        PlayAnim();
    }

    public void PlayAnim()
    {
        skillCharacter.PlayAnim(_skillData.skillCfg.skillid.ToString());
    }

    public void SkillStart()
    {
        _curLogicFrame = 0;
         if (_skillData.character.customLogicFame != 0)
            _skillData.character.logicFrame = _skillData.character.customLogicFame;
        OnInitDamage();
    }

    public void SkillAfter()
    {
        onReleaseAfter?.Invoke();
    }

    public void SKillEnd()
    {
        if (!IsReleasing) return;
        IsReleasing = false;
        onReleaseSkillEnd?.Invoke();
        ReleaseAllEffect();
        OnDamageRelease();
    }

    public void SkillForceEnd(bool isResetNormalAttackIndex = true)
    {
        if (!IsReleasing) return;
        IsReleasing = false;
        onForceEnd?.Invoke(isResetNormalAttackIndex);
        ReleaseAllEffect();
        OnDamageRelease();
        
        
    }


    public void OnLogicFrameUpdate()
    {
        // 技能已结束（被外部提前中止或自然结束），不再执行任何逻辑
        if (!IsReleasing) return;

        //达到后摇关键帧 
        if (_curLogicFrame == _skillData.skillCfg.skillShakeAfterFrame)
        {
            SkillAfter();
        }
        
        //先更新移动
        OnLogicFrameUpdateAction();
        
        OnLogicFrameUpdateEffect();
        OnLogicFrameUpdateDamage();
        
        OnLogicFrameUpdateAudio();
        
        
        
        if (_curLogicFrame == _skillData.character.MaxLogicFrame)
        {
            SKillEnd();
        }
        _curLogicFrame++;
    }
}
