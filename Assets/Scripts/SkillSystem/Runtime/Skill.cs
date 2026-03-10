using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;

public enum SkillState
{
    None,
    Before,
    After,
    End,
}

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

    
    public Action<Skill> OnReleaseAfter;

    
    public Action<Skill, bool> OnReleaseSkillEnd;

    
    public SkillState skillState = SkillState.None;

    
    private int mCurLogicFrame = 0;
    
    private int mCurLogicFrameAccTime = 0;
    
    private int mCombinationSkillid;

    
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

    public void ReleaseSKill(Action<Skill> releaseAfterCallBack , Action<Skill, bool> releaseSkillEnd)
    {
        OnReleaseAfter = releaseAfterCallBack;
        OnReleaseSkillEnd = releaseSkillEnd;
        SkillStart();
        skillState = SkillState.Before;
        PlayAnim();
    }

    public void PlayAnim()
    {
        skillCharacter.PlayAnim(_skillData.skillCfg.skillid.ToString());
    }

    public void SkillStart()
    {
        mCurLogicFrame = 0;
        mCurLogicFrameAccTime = 0;
        mCombinationSkillid = _skillData.skillCfg.combinationSkillId;
         if (_skillData.character.customLogicFame != 0)
            _skillData.character.logicFrame = _skillData.character.customLogicFame;
        OnInitDamage();
    }

    public void SkillAfter()
    {
        skillState = SkillState.After;
        OnReleaseAfter?.Invoke(this);
    }

    public void SKillEnd()
    {
        skillState = SkillState.End;
        OnReleaseSkillEnd?.Invoke(this, _skillData.skillCfg.combinationSkillId != 0);
        ReleaseAllEffect();
        OnDamageRelease();
        if (mCombinationSkillid != 0)
        {
            skillCharacter.ReleaseSKill(mCombinationSkillid);
        }
    }


    public void OnLogicFrameUpdate()
    {
        if (skillState == SkillState.None||skillState== SkillState.End)
        {
            return;
        }
        mCurLogicFrameAccTime = mCurLogicFrame * LogicFrameConfig.LogicFrameIntervalMs;

        //达到后摇关键帧 
        if (mCurLogicFrame == _skillData.skillCfg.skillShakeAfterFrame)
        {
            SkillAfter();
        }

        OnLogicFrameUpdateEffect();
        OnLogicFrameUpdateDamage();
        OnLogicFrameUpdateAction();
        OnLogicFrameUpdateAudio();
        
        
        
        if (mCurLogicFrame == _skillData.character.MaxLogicFrame)
        {
            SKillEnd();
        }
        mCurLogicFrame++;
    }
}
