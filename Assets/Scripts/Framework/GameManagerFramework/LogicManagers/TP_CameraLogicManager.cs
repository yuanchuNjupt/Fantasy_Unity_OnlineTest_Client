using System.Collections;
using System.Collections.Generic;
using Config;
using Framework.GameManagerFramework.WorldScripts;
using UnityEngine;

/// <summary>
/// 用于管理大厅和战斗中的第三人称摄像机逻辑
/// </summary>
[WorldSource(typeof(PermanentlyWorld))]
public class TP_CameraLogicManager : ILogicBehaviour
{
    public TP_CameraControl cameraControl;
    public void OnCreate()
    {
        
    }

    public void InitTPCamera(Transform lookTarget)
    {
        GameObject cameraObj = Object.Instantiate(Resources.Load<GameObject>(LoadPathConfig.TPCameraPath));
        cameraControl = cameraObj.GetComponent<TP_CameraControl>();
        cameraControl.gameObject.transform.position = lookTarget.transform.position - lookTarget.transform.forward;
        cameraControl._lookTarget = lookTarget.Find("CameraLookTarget").transform;
    }

    public void OnDestroy()
    {
    }
}
