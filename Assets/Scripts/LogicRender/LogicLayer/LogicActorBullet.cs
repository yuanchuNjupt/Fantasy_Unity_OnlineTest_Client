using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LogicActor 
{
    /// <summary>
    /// 
    /// </summary>
    private List<SKillBulletLogic> mBulletLogicList = new List<SKillBulletLogic>();

    /// <summary>
    /// 更新子弹逻辑帧
    /// </summary>
    public void OnLogicFramUpdateBullet()
    {
        //检查是否有失效子弹，并进行移除
        for (int i = mBulletLogicList.Count-1; i >=0; i--)
        {
            if (mBulletLogicList[i].isFailure)
            {
                RemoveBullet(mBulletLogicList[i]);
            }
        }

        //更新子弹逻辑帧
        foreach (var item in mBulletLogicList)
        {
            item.OnLogicFrameUpdate();
        }
    }
    //添加子弹逻辑层
    public void AddBullet(SKillBulletLogic bullet)
    {
        mBulletLogicList.Add(bullet);
    }
    //移除子弹逻辑层
    public void RemoveBullet(SKillBulletLogic bullet)
    {

        mBulletLogicList.Remove(bullet);
    }
}
