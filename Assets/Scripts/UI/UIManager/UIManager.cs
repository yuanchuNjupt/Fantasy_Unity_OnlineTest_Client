using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    //单例
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;
    //场景中canvas
    private Transform canvasTrans;
    private UIManager()
    {
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        //全游戏唯一的canvas 
        GameObject.DontDestroyOnLoad(canvas);
    }


    //里氏替换
    //用于存储显示着的面板
    //隐藏面板时 直接获取字典中的对应面板
    private Dictionary<string, BasePanel> panelDIc = new Dictionary<string, BasePanel>();

    //显示
    /// <summary>
    /// 
    /// </summary> 
    /// <typeparam name="T">面板名称</typeparam>
    /// <param name="isTimeStop">是否时停</param>
    /// <returns></returns>
    public T ShowPanel<T>(bool isTimeStop = false) where T : BasePanel
    {
        //规则 : T与面板名字必须一样
        string panelName = typeof(T).Name;
        //是否之前已经显示?
        if (panelDIc.ContainsKey(panelName))
        {
            return panelDIc[panelName] as T;
        }
        //创建预设体 建立父对象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));

        //得到后放到canvas下
        panelObj.transform.SetParent(canvasTrans , false);

        //指向面板 显示逻辑 并把它保存起来~
        T panel = panelObj.GetComponent<T>();
        //把面板脚本存到字典中 方便后续处理
        panelDIc.Add(panelName, panel);
        //显示
        if (isTimeStop)
        {
            panel.Show(() =>
            {
                Time.timeScale = 0;
            });
        }
        else
        {
            panel.Show(null);
        }


        //想用就返回
        return panel;
    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类名</typeparam>
    /// <param name="isFade">是否要淡入淡出</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if(panelDIc.ContainsKey(panelName))
        {
            //需要淡入淡出吗?
            if (isFade)
            {
                //淡出完再删
                panelDIc[panelName].Hide(() =>
                {
                    GameObject.Destroy(panelDIc[panelName].gameObject);
                    //删除字典里存的脚本
                    panelDIc.Remove(panelName);
                });
            }
            else
            {
                GameObject.Destroy(panelDIc[panelName].gameObject);
                //删除字典里存的脚本
                panelDIc.Remove(panelName);
            }
            
        }
    }
    //Get
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDIc.ContainsKey(panelName))
        {
            return panelDIc[panelName] as T;
        }
        //没面板 
        return null;
    }
}
