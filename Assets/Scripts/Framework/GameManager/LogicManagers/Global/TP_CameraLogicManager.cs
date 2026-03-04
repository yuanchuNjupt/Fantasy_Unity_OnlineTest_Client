using System.Collections;
using System.Collections.Generic;
using Config;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.WorldScripts;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 用于管理大厅和战斗中的第三人称摄像机逻辑
/// </summary>
[WorldSource(typeof(GlobalWorld))]
public class TP_CameraLogicManager : ILogicBehaviour
{
    public TP_CameraControl cameraControl;
    
    public void OnCreate()
    {
    }

    /// <summary>
    /// 初始化第三人称摄像机
    /// </summary>
    /// <param name="lookTarget">跟随目标（角色根节点 Transform）</param>
    /// <param name="cameraLookAction">当前场景激活的 CameraLook InputAction（Lobby/Battle各自的Map）</param>
    public void InitTPCamera(Transform lookTarget, InputAction cameraLookAction)
    {
        // 如果摄像机已存在且未被销毁，先销毁它
        if (cameraControl != null)
        {
            Object.Destroy(cameraControl.gameObject);
            cameraControl = null;
        }
        
        GameObject cameraObj = Object.Instantiate(Resources.Load<GameObject>(LoadPathConfig.TPCameraPath));
        cameraControl = cameraObj.GetComponent<TP_CameraControl>();
        cameraControl.gameObject.transform.position = lookTarget.transform.position - lookTarget.transform.forward;
        cameraControl._lookTarget = lookTarget.Find("CameraLookTarget").transform;
        
        // 注入当前场景对应的 CameraLook Action，不再硬绑定任何具体 Manager
        cameraControl.BindCameraLookAction(cameraLookAction);
    }

    public void OnDestroy()
    {
        // 清理摄像机对象
        if (cameraControl != null)
        {
            Object.Destroy(cameraControl.gameObject);
            cameraControl = null;
        }
    }
}
