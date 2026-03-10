using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;
using FixMath;
public partial class LogicActor
{
    
    //技能系统
    private SkillSystem _skillSystem;
    
    //普通攻击技能id数组
    private List<int> _normalSkillIdArr = new List<int>() { 1001, 1002};

    private List<int> _skillIdArr = new List<int>() {};

    //正在释放技能的列表
    public Skill currentSkill;
    
    //当前普通攻击连击索引
    private int _curNormalComboIndex = 0;
    
    //释放技能回调
    public Action<bool, int> OnReleaseSkillCallBack;
    
    //初始化技能列表
    public void InitActorSkill(List<int> normalAttackList , List<int> skillList)
    {
        _normalSkillIdArr = normalAttackList;
        _skillIdArr = skillList;
        _skillSystem = new SkillSystem(this);
        _skillSystem.InitSKills(_normalSkillIdArr);
        _skillSystem.InitSKills(_skillIdArr);
    }
    /// <summary>
    /// 释放普通攻击
    /// </summary>
    public void ReleaseNormalAttack()
    {
        if (LogicFrameConfig.IsUseLocalLogicFrame)
        {
            Debug.Log("释放普通攻击，当前连击索引：" + _curNormalComboIndex);
            
            ReleaseSKill(_normalSkillIdArr[_curNormalComboIndex]);
        }
        else
        {
            //输入技能释放操作
            // mBattleLogicLayer.ReleaseSkillInput(_normalSkillIdArr[_curNormalComboIndex]);
        }
    }
    /// <summary>
    /// 释放对应的技能
    /// </summary>
    /// <param name="skillId"></param>
    public void ReleaseSKill(int skillId, Action<bool> releaseSkillCallBack = null)
    {
        
        Skill skill = _skillSystem.ReleaseSkill(skillId,  OnSkillReleaseAfter, OnSkillReleaseEnd);
        //！=null 说明技能释放成功
        if (skill != null)
        {
            currentSkill = skill;
            if (!IsNormalAttackSkill(skill.skillId))
            {
                _curNormalComboIndex = 0;
            }
            ActionState = LogicObjectActionState.ReleasingSkillBefore;

            releaseSkillCallBack?.Invoke(true);
            OnReleaseSkillCallBack?.Invoke(true, skill.skillId);
        }
        else
        {
            releaseSkillCallBack?.Invoke(false);
            OnReleaseSkillCallBack?.Invoke(false, 0);
        }
    }
    /// <summary>
    /// 是否是普通攻击技能
    /// </summary>
    /// <param name="skillId">校验的技能id</param>
    /// <returns></returns>
    public bool IsNormalAttackSkill(int skillId)
    {
        return _normalSkillIdArr.Any(item => skillId == item);
    }
    /// <summary>
    /// 技能释放后摇
    /// </summary>
    /// <param name="skill"></param>
    public void OnSkillReleaseAfter(Skill skill)
    {
        
        ActionState = LogicObjectActionState.ReleasingSkillAfter;
        
        if (!IsNormalAttackSkill(skill.skillId))
        {
            _curNormalComboIndex = 0;
        }
        else
        {
            _curNormalComboIndex++;
            //如果当前普通攻击技能索引大于等级普通攻击技能数组长度，索引归0
            if (_curNormalComboIndex >= _normalSkillIdArr.Count || skill.skillId == _normalSkillIdArr[^1])
            {
                _curNormalComboIndex = 0;
            }
        }
    }
    /// <summary>
    /// 技能释放完成
    /// </summary>
    /// <param name="skill"></param>
    public void OnSkillReleaseEnd(Skill skill)
    {
        currentSkill = null;
        ActionState = LogicObjectActionState.Idle;
        _curNormalComboIndex = 0;
    }

    public Skill GetSKill(int skillId)
    {
        return _skillSystem.GetSKill(skillId);
    }
    /// <summary>
    /// 逻辑帧更新技能接口
    /// </summary>
    public void OnLogicFrameUpdateSkill()
    {
        if (_skillSystem==null)
        {
            return;
        }
        _skillSystem?.OnLogicFrameUpdate();
    }
   
}
