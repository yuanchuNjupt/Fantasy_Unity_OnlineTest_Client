
using System;
using Fantasy.Async;
using Framework.AdvancedLog;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.WorldScripts;
using Generate;
using UnityEngine;

/// <summary>
/// 全局唯一的游戏入口，负责游戏的初始化和全局管理
/// </summary>
public class Main : MonoBehaviour
{

    [Header("帧率设置")]
    public int FPS = 120;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartAsync().Coroutine();
    }

    private void Update()
    {
        WorldManager.OnWorldUpdate();
    }

    private async FTask StartAsync()
    {



        await NetWorkManager.Instance.Initlization();

        Log.Info(LogColor.Blue , "网络层初始化完成");
        Application.targetFrameRate = FPS;
        
        //全局配置
        WorldManager.CreateWorld<GlobalWorld>();
        
        //登录注册世界
        WorldManager.CreateWorld<LoginWorld>();
        

    }

    private void OnDestroy()
    {
        NetWorkManager.Instance.OnRelease();
        WorldManager.DestroyAllWorld();
        
    }
}
