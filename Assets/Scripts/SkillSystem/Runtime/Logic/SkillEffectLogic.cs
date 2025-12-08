using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;
using FixIntPhysics;
public class SkillEffectLogic : LogicObject
{
    private LogicActor mSkillCreater;
    private SkillEffectConfig mEffectCfg;
    private ColliderBehaviour mCollider;
    private int mAccRumtime;//累计运行时间
    public SkillEffectLogic(LogicObjectType objType, SkillEffectConfig effectCfg, RenderObject renderObject, LogicActor skillCreater,Skill skill)
    {
        this.ObjectType = objType;
        this.RenderObj = renderObject;
        this.mSkillCreater = skillCreater;
        this.mEffectCfg = effectCfg;
        this.LogicXAxis = skillCreater.LogicXAxis;
        //初始化特效逻辑位置
        if (effectCfg.effectPosType == EffectPosType.FollowDir || effectCfg.effectPosType == EffectPosType.FollowPosDir)
        {
            FixIntVector3 offsetPos = new FixIntVector3(effectCfg.effectOffsetPos) * LogicXAxis;
            offsetPos.y = FixIntMath.Abs(offsetPos.y);
            LogicPos = skillCreater.LogicPos + offsetPos;
        }
        else if (effectCfg.effectPosType == EffectPosType.Zero)
        {
            LogicPos = FixIntVector3.zero;
        }
        else if (effectCfg.effectPosType== EffectPosType.GuidePos)
        {
            //起始位置
            FixIntVector3 initPos= skill.sKillGuidePos + mSkillCreater.LogicXAxis * new FixIntVector3(effectCfg.effectOffsetPos);
            initPos.y = FixIntMath.Abs(initPos.y);
            LogicPos = initPos;
        }
 
    }

    public void OnLogicFrameEffectUpdate(Skill skill, int curLogicFrame)
    {
        if (mEffectCfg.effectPosType == EffectPosType.FollowPosDir)
        {
            FixIntVector3 offsetPos = new FixIntVector3(mEffectCfg.effectOffsetPos) * LogicXAxis;
            offsetPos.y = FixIntMath.Abs(offsetPos.y);
            LogicPos = mSkillCreater.LogicPos + offsetPos;
        }
        //1.处理特效行动配置，让特效能够跟随配置移动
        if (mEffectCfg.isAttachAction && mEffectCfg.actionConfig.triggerFrame == curLogicFrame)
        {
            skill.AddMoveAction(mEffectCfg.actionConfig, this,mEffectCfg.effectOffsetPos, () =>
            {
                Debug.Log("MoveToAction Finish SkillEffectLogic");
                mCollider?.OnRelease();
                skill.DestroyEffect(mEffectCfg);
                mCollider = null;
            }, () =>
            {
                //特效移动逻辑帧回调
                //更新碰撞体位置
                if (mEffectCfg.damageConfig.isFollowEffect)
                {
                    skill.CreateOrUpdateCollider(mEffectCfg.damageConfig, mCollider, this);
                }
                if (mEffectCfg.isAttachDamage)
                {
                    //处理间隔性伤害
                    if (mEffectCfg.damageConfig.triggerIntervalMs != 0 && mCollider != null)
                    {
                        mAccRumtime += LogicFrameConfig.LogicFrameIntervalms;
                        if (mAccRumtime >= mEffectCfg.damageConfig.triggerIntervalMs)
                        {
                            skill.TriggerColliderDamage(mCollider, mEffectCfg.damageConfig);
                            mAccRumtime -= mEffectCfg.damageConfig.triggerIntervalMs;
                        }
                    }
                }
            });
        }
        //2.处理伤害配置，让伤害碰撞体能够跟随动效进行移动
        if (mEffectCfg.isAttachDamage)
        {
            //创建伤害碰撞体
            if (mEffectCfg.damageConfig.triggerFrame == curLogicFrame)
            {
                mCollider = skill.CreateOrUpdateCollider(mEffectCfg.damageConfig, null, this);
                if (mEffectCfg.damageConfig.triggerIntervalMs == 0)
                {
                    skill.TriggerColliderDamage(mCollider, mEffectCfg.damageConfig);
                }
            }


        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        RenderObj.OnRelease();
    }
}
