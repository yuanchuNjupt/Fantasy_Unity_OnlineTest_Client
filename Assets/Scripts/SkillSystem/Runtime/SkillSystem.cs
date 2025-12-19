using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ����ϵͳ
/// </summary>
public class SkillSystem
{

    private LogicActor mSkillCreater;

    private List<Skill> mSkillArr = new List<Skill>();

    private Skill mCurReleasingSkill;

    private List<int> mCombinationSkillIdList = new List<int>();
    public SkillSystem(LogicActor logicActor)
    {
        mSkillCreater = logicActor;
    }

    public void InitSKills(int[] skillidArr)//1000 1001 1002
    {
        foreach (var skillid in skillidArr)
        {
            Skill skill = new Skill(skillid, mSkillCreater);
            mSkillArr.Add(skill);
            if (skill.SKillCfg.ComobinationSkillid != 0)
            {
                InitSKills(new int[] { skill.SKillCfg.ComobinationSkillid });
            }
            if (skill.SKillCfg.stockPileStageData.Count>0)
            {
                foreach (var item in skill.SKillCfg.stockPileStageData)
                {
                    InitSKills(new int[] { item.skillid });
                }
            }
            if (skill.damageCfgList.Count > 0)
            {
                foreach (var item in skill.damageCfgList)
                {
                    if (item.triggerSkillid!=0)
                        InitSKills(new int[] { item.triggerSkillid });
                }
            }
        }
        Debug.Log("技能初始化完成，技能个数:" + skillidArr.Length);
    }
    public Skill ReleaseSkill(int skillid, FixIntVector3 guidePos,  Action<Skill> releaseAfterCallBack, Action<Skill> releaseSkillEnd )
    {
        if (mCurReleasingSkill!=null&&(mCurReleasingSkill.skillState!= SkillState.End&&mCurReleasingSkill.skillState!= SkillState.After))
        {
            return null;
        }
        if (mCombinationSkillIdList.Count>0&&!mCombinationSkillIdList.Contains(skillid))
        {
            return null;
        }
        
        
        var skill = mSkillArr.FirstOrDefault(x => x.skillid == skillid);
        if (skill == null)
        {
            Debug.LogError("技能不存在，无法释放:" + skillid);
            return null;
        }
        
        if (skill.skillState != SkillState.None && skill.skillState != SkillState.End)
        {
            Debug.Log("技能正在释放中，无法释放:" + skillid);
            return null;
        }
        if (skill.SKillCfg.ComobinationSkillid!=0)
        {
            CacleteCombinationSkillIdList(skill.SKillCfg.ComobinationSkillid);
        }
        skill.ReleaseSKill(releaseAfterCallBack , guidePos, (skill, combinationSkill) =>
        {
            releaseSkillEnd?.Invoke(skill);
            if (!combinationSkill)
            {
                mCurReleasingSkill = null;
                if (skill.SKillCfg.ComobinationSkillid==0&&mCombinationSkillIdList.Count>0)
                {
                    mCombinationSkillIdList.Clear();
                }
            }
        });
        mCurReleasingSkill = skill;
        return skill;
        
        
        // foreach (var skill in mSkillArr)
        // {
        //     if (skill.skillid == skillid)
        //     {
        //         if (skill.skillState != SkillState.None && skill.skillState != SkillState.End)
        //         {
        //             Debug.Log("技能正在释放中，无法释放:" + skillid);
        //             return null;
        //         }
        //         if (skill.SKillCfg.ComobinationSkillid!=0)
        //         {
        //             CacleteCombinationSkillIdList(skill.SKillCfg.ComobinationSkillid);
        //         }
        //         skill.ReleaseSKill(releaseAfterCallBack , guidePos, (skill, combinationSkill) =>
        //         {
        //             releaseSkillEnd?.Invoke(skill);
        //             if (!combinationSkill)
        //             {
        //                 mCurReleasingSkill = null;
        //                 if (skill.SKillCfg.ComobinationSkillid==0&&mCombinationSkillIdList.Count>0)
        //                 {
        //                     mCombinationSkillIdList.Clear();
        //                 }
        //             }
        //         });
        //         mCurReleasingSkill = skill;
        //         return skill;
        //     }
        // }
    }

    public void TriggerStockPileSkill(int skillid)
    {
        if (mCurReleasingSkill != null && mCurReleasingSkill.skillid!=skillid)
        {
            return;
        }

        if (mCombinationSkillIdList.Count > 0 && !mCombinationSkillIdList.Contains(skillid))
        {
            return;
        }
        Skill skill = GetSKill(skillid);
        if (skill != null)
        {
            skill.TriggerStockPileSkill();
        }
    }
    public Skill GetSKill(int skillid)
    {
        foreach (var item in mSkillArr)
        {
            if (item.skillid == skillid)
            {
                return item;
            }
        }
        return null;
    }


    public void CacleteCombinationSkillIdList(int skillid) //1000,1001 ,1002
    {
        if (skillid!=0)
        {
            int combinationSkillId = skillid;
            while (combinationSkillId!=0)
            {
                mCombinationSkillIdList.Add(combinationSkillId);
                combinationSkillId= GetSKill(combinationSkillId).SKillCfg.ComobinationSkillid;
            }

        }
        else
        {
            Debug.LogError("��Ч�ļ���id:"+skillid);
        }
    }
    public void OnLogicFrameUpdate()
    {
        foreach (var item in mSkillArr)
        {
            item.OnLogicFrameUpdate();
        }
    }
}
