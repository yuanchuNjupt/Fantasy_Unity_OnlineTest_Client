using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;
using FixMath;
using Framework.AdvancedLog;

public partial class LogicActor
{
    
    //技能系统
    protected SkillSystem skillSystem;
    
    //普通攻击技能id数组
    protected List<int> normalSkillIdArr;

    protected List<int> skillIdArr;

    //正在释放技能的列表
    public Skill currentSkill;
    
    //当前普通攻击连击索引
    protected int curNormalComboIndex = 0;
    
    
    
    //初始化技能列表
    public void InitActorSkill(List<int> normalAttackList , List<int> skillList)
    {
        normalSkillIdArr = normalAttackList;
        skillIdArr = skillList;
        skillSystem = new SkillSystem(this);
        skillSystem.InitSKills(normalSkillIdArr);
        skillSystem.InitSKills(skillIdArr);
    }
    /// <summary>
    /// 释放普通攻击
    /// </summary>
    public virtual void ReleaseNormalAttack()
    {
        
    }

    
    
    
    
    /// <summary>
    /// 释放对应的技能
    /// </summary>
    /// <param name="skillId"></param>
    public void ReleaseSKill(int skillId)
    {

        if (ActionState is LogicObjectActionState.ReleasingSkillBefore)
        {
            Log.Info(LogColor.Orange , "释放技能","正在释放技能前摇，无法释放技能");
            return;
        }
        if(ActionState is LogicObjectActionState.ReleasingSkillAfter)
        {
            Log.Info(LogColor.Orange , "释放技能","处于当前技能后摇，强制结束技能并切换技能");
            currentSkill?.SkillForceEnd(false);
        }
        
        
        
        
        
        
        Log.Info(LogColor.Orange , "释放技能",$"释放技能，技能id：{skillId}");
        
        currentSkill = skillSystem.ReleaseSkill(skillId,OnSkillReleaseAfter,OnSkillReleaseEnd , ForceEndCurrentSkill);
        
        if (!IsNormalAttackSkill(currentSkill.skillId))
        {
            curNormalComboIndex = 0;
        }
        else
        {
            curNormalComboIndex++;
            //如果当前普通攻击技能索引大于等级普通攻击技能数组长度，索引归0
            if (curNormalComboIndex >= normalSkillIdArr.Count || currentSkill.skillId == normalSkillIdArr[^1])
            {
                curNormalComboIndex = 0;
            }
        }
        
        ActionState = LogicObjectActionState.ReleasingSkillBefore;

    }
    /// <summary>
    /// 是否是普通攻击技能
    /// </summary>
    /// <param name="skillId">校验的技能id</param>
    /// <returns></returns>
    public bool IsNormalAttackSkill(int skillId)
    {
        return normalSkillIdArr.Any(item => skillId == item);
    }
    /// <summary>
    /// 技能释放后摇
    /// </summary>
    public void OnSkillReleaseAfter()
    {
        ActionState = LogicObjectActionState.ReleasingSkillAfter;
        
    }
    /// <summary>
    /// 技能释放完成
    /// </summary>
    public void OnSkillReleaseEnd()
    {
        Log.Info(LogColor.Black , "技能释放完成" , $"技能释放完成，技能id：{currentSkill.skillId}");

        curNormalComboIndex = 0;
        currentSkill = null;
        ActionState = LogicObjectActionState.Idle;
    }
    
    public void ForceEndCurrentSkill(bool isResetNormalAttackIndex = true)
    {
        
        Log.Info(LogColor.Black , "强制中止技能" , $"技能id：{currentSkill.skillId}");
        if(isResetNormalAttackIndex)
        {
            curNormalComboIndex = 0;
        }
        
        
        currentSkill = null;
    }
    
    

    public Skill GetSKill(int skillId)
    {
        return skillSystem.GetSKill(skillId);
    }
    /// <summary>
    /// 逻辑帧更新技能接口
    /// </summary>
    public void OnLogicFrameUpdateSkill()
    {
        if (skillSystem==null)
        {
            return;
        }
        skillSystem?.OnLogicFrameUpdate();
    }
   
}
