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
    protected float smoothPosSpeed = 10;

    protected bool isUpdatePosAndRotation = true;
    protected Vector3 renderDir;

    protected bool isLocalPlayer = false;
    protected Vector3 preTargetPos;//预测位置
    protected Vector3 preTargetDir;//预测朝向
    /// <summary>
    /// 当前预测的移动次数
    /// </summary>
    protected int curPreMoveCount;

    public void SetLogicObject(LogicObject logicObj,bool isUpdatePosAndRotation=true,bool isLocalPlayer=false)
    {
        logicObject = logicObj;
        this.isUpdatePosAndRotation = isUpdatePosAndRotation;
        this.isLocalPlayer=isLocalPlayer;
        //初始化位置
        transform.position = logicObj.LogicPos.ToVector3();
        preTargetPos = logicObj.LogicPos.ToVector3();  // 初始化预测目标位置
        preTargetDir = logicObj.LogicForwardDir.ToVector3(); // 初始化预测朝向
        if (this.isUpdatePosAndRotation == false)
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
        Destroy(gameObject);
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
        if (logicObject == null || isUpdatePosAndRotation == false)
        {
            return;
        }
        //如果是本地玩家，为了玩家操作的体验感和流畅度，需要预测本地玩家的渲染位置（与逻辑位置无关，当逻辑位置更新的时候，需要立即回滚角色的渲染位置）
        //战斗中所有逻辑运算都是基于逻辑位置进行运算的，所以我们这里预测渲染位置是不影响游戏逻辑的。
        //主要是应对弱网
        if (isLocalPlayer)
        {
            //逻辑位置是否是最新，如果是，立马更新并回滚预测位置
            if (isUpdatePosAndRotation == true)
            {
                if (logicObject.objectHasNewPos)
                {
                    //此时接收到了服务器下发的最新逻辑位置，强制更新渲染位置到最新逻辑位置，并重置预测位置和预测计数
                    preTargetPos = logicObject.LogicPos.ToVector3();
                    preTargetDir = logicObject.LogicForwardDir.ToVector3(); // 同时同步朝向
                    logicObject.objectHasNewPos = false;
                    curPreMoveCount = 0;
                }
                else
                {
                    //位置的预测.达到最大预测次数则停止增量更新，但继续插值到最后的预测位置
                    if (curPreMoveCount <= LogicFrameConfig.PreMaxMoveLogicFrameCount)
                    {
                        //计算预测的增量位置
                        Vector3 deltaPos = logicObject.LogicForwardDir.ToVector3() * (logicObject.LogicMoveSpeed.RenderFloat * Time.deltaTime);
                        preTargetPos += deltaPos;
                        curPreMoveCount++;
                    }
                    // 无论是否超限，都实时更新预测朝向（跟随逻辑朝向变化）
                    preTargetDir = logicObject.LogicForwardDir.ToVector3();
                }
                //更新位置
                transform.position = Vector3.Lerp(transform.position, preTargetPos, Time.deltaTime * smoothPosSpeed);
                return;
            }

        }

        //对逻辑位置做插值动画，流畅渲染对象移动
        transform.position = Vector3.Lerp(transform.position, logicObject.LogicPos.ToVector3(), Time.deltaTime * smoothPosSpeed);
    }
    /// <summary>
    /// 通用的方向更新逻辑
    /// </summary>
    public virtual void UpdateDir()
    {
        if (logicObject == null || isUpdatePosAndRotation == false)
        {
            return;
        }

        // 本地玩家朝向预测
        if (isLocalPlayer)
        {
            // 使用预测朝向而不是逻辑朝向，保持与位置预测的一致性
            renderDir = preTargetDir;
        }
        else
        {
            // 远程玩家直接使用逻辑朝向
            renderDir = logicObject.LogicForwardDir.ToVector3();
        }

        // 计算目标旋转角度（朝向移动方向）
        Quaternion targetRotation = Quaternion.LookRotation(renderDir);
        
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

    public virtual void OnHit()
    {
        //受击动画
        
    }
    
    
    public virtual void AddHitEffect(string effectHitObjPath, int survivalTimems, LogicObject source)
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
