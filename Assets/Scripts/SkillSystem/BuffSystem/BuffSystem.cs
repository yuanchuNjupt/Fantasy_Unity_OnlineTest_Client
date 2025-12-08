using System.Collections;
using System.Collections.Generic;
using GGG.Tool.Singleton;
using UnityEngine;

/// <summary>
/// 管理所有的buff 释放、移除 、更新逻辑 
/// Buff 全局类型，环境类型，地图类型，角色死亡。GM 
/// </summary>
public class BuffSystem : Singleton<BuffSystem>
{
    /// <summary>
    /// 所有buff列表
    /// </summary>
    private List<Buff> mBuffList = new List<Buff>();


    public void OnCreate()
    {

    }
    /// <summary>
    /// 附加一个buff
    /// </summary>
    /// <param name="buffid">id</param>
    /// <param name="releaser">Buff施法者</param>
    /// <param name="attachTarget">Buff附加目标</param>
    /// <param name="skill">buff来源</param>
    /// <param name="paramsObjs">buff所需参数</param>
    /// <returns></returns>
    public Buff AttachBuff(int buffid, LogicActor releaser, LogicActor attachTarget, Skill skill, object[] paramsObjs=null)
    {
        if (buffid==0)
        {
            Debug.LogError("Buff id 不能为0，当前附加Buff为无效buff！");
            return null;
        }
        Buff buff = new Buff(buffid,releaser,attachTarget,skill,paramsObjs);
        buff.OnCreate();
        mBuffList.Add(buff);
        return buff;
    }
    /// <summary>
    /// 逻辑帧更新接口
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        for (int i = mBuffList.Count-1; i >=0; i--)
        {
            mBuffList[i].OnLogicFrameUpdate();
        }
    }
    /// <summary>
    /// 移除指定的buff
    /// </summary>
    /// <param name="buff"></param>
    public void RemoveBuff(Buff buff)
    {
        if (mBuffList.Contains(buff))
        {
            mBuffList.Remove(buff);
        }
    }
    public void OnDestory()
    {
        for (int i = mBuffList.Count - 1; i >= 0; i--)
        {
            mBuffList[i].OnDestroy();
        }
    }

}
