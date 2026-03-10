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

    // private Skill _curReleasingSkill;

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
        }
        Debug.Log("技能初始化完成，技能个数:" + skillIdArr.Count);
    }
    public Skill ReleaseSkill(int skillId,Action onReleaseSkillAfter ,Action onReleaseSkillEnd )
    {
        //当前正在释放的技能不为空，并且技能状态不为结束或者后续，则无法释放新技能
        // if (_curReleasingSkill!=null&&(_curReleasingSkill.skillState!= SkillState.End&&_curReleasingSkill.skillState!= SkillState.After))
        // {
        //     return null;
        // }
        
        var skill = _skillArr.FirstOrDefault(x => x.skillId == skillId);
        
        if (skill == null)
        {
            Debug.LogError("技能不存在，无法释放:" + skillId);
            return null;
        }
        
        // if (skill.skillState != SkillState.None && skill.skillState != SkillState.End)
        // {
        //     Debug.Log("技能正在释放中，无法释放:" + skillId);
        //     return null;
        // }
        

        
        // skill.ReleaseSKill(OnSkillReleaseAfterCallBack , () =>
        // {
        //     
        //     onReleaseSkillEnd?.Invoke();
        //     
        //     _curReleasingSkill = null;
        // });
        //
        // _curReleasingSkill = skill;
        
        
        skill.ReleaseSKill(onReleaseSkillAfter , onReleaseSkillEnd);
        
        
        
        
        return skill;
    }
    
    public Skill GetSKill(int skillId)
    {
        return _skillArr.FirstOrDefault(item => item.skillId == skillId);
    }
    

    public void OnLogicFrameUpdate()
    {
        foreach (var item in _skillArr)
        {
            item.OnLogicFrameUpdate();
        }
    }
}
