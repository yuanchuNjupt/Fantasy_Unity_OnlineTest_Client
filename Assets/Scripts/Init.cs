using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Generate;
using GGG.Tool.Singleton;
using UnityEngine;

public class Init : Singleton<Init>
{


    public long PlayerId;

    [Header("帧率设置")]
    public int FPS = 120;
    
    
    
    
    
    
    void Start()
    {
        StartAsync().Coroutine();
        DontDestroyOnLoad(gameObject);
        
    }

    private async FTask StartAsync()
    {


        // await Fantasy.Platform.Unity.Entry.Initialize();
        //
        // //Scene客户端场景
        // _scene = await Scene.Create(SceneRuntimeMode.MainThread);

        await NetWorkManager.Instance.Initlization();

        Debug.Log("框架初始化完毕");

        // _session = _scene.Connect("192.168.101.193:20000", NetworkProtocolType.KCP, OnConnectSuccess, OnConnectFail, OnConnectDisconnect, false, 5000);

       Application.targetFrameRate = FPS;


    }




    private void OnDestroy()
    {
        NetWorkManager.Instance.OnRelease();
    }
}
