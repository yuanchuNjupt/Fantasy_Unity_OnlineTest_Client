using FixIntPhysics;
using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 只负责buff碰撞体的生成、和目标的检测，不处理buff的伤害相关逻辑。
/// 只单纯提供碰撞体相关API
/// </summary>
public class BuffCollider  
{
    /// <summary>
    /// 当前碰撞体实例
    /// </summary>
    private ColliderBehaviour mBuffCollider;
    /// <summary>
    /// Buff配置
    /// </summary>
    private BuffConfig mBuffCfg;
    /// <summary>
    /// Buff伤害配置
    /// </summary>
    private SkillDamageConfig mDamageCfg;

    /// <summary>
    /// Buff释放者
    /// </summary>
    private LogicActor mReleaser;

    /// <summary>
    /// Buff附加目标
    /// </summary>
    private LogicActor mAttachTarget;
    /// <summary>
    /// 当前Buff隶属技能
    /// </summary>
    private Skill mSKill;

    //初始化碰撞体所需数据
    public BuffCollider(Buff buff)
    {
        mBuffCfg = buff.BuffCfg;
        mDamageCfg = buff.BuffCfg.targetConfig.damageCfg;
        mReleaser = buff.releaser;
        mAttachTarget = buff.attachTarget;
        mSKill = buff.skill;
    }

    //1.生成对应的碰撞体

    /// <summary>
    /// 创建碰撞体
    /// </summary>
    public ColliderBehaviour CreateOrUpdateCollider(LogicObject followObj = null)
    {
        //创建对应的定点数碰撞体
        if (mDamageCfg.detectionMode == DamageDetectionMode.BOX3D)
        {
            FixIntVector3 boxSize = new FixIntVector3(mDamageCfg.boxSize);
            FixIntVector3 offset = new FixIntVector3(mDamageCfg.boxOffset) ;

            //限制y轴的偏移只能往上进行偏移
            offset.y = FixIntMath.Abs(offset.y);
            if (mBuffCollider == null)
                // mBuffCollider = new FixIntBoxCollider(boxSize, offset);

            // mBuffCollider.SetBoxData(offset, boxSize);
            mBuffCollider.UpdateColliderInfo(GetBuffPos(), boxSize);
        }
        else if (mDamageCfg.detectionMode == DamageDetectionMode.Sphere3D)
        {
            FixIntVector3 offset = new FixIntVector3(mDamageCfg.sphereOffset);
            //限制y轴的偏移只能往上进行偏移
            offset.y = FixIntMath.Abs(offset.y);

            if (mBuffCollider == null)
                // mBuffCollider = new FixIntSphereCollider(mDamageCfg.raduis, offset);

            // mBuffCollider.SetBoxData(mDamageCfg.raduis, offset);
            mBuffCollider.UpdateColliderInfo(GetBuffPos(), FixIntVector3.zero, mDamageCfg.raduis);
        }
        return mBuffCollider;
    }



    //2.更新碰撞体，检测有多少目标发生了碰撞
    public List<LogicActor> CacleColliderTargetObjects()
    {
        //1.获取敌人目标列表 敌人 英雄
        // List<LogicActor> enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(mReleaser.ObjectType);

        //2.通过碰撞检测逻辑，去检测碰撞到的敌人
        List<LogicActor> damageTargetList = new List<LogicActor>();
        // foreach (var target in enemyList)
        // {
        //     if (mBuffCollider.ColliderType == ColliderType.Box)
        //     {
        //         //如果返回值为True，说明两个碰撞体发生了碰撞
        //         if (PhysicsManager.IsCollision(mBuffCollider as FixIntBoxCollider, target.Collider))
        //         {
        //             damageTargetList.Add(target);
        //         }
        //     }
        //     else if (mBuffCollider.ColliderType == ColliderType.Shpere)
        //     {
        //         //如果返回值为True，说明两个碰撞体发生了碰撞
        //         if (PhysicsManager.IsCollision(target.Collider, mBuffCollider as FixIntSphereCollider))
        //         {
        //             damageTargetList.Add(target);
        //         }
        //     }
        // }
        return damageTargetList;
    }
    /// <summary>
    /// 获取Buff附加位置
    /// </summary>
    /// <returns></returns>
    public FixIntVector3 GetBuffPos()
    {
        if (mBuffCfg.attachType== BuffAttachType.Guide_Pos)
        {
            return mSKill.sKillGuidePos;
        }
        else if (mBuffCfg.attachType == BuffAttachType.Creator)
        {
            return mReleaser.LogicPos;
        }
        else if (mBuffCfg.attachType == BuffAttachType.Target)
        {
            return mAttachTarget.LogicPos;
        }
        else
        {
            return mReleaser.LogicPos;
        }
    }

    //3.释放当前碰撞体
    public void OnRelease()
    {
        mBuffCollider?.OnRelease();
        mBuffCollider = null;
    }
}
