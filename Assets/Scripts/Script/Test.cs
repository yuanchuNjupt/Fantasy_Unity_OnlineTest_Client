using System;
using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using UnityEngine;

public class Test
{
    
    
    //以下三行是为了实现单例模式
    
    private static Test _instance = new Test();
    
    //全局唯一访问入口
    public static Test Instance => _instance;
    
    private Test() { }
    
    
    //以下实现业务逻辑

    public void TestMethod()
    {
        Debug.Log("Test Method");
    }
    
    
}
