using FixMath;
using System.Collections;
using System.Collections.Generic;
using FixedPhysics.Bounds;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Core;
using Framework.AdvancedLog;
using UIFramework.Core;
using UnityEngine;

public partial class LogicActor : LogicObject
{
    public override void OnCreate()
    {
        base.OnCreate();
    }
 
    public override void OnLogicFrameUpdate()
    {
        base.OnLogicFrameUpdate();
        //更新移动帧
        OnLogicFrameUpdateMove();
        //更新技能帧
        OnLogicFrameUpdateSkill();
        //更新重力帧
        OnLogicFrameUpdateGravity();
    }

    public void PlayAnim(AnimationClip clip)
    {
        RenderObj.PlayAnim(clip);
    }
    public void PlayAnim(string name)
    {
        Debug.Log("释放技能："+name);
        RenderObj.PlayAnim(name);
    }
    public virtual void OnHit(SkillDamageConfig config)
    {
        RenderObj.OnHit();
        CalculateDamage(config.damageRate , DamageSource.SKill);
    }
    public virtual void AddHitEffect(string effectHitObjPath,int survivalTimeMs, LogicObject source)
    {
        RenderObj.AddHitEffect(effectHitObjPath, survivalTimeMs, source);
    }
    
    
    
    
    /// <summary>
    /// 计算伤害
    /// </summary>
    /// <param name="damage">伤害数值</param>
    /// <param name="source">伤害来源</param>
    public void CalculateDamage(FixedInt damage,DamageSource source)
    {
        if (ObjectState== LogicObjectState.Survival)
        {
            //1.对象逻辑层血量减少
            ReduceHP(damage);
            //2.判断对象是否死亡 如果死亡就处理死亡逻辑
            if (this.HP<=0)
            {
                Log.Info(LogColor.Red , "战斗系统" , $"对象死亡");
                OnDeath();
            }
        }
    }
    /// <summary>
    /// 对象死亡
    /// </summary>
    public virtual void OnDeath()
    {
        Collider.SetActive(false);
        ObjectState = LogicObjectState.Death;
        RenderObj.OnDeath();
        OnDeathCallBack?.Invoke();
    }
    /// <summary>
    /// 浮空回调，
    /// </summary>
    /// <param name="uploating">是否处于上浮</param>
    public virtual void Floating(bool upfoating) {}
    /// <summary>
    /// 对象触地
    /// </summary>
    /// <param name="upfoating"></param>
    public virtual void TriggerGround() { }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void SetRenderObj(RenderObject renderObj)
    {
        this.RenderObj = renderObj;
    }
    
}
