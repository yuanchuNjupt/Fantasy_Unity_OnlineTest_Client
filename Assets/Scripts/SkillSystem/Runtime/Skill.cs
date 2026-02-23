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
    public int skillId;

    //释放技能的角色
    public LogicActor mSkillCharacter;
    
    //技能数据
    private SkillDataConfig mSkillData;

    //外界访问技能数据接口
    public SkillConfig SKillCfg { get { return mSkillData.skillCfg; } }

    public List<SkillDamageConfig> damageCfgList { get { return mSkillData.damageCfgList; } }

    
    public Action<Skill> OnReleaseAfter;

    
    public Action<Skill, bool> OnReleaseSkillEnd;

    
    public SkillState skillState = SkillState.None;

    
    private int mCurLogicFrame = 0;
    
    private int mCurLogicFrameAccTime = 0;
    
    private int mCombinationSkillid;

    
    public Skill(int skillId, LogicActor skillCharacter)
    {
        this.skillId = skillId;
        this.mSkillCharacter = skillCharacter;
        
        //加载技能数据
        mSkillData = Resources.Load<ScriptableObject>(LoadPathConfig.SkillLoadPath + skillId) as SkillDataConfig;
        
        // 检查加载是否成功
        if (mSkillData == null)
        {
            Debug.LogError($"技能数据加载失败！技能ID: {skillId}");
        }
        // mSkillData = ZMAsset.LoadScriptableObject<SkillDataConfig>(AssetPathConfig.SKILL_DATA_PATH + skillId + ".asset");
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
        mSkillCharacter.PlayAnim(mSkillData.skillCfg.skillid.ToString());
    }

    public void SkillStart()
    {
        mCurLogicFrame = 0;
        mCurLogicFrameAccTime = 0;
        mCombinationSkillid = mSkillData.skillCfg.combinationSkillId;
         if (mSkillData.character.customLogicFame != 0)
            mSkillData.character.logicFrame = mSkillData.character.customLogicFame;
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
        OnReleaseSkillEnd?.Invoke(this, mSkillData.skillCfg.combinationSkillId != 0);
        ReleaseAllEffect();
        OnDamageRelease();
        if (mCombinationSkillid != 0)
        {
            mSkillCharacter.ReleaseSKill(mCombinationSkillid);
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
        if (mCurLogicFrame == mSkillData.skillCfg.skillShakeAfterFrame)
        {
            SkillAfter();
        }

        OnLogicFrameUpdateEffect();
        OnLogicFrameUpdateDamage();
        OnLogicFrameUpdateAction();
        OnLogicFrameUpdateAudio();
        
        
        
        if (mCurLogicFrame == mSkillData.character.MaxLogicFrame)
        {
            SKillEnd();
        }
        mCurLogicFrame++;
    }
}
