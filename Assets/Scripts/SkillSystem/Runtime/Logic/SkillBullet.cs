using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Skill
{
    //当前所有子弹累加时间列表
    private List<int> mCurCreateBulletAccTimeList = new List<int>();
    /// <summary>
    /// 随机种子随机数生成器
    /// </summary>
    private LogicRandom mLogicRandom;
    /// <summary>
    /// 初始化子弹相关数据
    /// </summary>
    public void OnBulletInit()
    {
        mLogicRandom = new LogicRandom(10);
        if (mSkillData.bulletCfgList != null && mSkillData.bulletCfgList.Count > 0)
        {
            for (int i = 0; i < mSkillData.bulletCfgList.Count; i++)
            {
                mCurCreateBulletAccTimeList.Add(0);
            }
        }
    }
    public void OnLogicFrameUpdateBullet()
    {
        if (mSkillData.bulletCfgList != null && mSkillData.bulletCfgList.Count > 0)
        {

            for (int i = 0; i < mSkillData.bulletCfgList.Count; i++)
            {
                mCurCreateBulletAccTimeList[i]+= LogicFrameConfig.LogicFrameIntervalms; 
                SkillBulletConfig item=  mSkillData.bulletCfgList[i];
                if (item.triggerFrame == mCurLogicFrame)
                {
                    //需要创建子弹
                    CreateBullet(item);
                }
                //判断子弹是否是循环创建
                if (item.isLoopCreate)
                {
                    //增强代码鲁棒性
                    if (item.loopIntervalMs==0)
                    {
                        Debug.LogError("item.loopIntervalMs == 0,不进行子弹循环创建");
                        continue;
                    }
                    //当前创建子弹累加时间超过了创建子弹的间隔，就创建子弹
                    while (mCurCreateBulletAccTimeList[i] >= item.loopIntervalMs)
                    {
                        CreateBullet(item);
                        mCurCreateBulletAccTimeList[i] -= item.loopIntervalMs;
                    }
                }
            }
        }
    }
  
    /// <summary>
    /// 创建子弹
    /// </summary>
    /// <param name="config">创建子弹配置</param>
    public void CreateBullet(SkillBulletConfig config)
    {
        //简单创建，减少对框架的依赖，目的是为了让大家学习起来更加简单，移植起来更加方便。
        //后期通过资源框架进行创建，并通过资源对象池进行管理。
        GameObject bulletObj = GameObject.Instantiate(config.bulletPrefab);
        // GameObject bulletObj = ZMAsset.Instantiate(config.bulletPrefabPath, null);
        //处理渲染层
        SkillBulletRender bulletRender= bulletObj.GetComponent<SkillBulletRender>();
        if (bulletRender==null)
        {
            bulletRender= bulletObj.AddComponent<SkillBulletRender>();
        }

        FixIntVector3 rangePos = FixIntVector3.zero;
        if (config.isLoopCreate)
        {
            //随机xyz轴坐标
            FixInt x = mLogicRandom.Range(config.minrandomRangeVect3.x, config.maxRandomRangeVect3.x);
            FixInt y = mLogicRandom.Range(config.minrandomRangeVect3.y, config.maxRandomRangeVect3.y);
            FixInt z = mLogicRandom.Range(config.minrandomRangeVect3.z, config.maxRandomRangeVect3.z);
            rangePos = new FixIntVector3(x, y, z);
        }
        //处理逻辑层
        SKillBulletLogic bulletLogic = new SKillBulletLogic(this,mSkillCreater, bulletRender, config, rangePos);
        bulletRender.SetRenderData(bulletLogic,config);
        mSkillCreater.AddBullet(bulletLogic);
    }

    public void OnBulletRelease()
    {
        mCurCreateBulletAccTimeList.Clear();
        mLogicRandom=null;
    }
}
