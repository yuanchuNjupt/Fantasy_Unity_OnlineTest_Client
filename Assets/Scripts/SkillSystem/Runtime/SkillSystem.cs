using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;


public class SkillSystem
{

    private readonly LogicActor _skillCharacter;

    private readonly List<Skill> _skillArr = new List<Skill>();

    private Skill _curReleasingSkill;

    private readonly List<int> _combinationSkillIdList = new List<int>();
    public SkillSystem(LogicActor logicActor)
    {
        _skillCharacter = logicActor;
    }

    public void InitSKills(List<int> skillIdArr)//1000 1001 1002
    {
        foreach (var skillid in skillIdArr)
        {
            Skill skill = new Skill(skillid, _skillCharacter);
            _skillArr.Add(skill);
            if (skill.SKillCfg.combinationSkillId != 0)
            {
                InitSKills(new List<int> { skill.SKillCfg.combinationSkillId });
            }
        }
        Debug.Log("技能初始化完成，技能个数:" + skillIdArr.Count);
    }
    public Skill ReleaseSkill(int skillId, Action<Skill> releaseAfterCallBack, Action<Skill> releaseSkillEnd )
    {
        //当前正在释放的技能不为空，并且技能状态不为结束或者后续，则无法释放新技能
        if (_curReleasingSkill!=null&&(_curReleasingSkill.skillState!= SkillState.End&&_curReleasingSkill.skillState!= SkillState.After))
        {
            return null;
        }
        
        //如果当前技能组合列表不为空，并且不包含当前要释放的技能id，则无法释放
        if (_combinationSkillIdList.Count>0&&!_combinationSkillIdList.Contains(skillId))
        {
            return null;
        }
        
        
        var skill = _skillArr.FirstOrDefault(x => x.skillId == skillId);
        if (skill == null)
        {
            Debug.LogError("技能不存在，无法释放:" + skillId);
            return null;
        }
        
        if (skill.skillState != SkillState.None && skill.skillState != SkillState.End)
        {
            Debug.Log("技能正在释放中，无法释放:" + skillId);
            return null;
        }
        
        if (skill.SKillCfg.combinationSkillId!=0)
        {
            CalculateCombinationSkillIdList(skill.SKillCfg.combinationSkillId);
        }
        
        skill.ReleaseSKill(releaseAfterCallBack , (skill, combinationSkill) =>
        {
            releaseSkillEnd?.Invoke(skill);
            if (!combinationSkill)
            {
                _curReleasingSkill = null;
                if (skill.SKillCfg.combinationSkillId==0&&_combinationSkillIdList.Count>0)
                {
                    _combinationSkillIdList.Clear();
                }
            }
        });
        
        _curReleasingSkill = skill;
        return skill;
    }
    
    public Skill GetSKill(int skillId)
    {
        return _skillArr.FirstOrDefault(item => item.skillId == skillId);
    }


    public void CalculateCombinationSkillIdList(int skillId) //1000,1001 ,1002
    {
        if (skillId!=0)
        {
            int combinationSkillId = skillId;
            while (combinationSkillId!=0)
            {
                _combinationSkillIdList.Add(combinationSkillId);
                combinationSkillId= GetSKill(combinationSkillId).SKillCfg.combinationSkillId;
            }

        }
    }
    public void OnLogicFrameUpdate()
    {
        foreach (var item in _skillArr)
        {
            item.OnLogicFrameUpdate();
        }
    }
}
