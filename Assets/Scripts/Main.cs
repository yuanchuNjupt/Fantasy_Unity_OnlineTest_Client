
using Fantasy.Async;
using Framework.GameManagerFramework.WorldScripts;
using Generate;
using UnityEngine;

public class Main : MonoBehaviour
{

    [Header("帧率设置")]
    public int FPS = 120;
    
    void Start()
    {
        StartAsync().Coroutine();
    }

    private async FTask StartAsync()
    {



        await NetWorkManager.Instance.Initlization();

        Debug.Log("Network框架初始化完毕");
        Application.targetFrameRate = FPS;
        
        //用户信息持久化世界
        WorldManager.CreateWorld<PermanentlyDataWorld>();
        
        //登录注册世界
        WorldManager.CreateWorld<LoginWorld>();
        
        
        
    }

    private void OnDestroy()
    {
        NetWorkManager.Instance.OnRelease();
    }
}
