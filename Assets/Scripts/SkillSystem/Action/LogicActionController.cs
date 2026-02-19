using System.Collections;
using System.Collections.Generic;
using GGG.Tool.Singleton;
using UnityEngine;
public class LogicActionController:Singleton<LogicActionController>
{
    /// <summary>
    /// 行动列表
    /// </summary>
    private List<ActionBehaviour> mActionList = new List<ActionBehaviour>();

    /// <summary>
    /// 开始进行行动
    /// </summary>
    /// <param name="action"></param>
    public void RunAciton(ActionBehaviour action)
    {
        action.actionFinish = false;
        mActionList.Add(action);
    }
    /// <summary>
    /// 逻辑帧更新帧
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        //移除已经完成的行动
        for (int i = mActionList.Count-1; i >=0 ; i--)
        {
           ActionBehaviour action=  mActionList[i];
            if (action.actionFinish)
            {
                action.OnActionFinish();
                RemoveAction(action);
            }
        }
        //更新逻辑帧
        foreach (var item in mActionList)
        {
            item.OnLogicFrameUpdate();
        }
    }
    /// <summary>
    /// 移除对应的行动
    /// </summary>
    /// <param name="action"></param>
    public void RemoveAction(ActionBehaviour action)
    {
        mActionList.Remove(action);
    }
    /// <summary>
    /// 脚本资源释放
    /// </summary>
    public void OnDestroy()
    {
        mActionList.Clear();
    }
}
