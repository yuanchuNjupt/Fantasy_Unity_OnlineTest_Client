using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;
public partial class LogicActor
{
    /// <summary>
    /// 技能系统
    /// </summary>
    private SkillSystem mSkillSystem;
    /// <summary>
    /// 普通攻击技能id数组
    /// </summary>
    private int[] mNormalSkillidArr = new int[] { 1001, 1002, 1003 };

    private int[] mSkillidArr;

    /// <summary>
    /// 正在释放技能的列表
    /// </summary>
    public List<Skill> releaseingSkillList = new List<Skill>();
    /// <summary>
    /// 当前普通攻击连击索引
    /// </summary>
    private int mCurNormalComboIndex = 0;
    /// <summary>
    /// 当前对象持有的所有buff
    /// </summary>
    private List<Buff> mBuffList = new List<Buff>();
    /// <summary>
    /// 战斗逻辑层
    /// </summary>
    // private BattleLogicCtrl mBattleLogicLayer;
    /// <summary>
    /// 释放技能回调
    /// </summary>
    public Action<bool, int> OnReleaseSkillCallBack;
    /// <summary>
    /// 初始化技能列表
    /// </summary>
    public void InitActorSkill(int id)
    {
        // mBattleLogicLayer = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>();
        // HeroDataMgr heroData = BattleWorld.GetExitsDataMgr<HeroDataMgr>();
        // mNormalSkillidArr = heroData.GetHeroNormalSkilidArr(id);
        // mSkillidArr = heroData.GetHeroSkillIdArr(id);
        mSkillSystem = new SkillSystem(this);
        mSkillSystem.InitSKills(mNormalSkillidArr);
        mSkillSystem.InitSKills(mSkillidArr);
    }
    /// <summary>
    /// 释放普通攻击
    /// </summary>
    public void ReleaseNormalAttack()
    {
        if (LogicFrameConfig.IsUseLocalLogicFrame)
        {
            ReleaseSKill(mNormalSkillidArr[mCurNormalComboIndex]);
        }
        else
        {
            //输入技能释放操作
            // mBattleLogicLayer.ReleaseSkillInput(mNormalSkillidArr[mCurNormalComboIndex]);
        }
    }
    /// <summary>
    /// 释放对应的技能
    /// </summary>
    /// <param name="skillid"></param>
    public void ReleaseSKill(int skillid,FixIntVector3 guidePos=default(FixIntVector3), Action<bool> releaseSkillCallBack = null)
    {
        Skill skill = mSkillSystem.ReleaseSkill(skillid, guidePos,   OnSkillReleaseAfter, (skill)=> {
            if (skill.SKillCfg.skillType== SKillType.StockPile)
            {
                releaseSkillCallBack?.Invoke(true);
                OnReleaseSkillCallBack?.Invoke(true, skill.skillid);
            }
            OnSkillReleaseEnd(skill);
        });
        //！=null 说明技能释放成功
        if (skill != null)
        {
            releaseingSkillList.Add(skill);
            if (!IsNormalAttackSkill(skill.skillid))
            {
                mCurNormalComboIndex = 0;
            }
            ActionSate = LogicObjectActionState.ReleasingSkill;
            if (skill.SKillCfg.skillType!= SKillType.StockPile)
            {
                releaseSkillCallBack?.Invoke(true);
                OnReleaseSkillCallBack?.Invoke(true, skill.skillid);
            }
        }
        else
        {
            releaseSkillCallBack?.Invoke(false);
            OnReleaseSkillCallBack?.Invoke(false, 0);
        }
    }
    /// <summary>
    /// 主动触发蓄力技能
    /// </summary>
    /// <param name="skillid"></param>
    public void TriggerStockPileSkill(int skillid)
    {
        mSkillSystem.TriggerStockPileSkill(skillid);
    }
    /// <summary>
    /// 是否是普通攻击技能
    /// </summary>
    /// <param name="skillid">校验的技能id</param>
    /// <returns></returns>
    public bool IsNormalAttackSkill(int skillid)
    {
        foreach (var item in mNormalSkillidArr)
        {
            if (skillid == item)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 技能释放后摇
    /// </summary>
    /// <param name="skill"></param>
    public void OnSkillReleaseAfter(Skill skill)
    {
        if (!IsNormalAttackSkill(skill.skillid))
        {
            mCurNormalComboIndex = 0;
        }
        else
        {
            mCurNormalComboIndex++;
            //如果当前普通攻击技能所以大于等级普通攻击技能数组长度，索引归0
            if (mCurNormalComboIndex >= mNormalSkillidArr.Length || skill.skillid == mNormalSkillidArr[mNormalSkillidArr.Length - 1])
            {
                mCurNormalComboIndex = 0;
            }
        }
    }
    /// <summary>
    /// 技能释放完成
    /// </summary>
    /// <param name="skill"></param>
    public void OnSkillReleaseEnd(Skill skill)
    {
        releaseingSkillList.Remove(skill);
        if (releaseingSkillList.Count == 0)
        {
            ActionSate = LogicObjectActionState.Idle;
            mCurNormalComboIndex = 0;
        }
    }

    public Skill GetSKill(int skillid)
    {
        return mSkillSystem.GetSKill(skillid);
    }
    /// <summary>
    /// 逻辑帧更新技能接口
    /// </summary>
    public void OnLogicFrameUpdateSkill()
    {
        if (mSkillSystem==null)
        {
            return;
        }
        mSkillSystem?.OnLogicFrameUpdate();
    }
    /// <summary>
    /// 添加一个buff
    /// </summary>
    /// <param name="buff"></param>
    public void AddBuff(Buff buff)
    {
        mBuffList.Add(buff);
    }
    /// <summary>
    /// 移除一个buff
    /// </summary>
    /// <param name="buff"></param>
    public void RemoveBuff(Buff buff)
    {
        if (mBuffList.Contains(buff))
        {
            mBuffList.Remove(buff);
        }
        if (ObjectState == LogicObjectState.Death)
        {
            return;
        }

        // if (mBuffList.Count == 0 && RenderObj.GetCurAnimName() != AnimationName.Anim_Getup)
        // {
        //     PlayAnim(AnimationName.Anim_Idle);
        //     ActionSate = LogicObjectActionState.Idle;
        // }
    }
}
