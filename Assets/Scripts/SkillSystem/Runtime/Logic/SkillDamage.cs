using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixIntPhysics;
using FixMath;

/// <summary>
/// 伤害来源
/// </summary>
public enum DamageSource
{
    None,
    SKill,//技能伤害
    Buff,//Buff伤害
    Bullet,//子弹伤害
}
public partial class Skill
{
    /// <summary>
    /// 特效对象字典 key为特效配置的HashCode，Value为生成的对应的特效
    /// </summary>

    private Dictionary<int, ColliderBehaviour> mColliderDic = new Dictionary<int, ColliderBehaviour>();
    ///// <summary>
    ///// 当前伤害累加时间
    ///// </summary>
    //private int mCurDamageAccTime;

    //当前所有子弹累加时间列表
    private List<int> mCurDamageAccTimeList = new List<int>();

    private void OnInitDamage()
    {
        if (mSkillData.damageCfgList != null && mSkillData.damageCfgList.Count > 0)
        {
            for (int i = 0; i < mSkillData.damageCfgList.Count; i++)
            {
                mCurDamageAccTimeList.Add(0);
            }
        }
    }
    /// <summary>
    /// 逻辑帧更新伤害
    /// </summary>
    public void OnLogicFrameUpdateDamage()
    {
        //判断当前伤害配置列表是否为空，以及长度是否大于0
        if (mSkillData.damageCfgList != null && mSkillData.damageCfgList.Count > 0)
        {
            for (int i = 0; i < mSkillData.damageCfgList.Count; i++)
            {
                SkillDamageConfig item = mSkillData.damageCfgList[i];
                int hashCode = item.GetHashCode();
                if (item.colliderPosType == ColliderPosType.FollowPos)
                {
                    ColliderBehaviour damageCollider = null;
                    //更新碰撞体位置
                    if (mColliderDic.TryGetValue(item.GetHashCode(), out damageCollider) && damageCollider != null)
                    {
                        CreateOrUpdateCollider(item, damageCollider);
                    }
                }

                //创建碰撞体
                if (mCurLogicFrame == item.triggerFrame)
                {
                    DestroyCollider(item);
                    ColliderBehaviour collider = CreateOrUpdateCollider(item, null);
                    //创建字典缓存当前碰撞体
                    mColliderDic.Add(hashCode, collider);
                    if (item.triggerIntervalMs == 0)
                    {
                        //触发一次伤害//TOOD
                        if (mColliderDic.ContainsKey(hashCode))
                        {
                            TriggerColliderDamage(mColliderDic[hashCode], item);
                        }
                    }
                }

                //处理碰撞体伤害检测
                if (item.triggerIntervalMs != 0)
                {
                    //int mCurDamageAccTime = mCurDamageAccTimeList[i]; 值拷贝，非mCurDamageAccTimeList[i]对应的一个值
                    mCurDamageAccTimeList[i] += LogicFrameConfig.LogicFrameIntervalms;
                    //如果当前累加时间大于触发伤害间隔，那就造成伤害检测
                    if (mCurDamageAccTimeList[i] >= item.triggerIntervalMs)
                    {
                        //触发一次伤害//TOOD
                        mCurDamageAccTimeList[i] = 0;
                        if (mColliderDic.ContainsKey(hashCode))
                        {
                            TriggerColliderDamage(mColliderDic[hashCode], item);
                        }
                    }
                }

                //销毁碰撞体
                if (item.endFrame == mCurLogicFrame)
                {
                    DestroyCollider(item);
                }
            }
        }
    }
    /// <summary>
    /// 触发碰撞体伤害
    /// </summary>
    public void TriggerColliderDamage(ColliderBehaviour collider, SkillDamageConfig config)
    {
        //1.获取敌人目标列表 敌人 英雄
        // List<LogicActor> enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(mSkillCreater.ObjectType);

        //2.通过碰撞检测逻辑，去检测碰撞到的敌人
        List<LogicActor> damageTargetList = new List<LogicActor>();
        // foreach (var target in enemyList)
        // {
        //     if (collider.ColliderType == ColliderType.Box)
        //     {
        //         //如果返回值为True，说明两个碰撞体发生了碰撞
        //         if (PhysicsManager.IsCollision(collider as FixIntBoxCollider, target.Collider))
        //         {
        //             damageTargetList.Add(target);
        //         }
        //     }
        //     else if (collider.ColliderType == ColliderType.Shpere)
        //     {
        //         //如果返回值为True，说明两个碰撞体发生了碰撞
        //         if (PhysicsManager.IsCollision(target.Collider, collider as FixIntSphereCollider))
        //         {
        //             damageTargetList.Add(target);
        //         }
        //     }
        // }
        //释放列表
        // enemyList.Clear();
        //3.获取到攻击目标后，对这些敌人造成伤害
        foreach (var target in damageTargetList)
        {
            //造成伤害
            // target.SkillDamage(DamageCalcuCenter.CaclulateDamage(config,mSkillCreater,target), config);

            //添加Buff 
            if (config.addBuffs != null && config.addBuffs.Length > 0)
            {
                foreach (var buffid in config.addBuffs)
                {
                    BuffSystem.MainInstance.AttachBuff(buffid, mSkillCreater, target, this, null);
                }
            }
            //触发对应的后续技能 
            if (config.triggerSkillid != 0)
            {
                //预释放技能 这个技能会在当前技能释放完成后 立即进行释放
                mCombinationSkillid = config.triggerSkillid;
            }
            //添加击中特效
            AddHitEffect(target, config.targetType == TargetType.Self ? mSkillCreater : target);
            //播放击中音效
            PlayHitAudio();

        }
    }
    /// <summary>
    /// 添加击中特效
    /// </summary>
    public void AddHitEffect(LogicActor targetObj, LogicActor source)
    {
        if (mSkillData.skillCfg.skillHitEffect != null)
        {
            targetObj.OnHit(mSkillData.skillCfg.skillHitEffectPath, mSkillData.skillCfg.hitEffectSurvivalTimeMs, source, mSkillCreater.LogicXAxis);
        }
    }
    /// <summary>
    /// 创建碰撞体
    /// </summary>
    public ColliderBehaviour CreateOrUpdateCollider(SkillDamageConfig item, ColliderBehaviour damageCollider, LogicObject followObj = null)
    {
        ColliderBehaviour collider = damageCollider;
        LogicObject followTragetObj = followObj == null ? mSkillCreater : followObj;
        //创建对应的定点数碰撞体
        if (item.detectionMode == DamageDetectionMode.BOX3D)
        {
            FixIntVector3 boxSize = new FixIntVector3(item.boxSize);
            FixIntVector3 offset = new FixIntVector3(item.boxOffset) * followTragetObj.LogicXAxis;

            //限制y轴的偏移只能往上进行偏移
            offset.y = FixIntMath.Abs(offset.y);
            if (damageCollider == null)
            //     collider = new FixIntBoxCollider(boxSize, offset);
            //
            // collider.SetBoxData(offset, boxSize);
            collider.UpdateColliderInfo(followTragetObj.LogicPos, boxSize);
        }
        else if (item.detectionMode == DamageDetectionMode.Sphere3D)
        {
            FixIntVector3 offset = new FixIntVector3(item.sphereOffset) * followTragetObj.LogicXAxis;
            //限制y轴的偏移只能往上进行偏移
            offset.y = FixIntMath.Abs(offset.y);

            if (damageCollider == null)
            //     collider = new FixIntSphereCollider(item.raduis, offset);
            //
            // collider.SetBoxData(item.raduis, offset);
            collider.UpdateColliderInfo(followTragetObj.LogicPos, FixIntVector3.zero, item.raduis);
        }
        return collider;
    }



    /// <summary>
    /// 销毁对应配置生成的碰撞体
    /// </summary>
    /// <param name="item"></param>
    public void DestroyCollider(SkillDamageConfig item)
    {
        ColliderBehaviour collider = null;
        int hashCode = item.GetHashCode();
        mColliderDic.TryGetValue(hashCode, out collider);
        if (collider != null)
        {
            mColliderDic.Remove(hashCode);
            collider.OnRelease();
        }
    }

    public void OnDamageRelease()
    {
        mCurDamageAccTimeList.Clear();
    }
}
