using FixIntPhysics;
using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SKillBulletLogic : LogicObject
{
    /// <summary>
    /// 子弹隶属技能
    /// </summary>
    private Skill mSKill;
    /// <summary>
    /// 子弹发射者
    /// </summary>
    private LogicActor mFireLogicActor;
    /// <summary>
    /// 子弹配置
    /// </summary>
    private SkillBulletConfig mBulletCfg;

    /// <summary>
    /// 当前子弹碰撞体
    /// </summary>
    private ColliderBehaviour mBulletCollider;
    //当前逻辑帧
    private int mCurLogicFrame = 0;
    //当前逻辑帧累加时间
    private int mCurLogicFrameAccTime;
    //子弹是否击中目标
    private bool mBulletIsHit=false;
    //是否失效
    public bool isFailure = false;   
    //子弹当前帧击中目标列表
    private List<LogicActor> mHitTargetList = new List<LogicActor>();
    public SKillBulletLogic(Skill skill, LogicActor fireLogicActor, RenderObject renderObj, SkillBulletConfig bulletCfg, FixIntVector3 rangePos)
    {
        mSKill = skill;
        mFireLogicActor = fireLogicActor;
        RenderObj = renderObj;
        mBulletCfg = bulletCfg;
        //帧同步中处理随机改怎么处理  1.不能使用纯随机数。 答：应该使用基于随机种子的伪随机数
        //什么是 随机种子 PPT
        //初始化对象位置
        //更新轴向

        LogicXAxis = fireLogicActor.LogicXAxis;

        //初始化逻辑对象方向
        LogicDir = new FixIntVector3(LogicXAxis,0,0)+new FixIntVector3(bulletCfg.dir);
         //初始化当前对象偏移位置
        FixIntVector3 pos = LogicXAxis * (new FixIntVector3(mBulletCfg.offset) + rangePos);
        pos.y= FixIntMath.Abs(pos.y);
        LogicPos = fireLogicActor.LogicPos + pos;

        //更新旋转角度
        LogicAngle = new FixIntVector3(bulletCfg.angle) * LogicXAxis;
        
        //当前这个子弹是否附加伤害
        if (bulletCfg.isAttachDamage)
        {
            SkillDamageConfig damageCfg = bulletCfg.damageCfg;
            if (damageCfg.detectionMode== DamageDetectionMode.BOX3D)
            {
                // mBulletCollider = new FixIntBoxCollider(FixIntVector3.zero,FixIntVector3.zero);
            }
            else if (damageCfg.detectionMode == DamageDetectionMode.Sphere3D)
            {
                // mBulletCollider = new FixIntSphereCollider(damageCfg.raduis, FixIntVector3.zero);
            }
        }
    }
    /// <summary>
    /// 处理子弹碰撞和子弹移动逻辑
    /// </summary>
    public override void OnLogicFrameUpdate()
    {
        base.OnLogicFrameUpdate();
        //计算逻辑帧累加时间
        mCurLogicFrameAccTime = mCurLogicFrame * LogicFrameConfig.LogicFrameIntervalms;
        //逻辑帧自增
        mCurLogicFrame++;
       
        //子弹击中逻辑
        foreach (LogicActor target in mHitTargetList)
        {
      
            //造成子弹伤害
            // target.BulletDamage(DamageCalcuCenter.CaclulateDamage(mBulletCfg.damageCfg, mFireLogicActor,target),mBulletCfg.damageCfg);
            //播放击中效果
            target.OnHit(mBulletCfg.hitEffectPath ,mBulletCfg.hitEffectSurvivalTimems,this,LogicXAxis);
            //处理击中音效
            if (mBulletCfg.hitAudio!=null)
            {
                // AudioController.GetInstance().PlaySoundByAudioClip(mBulletCfg.hitAudio,false,1);
            }
            //处理子弹附加的Buff
            AttachBuff(target);

            if (mBulletCfg.isHitDestory)
            {
                Release();
                break;
            }
        }
        //击中目标处理完成，清理缓存数据
        if (mHitTargetList.Count>0)
        {
            mHitTargetList.Clear();
        }
        //子弹碰撞体位置更新
        if (mBulletCollider != null)
        {
            if (mBulletCfg.damageCfg.colliderPosType== ColliderPosType.FollowPos)
            {
                //更新子弹碰撞体位置
                if (mBulletCfg.damageCfg.detectionMode== DamageDetectionMode.BOX3D)
                {
                    FixIntVector3 offset = LogicXAxis * new FixIntVector3(mBulletCfg.damageCfg.boxOffset);
                    // mBulletCollider.SetBoxData(offset,new FixIntVector3(mBulletCfg.damageCfg.boxSize));
                    mBulletCollider.UpdateColliderInfo(LogicPos, new FixIntVector3(mBulletCfg.damageCfg.boxSize));
                }
                else if (mBulletCfg.damageCfg.detectionMode == DamageDetectionMode.Sphere3D)
                {
                    FixIntVector3 offset = LogicXAxis * new FixIntVector3(mBulletCfg.damageCfg.sphereOffset);
                    // mBulletCollider.SetBoxData(mBulletCfg.damageCfg.raduis, offset);
                    mBulletCollider.UpdateColliderInfo(LogicPos,FixIntVector3.zero, mBulletCfg.damageCfg.raduis);
                }
            }

            //获取场景中所有的敌人
            // List<LogicActor> enemyList= BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(mFireLogicActor.ObjectType);
            //计算子弹碰撞体是否击中敌人
            // foreach (var target in enemyList)
            // {
            //     if (mBulletCfg.damageCfg.detectionMode == DamageDetectionMode.BOX3D)
            //     {
            //        mBulletIsHit= PhysicsManager.IsCollision(mBulletCollider as FixIntBoxCollider, target.Collider);
            //     }
            //     else if (mBulletCfg.damageCfg.detectionMode == DamageDetectionMode.Sphere3D)
            //     {
            //         mBulletIsHit = PhysicsManager.IsCollision(target.Collider, mBulletCollider as FixIntSphereCollider);
            //     }
            //     //把击中的目标添加至列表
            //     if (mBulletIsHit)
            //     {
            //         mHitTargetList.Add(target);
            //     }
            // }
        }

        //子弹位置更新
        LogicPos += LogicDir * (FixInt)mBulletCfg.moveSpeed * (FixInt)LogicFrameConfig.LogicFrameInterval;
        //当前运行时间，达到了子弹存活，就销毁子弹
        if (mCurLogicFrameAccTime>=mBulletCfg.survivalTimeMsg)
        {
            Release();
        }
    }
    /// <summary>
    /// 附加子弹击中buff
    /// </summary>
    public void AttachBuff(LogicActor target)
    {

        if (mBulletCfg.damageCfg.addBuffs!=null&& mBulletCfg.damageCfg.addBuffs.Length>0)
        {
            foreach (var buffid in mBulletCfg.damageCfg.addBuffs)
            {
                BuffSystem.MainInstance.AttachBuff(buffid,mFireLogicActor, target,mSKill);
            }
        }
    }
    public void Release()
    {
        RenderObj.OnRelease();
        mBulletCollider?.OnRelease();
        mBulletCollider = null;
        isFailure = true;
    }
}
