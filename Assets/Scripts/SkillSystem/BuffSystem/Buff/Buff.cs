using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuffState
{
    None,
    Delay,//延迟中
    Start,//开始触发
    Update,//Buff更新中
    End,//buff结束
}

public class Buff
{
    /// <summary>
    /// buff配置
    /// </summary>
    public BuffConfig BuffCfg { get; private set; }
    /// <summary>
    /// buff当前状态
    /// </summary>
    public BuffState buffState;
    /// <summary>
    /// Buff唯一id
    /// </summary>
    public readonly int Buffid;
    /// <summary>
    /// buff释放者
    /// </summary>
    public LogicActor releaser;
    /// <summary>
    /// buff附加/攻击目标
    /// </summary>
    public LogicActor attachTarget;
    /// <summary>
    /// 隶属技能
    /// </summary>
    public Skill skill;
    /// <summary>
    /// Buff所需要的一些参数
    /// </summary>
    public object[] paramsObjs;
    /// <summary>
    /// 当前延迟时间
    /// </summary>
    private int mCurDelayTime;
    /// <summary>
    /// Buff逻辑组合对象
    /// </summary>
    private BuffComposite mBuffLogic;
    /// <summary>
    /// buff渲染对象
    /// </summary>
    private BuffRender mBuffRender;
    /// <summary>
    /// 当前真实运行时间
    /// </summary>
    private int mCurRealRuntime;
    /// <summary>
    /// 当前累计运行时间
    /// </summary>
    private int mAccRumTime;

    public Buff(int buffid, LogicActor releaser, LogicActor attachTarget, Skill skill, object[] paramsObjs)
    {
        this.Buffid = buffid;
        this.releaser = releaser;
        this.attachTarget = attachTarget;
        this.skill = skill;
        this.paramsObjs = paramsObjs;
    }

    public void OnCreate()
    {
        //加载buff配置文件
        // BuffCfg = ZMAsset.LoadScriptableObject<BuffConfig>(AssetPathConfig.BUFF_DATA_PATH + Buffid + ".asset");
        if (BuffCfg.buffType == BuffType.Repel)
        {
            mBuffLogic = new RepelBuff(this);
        }
        else if (BuffCfg.buffType == BuffType.Floating)
        {
            mBuffLogic = new FloatingBuff(this);
        }
        else if (BuffCfg.buffType == BuffType.Stiff)
        {
            mBuffLogic = new StiffBuff(this);
        }
        else if (BuffCfg.buffType == BuffType.HP_Modify_Group)
        {
            mBuffLogic = new AttributeModify_Buff_Group(this);
        }
        else if (BuffCfg.buffType == BuffType.Grab)
        {
            mBuffLogic = new GrabBuff(this);
        }
        else if (BuffCfg.buffType == BuffType.IgnoreGravity)
        {
            mBuffLogic = new IgnoreGravityBuff(this);
        }
        else if (BuffCfg.buffType == BuffType.MoveSpeed_Modify_Single)
        {
            mBuffLogic = new AttributeModify_Buff_Single(this);
        }
        else if (BuffCfg.buffType == BuffType.AllowMove|| BuffCfg.buffType == BuffType.NotAllowDir)
        {
            mBuffLogic = new StatusModify_Buff_Single(this);
        }
        buffState = BuffCfg.buffDelay == 0 ? BuffState.Start : BuffState.Delay;
        mCurDelayTime = BuffCfg.buffDelay;
    
    }

    public void OnLogicFrameUpdate()
    {
        switch (buffState)
        {
            case BuffState.Delay:
                if (mCurDelayTime == BuffCfg.buffDelay)
                {
                    //处理BUff延迟逻辑
                    mBuffLogic.BuffDelay();
                }
                mCurDelayTime -= LogicFrameConfig.LogicFrameIntervalms;
                if (mCurDelayTime <= 0)
                {
                    buffState = BuffState.Start;
                }
                break;
            case BuffState.Start:
                BuffStart();
                BuffTrigger();
                //判断buff是否需要切换为更新状态，如果当前buff持续时间为有限或无限，才进入更新状态
                buffState = (BuffCfg.buffDurationms == -1 || BuffCfg.buffDurationms > 0) ? BuffState.Update : BuffState.End;
                break;
            case BuffState.Update:
                UpdateBuffLogic();
                break;
            case BuffState.End:

                OnDestroy();
                break;
        }
    }
    public void BuffStart()
    {
        CreateBuffEffect();
        mBuffRender?.InitBuffRender(releaser, attachTarget, BuffCfg, skill.sKillGuidePos);
        //1.调用buffStart接口
        mBuffLogic.BuffStart();
        attachTarget.AddBuff(this);
    }
    public void BuffTrigger()
    {
        mBuffLogic.BuffTrigger();
        switch (BuffCfg.buffTriggerAnim)
        {
            // case ObjectAnimationState.BeHit:
            //     attachTarget.PlayAnim(AnimationName.Anim_Beiji_01);
            //     break;
            // case ObjectAnimationState.Stiff:
            //     attachTarget.PlayAnim(AnimationName.Anim_Beiji_02);
            //     break;
        }
        //处理当前BUff需要播放的音效
        //if (BuffCfg.buffAudio != null)
        //{
        //    AudioController.GetInstance().PlaySoundByAudioClip(BuffCfg.buffAudio, false, 2);
        //}
    }
    public void UpdateBuffLogic()
    {
        int logicFrameintervalMs = LogicFrameConfig.LogicFrameIntervalms;
        //1.处理buff间隔逻辑
        if (BuffCfg.buffIntervalms > 0)
        {
            //当前累计运行时间是否大于buff触发间隔，如果大于就触发buff效果
            mCurRealRuntime += logicFrameintervalMs;
            if (mCurRealRuntime >= BuffCfg.buffIntervalms)
            {
                BuffTrigger();
                mCurRealRuntime -= BuffCfg.buffIntervalms;
            }
        }
        //处理当前Buff的持续时间
        UpdateBuffDurationTime();
    }
    public void UpdateBuffDurationTime()
    {
        mAccRumTime += LogicFrameConfig.LogicFrameIntervalms;
        if (mAccRumTime >= BuffCfg.buffDurationms)
        {
            buffState = BuffState.End;
        }
    }
    /// <summary>
    /// 创建buff特效
    /// </summary>
    public BuffRender CreateBuffEffect()
    {
        //读取BuffEffect配置，去生成对应的特效
        if (BuffCfg.effectConfig != null && BuffCfg.effectConfig.effect!=null)
        {
            GameObject buffEffect = GameObject.Instantiate(BuffCfg.effectConfig.effect);
            // GameObject buffEffect =ZM.AssetFrameWork.ZMAsset.Instantiate(BuffCfg.effectConfig.effectPath,null);
            mBuffRender = buffEffect.GetComponent<BuffRender>();
            if (mBuffRender == null)
            {
                mBuffRender = buffEffect.AddComponent<BuffRender>();
            }
            return mBuffRender;
        }
        return null;
    }
    public void OnDestroy()
    {
        mBuffRender?.OnRelease();
        mBuffLogic.BuffEnd();
        BuffSystem.MainInstance.RemoveBuff(this);
        attachTarget.RemoveBuff(this);
    }
}
