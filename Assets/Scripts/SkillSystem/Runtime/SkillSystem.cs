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
    public Skill ReleaseSkill(int skillId,Action onReleaseSkillAfter ,Action onReleaseSkillEnd , Action<bool> onForceSkillEnd = null)
    {
        var skill = _skillArr.FirstOrDefault(x => x.skillId == skillId);
        
        if (skill == null)
        {
            Debug.LogError("技能不存在，无法释放:" + skillId);
            return null;
        }
        
        skill.ReleaseSKill(onReleaseSkillAfter , onReleaseSkillEnd , onForceSkillEnd);
        
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
