using System.Collections;
using System.Collections.Generic;
using GGG.Tool.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInit : Singleton<CameraInit>
{
    public TP_CameraControl cameraControl;
    

    public Camera PlayerCamera;

    
    public void InitPlayerCamera(Transform target)
    {
        cameraControl.gameObject.transform.position = target.transform.position - target.transform.forward;
        cameraControl._lookTarget = target.Find("CameraLookTarget").transform;
        cameraControl.gameObject.SetActive(true);
    }
    
    public void DeInitPlayerCamera()
    {
        cameraControl._lookTarget = null;
        cameraControl.transform.position = new Vector3(0, 0, -1);
        cameraControl.gameObject.SetActive(false);
    }
    
}
