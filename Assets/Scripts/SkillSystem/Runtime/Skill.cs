using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;

public enum SkillState
{
    None,
    Before,
    After,
    End,
}

public partial class Skill
{

    public int skillid;
    /// <summary>
    /// ���ܴ�����
    /// </summary>
    public LogicActor mSkillCreater;
    /// <summary>
    /// ������������
    /// </summary>
    private SkillDataConfig mSkillData;

    public SkillConfig SKillCfg { get { return mSkillData.skillCfg; } }
    /// <summary>
    /// �˺������б�
    /// </summary>
    public List<SkillDamageConfig> damageCfgList { get { return mSkillData.damageCfgList; } }
     /// <summary>
    /// �ͷż��ܺ�ҡ
    /// </summary>
    public Action<Skill> OnReleaseAfter;
    /// <summary>
    /// �ͷż��ܽ����ص�
    /// </summary>
    public Action<Skill, bool> OnReleaseSkillEnd;
    /// <summary>
    /// ����״̬
    /// </summary>
    public SkillState skillState = SkillState.None;
    /// <summary>
    /// ��ǰ�߼�֡
    /// </summary>
    private int mCurLogicFrame = 0;
    /// <summary>
    /// ��ǰ�ۼ�����ʱ��
    /// </summary>
    private int mCurLogicFrameAccTime = 0;
    /// <summary>
    /// �Ƿ��Զ�ƥ�������׶�
    /// </summary>
    private bool mAutoMacthStockStage;

    /// <summary>
    /// ��������λ��
    /// </summary>
    public FixedIntVector3 sKillGuidePos;
    /// <summary>
    /// ��ϼ���id
    /// </summary>
    private int mCombinationSkillid;
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="skillid">����id</param>
    /// <param name="skillCreater">���ܴ�����</param>
    public Skill(int skillid, LogicActor skillCreater)
    {
        this.skillid = skillid;
        this.mSkillCreater = skillCreater;
        
        //加载技能数据
        mSkillData = Resources.Load<ScriptableObject>(LoadPathConfig.SkillLoadPath + skillid) as SkillDataConfig;
        
        // 检查加载是否成功
        if (mSkillData == null)
        {
            Debug.LogError($"技能数据加载失败！技能ID: {skillid}");
        }
        // mSkillData = ZMAsset.LoadScriptableObject<SkillDataConfig>(AssetPathConfig.SKILL_DATA_PATH + skillid + ".asset");
    }

    public void ReleaseSKill(Action<Skill> releaseAfterCallBack, FixedIntVector3 guidePos, Action<Skill, bool> releaseSkillEnd)
    {
        OnReleaseAfter = releaseAfterCallBack;
        OnReleaseSkillEnd = releaseSkillEnd;
        sKillGuidePos = guidePos;
        SkillStart();
        skillState = SkillState.Before;
        PlayAnim();
    }

    public void PlayAnim()
    {
        mSkillCreater.PlayAnim(mSkillData.skillCfg.skillid.ToString());
    }

    public void SkillStart()
    {
        mCurLogicFrame = 0;
        mCurLogicFrameAccTime = 0;
        mAutoMacthStockStage = false;
        mCombinationSkillid = mSkillData.skillCfg.ComobinationSkillid;
         if (mSkillData.character.customLogicFame != 0)
            mSkillData.character.logicFrame = mSkillData.character.customLogicFame;
        // OnBulletInit();
        OnInitDamage();
    }

    public void SkillAfter()
    {
        skillState = SkillState.After;
        OnReleaseAfter?.Invoke(this);
    }

    public void SKillEnd()
    {
        skillState = SkillState.End;
        OnReleaseSkillEnd?.Invoke(this, mSkillData.skillCfg.ComobinationSkillid != 0);
        ReleaseAllEffect();
        // OnBulletRelease();
        OnDamageRelease();
        if (mCombinationSkillid != 0)
        {
            mSkillCreater.ReleaseSKill(mCombinationSkillid);
        }
    }


    public void OnLogicFrameUpdate()
    {
        if (skillState == SkillState.None||skillState== SkillState.End)
        {
            return;
        }
        mCurLogicFrameAccTime = mCurLogicFrame * LogicFrameConfig.LogicFrameIntervalms;

        if (skillState == SkillState.Before && mCurLogicFrameAccTime >= mSkillData.skillCfg.skillShakeArfterMs&&mSkillData.skillCfg.skillType!= SKillType.StockPile)
        {
            SkillAfter();
        }

        OnLogicFrameUpdateEffect();
        OnLogicFrameUpdateDamage();
        OnLogicFrameUpdateAction();
        OnLogicFrameUpdateAudio();
        // OnLogicFrameUpdateBullet();
        // OnLogicFrameUpdateBuff();
        
        if (mSkillData.skillCfg.skillType == SKillType.StockPile)
        {
            int stockDataCount = mSkillData.skillCfg.stockPileStageData.Count;
            if (stockDataCount > 0)
            {
                if (mAutoMacthStockStage)
                {
                    StockPileStageData stockData = mSkillData.skillCfg.stockPileStageData[0];
                    if (mCurLogicFrameAccTime >= stockData.startTimeMs)
                    {
                        StockPileFinish(stockData);
                    }
                }
                else
                {
                    StockPileStageData stockData = mSkillData.skillCfg.stockPileStageData[stockDataCount - 1];
                    if (mCurLogicFrameAccTime >= stockData.endTimeMs)
                    {
                        StockPileFinish(stockData);
                    }
                }
            }
        }
        else
        {
            if (mCurLogicFrame == mSkillData.character.MaxLogicFrame)
            {
                SKillEnd();
            }
        }

        if (mSkillData.skillCfg.showSkillPortrait && mCurLogicFrame==0)
        {
            mSkillCreater.RenderObj.ShowSkillPortrait(mSkillData.skillCfg.skillProtraitObj);
        }
        mCurLogicFrame++;
    }

    public void TriggerStockPileSkill()
    {
        foreach (var item in mSkillData.skillCfg.stockPileStageData)
        {
            if (mCurLogicFrameAccTime>=item.startTimeMs&&mCurLogicFrameAccTime<=item.endTimeMs)
            {
                StockPileFinish(item);
                return;
            }
        }
        mAutoMacthStockStage = true;
    }

    public void StockPileFinish(StockPileStageData stockData)
    {
        SKillEnd();
        if (stockData.skillid == 0)
        {
            Debug.LogError("");
        }
        else
        {
            mSkillCreater.ReleaseSKill(stockData.skillid);
        }
    }
}
