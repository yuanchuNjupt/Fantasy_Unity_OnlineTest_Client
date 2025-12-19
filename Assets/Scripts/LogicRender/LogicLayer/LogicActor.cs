using FixMath;
using System.Collections;
using System.Collections.Generic;
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
        //更新子弹帧
        OnLogicFramUpdateBullet();
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
    public virtual void OnHit(string effectHitObjPath,int survivalTimeMs, LogicObject source,FixInt logicXAxis)
    {
        RenderObj.OnHit(effectHitObjPath, survivalTimeMs, source);
    }
    public virtual void SkillDamage(FixInt hp,SkillDamageConfig damageConfig)
    {
        Debug.Log("SkillDamage hp:"+hp);
        CaculDamage(hp, DamageSource.SKill);
    }

    public virtual void BuffDamage(FixInt hp, SkillDamageConfig damageConfig)
    {
        Debug.Log("BuffDamage hp:" + hp);
        CaculDamage(hp, DamageSource.SKill);
    }
    /// <summary>
    /// 某个技能或buff会减少或阻挡子弹伤害
    /// </summary>
    public virtual void BulletDamage(FixInt hp,SkillDamageConfig damageConfig)
    {
        Debug.Log("BulletDamage hp:" + hp);
        CaculDamage(hp, DamageSource.Bullet);
    }


    /// <summary>
    /// 计算伤害
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="source"></param>
    public void CaculDamage(FixInt hp,DamageSource source)
    {
        if (ObjectState== LogicObjectState.Survival)
        {
            //1.对象逻辑层血量减少
            ReduceHP(hp);
            //2.判断对象是否死亡 如果死亡就处理死亡逻辑
            if (this.HP<=0)
            {
               
                OnDeath();


            }
            //3.进行伤害飘字渲染
            RenderObj.Damage(hp.RawInt, source);
        }
    }
    /// <summary>
    /// 对象死亡
    /// </summary>
    public virtual void OnDeath()
    {
        Collider.Active = false;
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
}
