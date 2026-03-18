using System.Collections.Generic;
using System.Collections.Generic;
using Battle;
using Battle.CustomCollider;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.Fixed_pointNumber.FixedIntMath;
using FixedPhysics.FixedCollider.Colliders._3D;
using FixedPhysics.FixedCollider.Colliders.Types;
using Framework.AdvancedLog;
using UnityEngine;

/// <summary>
/// 伤害来源
/// </summary>
public enum DamageSource
{
    None,
    SKill,
    Buff,
    Bullet,
}

public partial class Skill
{
    /// <summary>
    /// 技能碰撞体字典，key 为配置的 HashCode
    /// </summary>
    private Dictionary<int, FixedIntCollider3D> mColliderDic = new();

    private void OnInitDamage()
    {
        // 每次技能启动时清空残留碰撞体
        OnDamageRelease();
    }

    /// <summary>
    /// 逻辑帧更新伤害，只负责生命周期管理
    /// 碰撞响应由 SkillDamageBoxCollider / SkillDamageCylinderCollider 内部处理
    /// </summary>
    public void OnLogicFrameUpdateDamage()
    {
        if (_skillData.damageCfgList == null || _skillData.damageCfgList.Count == 0)
            return;

        for (int i = 0; i < _skillData.damageCfgList.Count; i++)
        {
            SkillDamageConfig item = _skillData.damageCfgList[i];
            int hashCode = item.GetHashCode();

            // FollowPos 模式：每帧更新碰撞体位置跟随角色
            if (item.colliderPosType == ColliderPosType.FollowPos)
            {
                if (mColliderDic.TryGetValue(hashCode, out var followCollider) && followCollider != null)
                {
                    // Box 碰撞体需要同步更新旋转，再更新位置，确保 offset 始终沿角色当前朝向
                    if (followCollider is SkillDamageBoxCollider followBox)
                    {
                        followBox.UpdateRotation(skillCharacter.LogicRotationY);
                    }

                    followCollider.UpdatePosition(skillCharacter.LogicPos);
                }
            }

            // 触发帧：创建碰撞体并绑定伤害回调
            if (_curLogicFrame == item.triggerFrame)
            {
                DestroyDamageCollider(item);
                var collider = CreateDamageCollider(item);
                if (collider != null)
                    mColliderDic[hashCode] = collider;
            }

            // 结束帧：销毁碰撞体
            if (_curLogicFrame == item.endFrame)
            {
                DestroyDamageCollider(item);
            }
        }
    }

    /// <summary>
    /// 创建伤害碰撞体并绑定命中回调
    /// </summary>
    private FixedIntCollider3D CreateDamageCollider(SkillDamageConfig item)
    {
        if (item.detectionMode == DamageDetectionMode.Box3D)
        {
            var targetPos = skillCharacter.LogicPos + skillCharacter.LogicForwardDir * item.boxOffset.z +
                            skillCharacter.LogicRightDir * item.boxOffset.x +
                            new FixedIntVector3(0, item.boxOffset.y, 0);

            var box = new SkillDamageBoxCollider(
                targetPos,
                skillCharacter.LogicRotationY,
                item);

            // 绑定命中回调：由碰撞体内部检测到 BattlePlayerCollider 后回调此处
            box.OnHitPlayerCallBack += playerCollider =>
                OnPlayerHit(playerCollider, item);

            return box;
        }
        else if (item.detectionMode == DamageDetectionMode.Cylinder3D)
        {
            
            var targetPos = skillCharacter.LogicPos + skillCharacter.LogicForwardDir * item.cylinderOffset.z +
                            skillCharacter.LogicRightDir * item.cylinderOffset.x +
                            new FixedIntVector3(0, item.cylinderOffset.y, 0);
            
            var cyl = new SkillDamageCylinderCollider(
                targetPos,
                item);

            cyl.OnHitPlayerCallBack += playerCollider =>
                OnPlayerHit(playerCollider, item);

            return cyl;
        }

        // 暂不支持 Sphere3D
        return null;
    }

    /// <summary>
    /// 碰撞体命中玩家后的统一处理入口
    /// </summary>
    private void OnPlayerHit(BattlePlayerCollider playerCollider, SkillDamageConfig config)
    { 
        var target = playerCollider.playerLogicLayer;
        if (target == null || target.ObjectState != LogicObjectState.Survival)
            return;
        
        // 通过UID比对
        if (skillCharacter is BattlePlayerLogicLayer skillCasterLogicLayer)
        {
            if (skillCasterLogicLayer.instance.uid == target.instance.uid)
            {
                Log.Info(LogColor.Red, "伤害检测", 
                    $"检测到自伤，已拦截！释放者: {skillCasterLogicLayer.instance.playerName}(UID:{skillCasterLogicLayer.instance.uid}), " +
                    $"被命中者: {target.instance.playerName}(UID:{target.instance.uid})");
                return;
            }
        }
        
            
        Log.Info($"攻击命中！: {target.instance.playerName}, Skill: {_skillData.skillCfg.skillName}");


        // 伤害结算（TODO：接入伤害计算中心）
        // target.SkillDamage(DamageCalcuCenter.Calculate(config, skillCharacter, target), config);
        target.OnHit(config);

        
        
        // 击中特效
        if (config.hitEffectPrefab != null)
        {
            
            // target.AddHitEffect();
            
            //暂时先这么写
            GameObject hitEffect = GameObject.Instantiate(config.hitEffectPrefab , target.instance.logicLayer.LogicPos.ToVector3(), Quaternion.identity);
            GameObject.Destroy(hitEffect , config.hitEffectSurvivalTimeMs / 1000f);
        }
        // 击中音效
        if (config.hitAudioClip != null)
        {
            AudioSource.PlayClipAtPoint(config.hitAudioClip, target.instance.logicLayer.LogicPos.ToVector3());
        }
    }

    /// <summary>
    /// 销毁指定配置对应的伤害碰撞体
    /// </summary>
    private void DestroyDamageCollider(SkillDamageConfig item)
    {
        int hashCode = item.GetHashCode();
        if (mColliderDic.TryGetValue(hashCode, out var collider) && collider != null)
        {
            // OnDestroy 内部已自动调用 PhysicsManager3D.RemoveCollider3D
            collider.OnDestroy();
            mColliderDic.Remove(hashCode);
        }
    }

    /// <summary>
    /// 技能结束时销毁所有残留碰撞体
    /// </summary>
    public void OnDamageRelease()
    {
        foreach (var kv in mColliderDic)
            kv.Value?.OnDestroy();
        mColliderDic.Clear();
    }
}