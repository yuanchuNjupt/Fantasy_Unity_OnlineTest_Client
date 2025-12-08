using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeModify_Buff_Group : BuffComposite
{
    /// <summary>
    /// 配置伤害值
    /// </summary>
    private FixInt configValue;
    /// <summary>
    /// 当前buff对应的碰撞体
    /// </summary>
    private BuffCollider mBuffCollider;
    public AttributeModify_Buff_Group(Buff buff) : base(buff) { }


    public override void BuffDelay()
    {

    }
    public override void BuffStart()
    {
        //获取配置参数
        if (mBuff.BuffCfg.buffParamsList.Count>0)
            configValue = mBuff.BuffCfg.buffParamsList[0].value;

        if (mBuff.BuffCfg.targetConfig.isOpen)
        {
            //使用组合模式：生成对应的碰撞体 
            mBuffCollider = new BuffCollider(mBuff);
            mBuffCollider.CreateOrUpdateCollider();
        }

    }
    public override void BuffTrigger()
    {
        //检测碰撞体碰撞到的所有目标
        if (mBuff.BuffCfg.targetConfig.isOpen)
        {
            //获取当前碰撞体碰撞到的所有的目标
            List<LogicActor> targetList = mBuffCollider.CacleColliderTargetObjects();
            for (int i = 0; i < targetList.Count; i++)
            {
                //获取Buff击中目标
                LogicActor target = targetList[i];
                if (target.ObjectState!= LogicObjectState.Death)
                {
                    //造成伤害
                    // target.BuffDamage(DamageCalcuCenter.CaclulateDamage(mBuff.BuffCfg, mBuff.releaser, target), mBuff.BuffCfg.targetConfig.damageCfg);
                    target.OnHit(mBuff.BuffCfg.buffHitEffectObjPath,1,mBuff.releaser,mBuff.releaser.LogicXAxis);
                    //处理造成伤害后的BUff的附加
                    int[] buffidArr = mBuff.BuffCfg.targetConfig.damageCfg.addBuffs;
                    if (buffidArr!=null&&buffidArr.Length>0)
                    {
                        for (int k = 0; k < buffidArr.Length; k++)
                        {
                            BuffSystem.MainInstance.AttachBuff(buffidArr[k],mBuff.releaser,target,mBuff.skill);
                        }
                    }
                }
            }
            targetList.Clear();
            targetList = null;
        }
    }
    public override void BuffEnd()
    {
        //销毁当前碰撞体
        mBuffCollider?.OnRelease();
        mBuffCollider = null;
    }
}
