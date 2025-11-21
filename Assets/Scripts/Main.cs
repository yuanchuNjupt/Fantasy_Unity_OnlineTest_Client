using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Account;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Generate;
using GGG.Tool.Singleton;
using UnityEngine;

public class Main : Singleton<Main>
{


    public UserData UserData { get; private set; }

    [Header("帧率设置")]
    public int FPS = 120;
    
    
    
    
    
    
    void Start()
    {
        StartAsync().Coroutine();
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
    
    public void SetUserData(string username,long accountId)
    {
        UserData = new UserData()
        {
            AccountId = accountId,
            UserName = username,
        };
    }




    private void OnDestroy()
    {
        NetWorkManager.Instance.OnRelease();
    }
}
