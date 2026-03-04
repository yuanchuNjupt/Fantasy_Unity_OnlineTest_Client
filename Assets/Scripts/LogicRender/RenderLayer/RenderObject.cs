using FixMath;
using System.Collections;
using System.Collections.Generic;
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;

/// <summary>
/// 渲染对象
/// </summary>
public class RenderObject : MonoBehaviour
{
    /// <summary>
    /// 逻辑对象
    /// </summary>
    public LogicObject logicObject;
    /// <summary>
    /// 位置插值速度
    /// </summary>
    protected float mSmoothPosSpeed = 10;

    protected bool mIsUpdatePosAndRotation = true;
    protected Vector3 mRenderDir;

    protected bool mIsLocalPlayer = false;
    protected Vector3 mPreTargetPos;//预测位置
    /// <summary>
    /// 当前预测的移动次数
    /// </summary>
    protected int mCurPreMoveCount;

    public void SetLogicObject(LogicObject logicObj,bool isUpdatePosAndRotation=true,bool isLocalPlayer=false)
    {
        logicObject = logicObj;
        mIsUpdatePosAndRotation = isUpdatePosAndRotation;
        mIsLocalPlayer=isLocalPlayer;
        //初始化位置
        transform.position = logicObj.LogicPos.ToVector3();
        if (mIsUpdatePosAndRotation == false)
            transform.localPosition = Vector3.zero;
        UpdateDir();
    }
    /// <summary>
    /// 渲染层脚本创建
    /// </summary>
    public virtual void OnCreate()
    {

    }
    /// <summary>
    /// 渲染层脚本释放
    /// </summary>
    public virtual void OnRelease()
    {
        //ZMAsset.Release(gameObject,true);
    }
    /// <summary>
    /// Unity引擎渲染帧，根据程序配置，渲染帧一般一秒为30帧、和60帧以及120帧 
    /// </summary>
    public virtual void Update()
    {
        if (logicObject == null) return;
        UpdatePosition();
        UpdateDir();
    }
    /// <summary>
    ///通用的位置更新逻辑
    /// </summary>
    public virtual void UpdatePosition()
    {
        if (logicObject == null || mIsUpdatePosAndRotation == false)
        {
            return;
        }
        //如果是本地玩家，为了玩家操作的体验感和流畅度，需要预测本地玩家的渲染位置（与逻辑位置无关，当逻辑位置更新的时候，需要立即回滚角色的渲染位置）
        //战斗中所有逻辑运算都是基于逻辑位置进行运算的，所以我们这里预测渲染位置是不影响游戏逻辑的。
        //主要是应对弱网，
        if (mIsLocalPlayer)
        {
            //逻辑位置是否是最新，如果是，立马更新并回滚预测位置
            if (mIsUpdatePosAndRotation == true)
            {
                if (logicObject.objectHasNewPos)//是否有最新的位置
                {
                    mPreTargetPos = logicObject.LogicPos.ToVector3();
                    logicObject.objectHasNewPos = false;
                    mCurPreMoveCount = 0;
                    // Debuger.Log("PreMove ForceUpdate Pos:" + mPreTargetPos);
                }
                else
                {
                    //位置的预测.达到最大预测次数则停止
                    if (mCurPreMoveCount > LogicFrameConfig.PreMaxMoveLogicFrameCount)
                    {
                        return;
                    }
                    //计算预测的增量位置
                    Vector3 deltaPos = logicObject.LogicDir.ToVector3() * (logicObject.LogicMoveSpeed.RenderFloat * Time.deltaTime);
                    mPreTargetPos += deltaPos;
                    mCurPreMoveCount++;
                    // Debuger.Log("PreMove mPreTargetPos:" + mPreTargetPos);
                }
                //更新位置
                transform.position = Vector3.Lerp(transform.position, mPreTargetPos, Time.deltaTime * mSmoothPosSpeed);
                return;
            }

        }

        //对逻辑位置做插值动画，流畅渲染对象移动
        transform.position = Vector3.Lerp(transform.position, logicObject.LogicPos.ToVector3(), Time.deltaTime * mSmoothPosSpeed);
    }
    /// <summary>
    /// 通用的方向更新逻辑
    /// </summary>
    public virtual void UpdateDir()
    {
        if (logicObject == null || mIsUpdatePosAndRotation == false)
        {
            return;
        }

        mRenderDir = logicObject.LogicDir.ToVector3();
        

        // 计算目标旋转角度（朝向移动方向）
        Quaternion targetRotation = Quaternion.LookRotation(mRenderDir);
        
        // 平滑插值到目标旋转
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
    }
    public virtual void OnDeath()
    {

    }
    public virtual void PlayAnim(AnimationClip clip)
    {

    }
    public virtual void PlayAnim(string clipName)
    {

    }

    public virtual string GetCurAnimName()
    {
        return "";
    }
    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="damageValue">伤害值</param>
    /// <param name="source">伤害来源</param>
    public virtual void Damage(int damageValue, DamageSource source)
    {
        // GameObject damageItemObj = ZMAsset.Instantiate(AssetPathConfig.GAME_PREFABS + "DamageItem/DamageText", null);
        // DamageTextItem item = damageItemObj.GetComponent<DamageTextItem>();
        // item.ShowDamageText(damageValue, this);
    }
    public virtual void OnHit(string effectHitObjPath, int survivalTimems, LogicObject source)
    {
        if (!string.IsNullOrEmpty(effectHitObjPath))
        {
            //GameObject hitEffctObj= GameObject.Instantiate(effectHitObj);
            
            // GameObject hitEffctObj = ZMAsset.Instantiate(effectHitObjPath, null);
            // hitEffctObj.transform.position = source.RenderObj.transform.position; //纯表现逻辑，为了表现统一可以直接使用渲染位置
            // hitEffctObj.transform.localScale = source.LogicXAxis > 0 ? Vector3.one : new Vector3(-1,1,1);
            // //GameObject.Destroy(hitEffctObj, survivalTimems*1.0f/1000);
            // LogicTimerManager.Instance.DelayCall(survivalTimems * 1.0f / 1000, () => {
            //     ZMAsset.Release(hitEffctObj);
            // });
        }
    }
    public virtual Transform GetTransParent(TransParentType parentType) { return null; }

}
