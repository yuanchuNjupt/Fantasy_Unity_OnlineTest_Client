using System.Collections;
using System.Collections.Generic;
using GGG.Tool.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInit : Singleton<CameraInit>
{
    public TP_CameraControl cameraControl;
    
    public Camera sceneCamera;

    
    public void InitPlayerCamera(Transform target)
    {
        sceneCamera.GetComponent<AudioListener>().enabled = false;
        cameraControl.gameObject.transform.position = target.transform.position - target.transform.forward;
        cameraControl._lookTarget = target.Find("CameraLookTarget").transform;
        cameraControl.playerInput = target.GetComponent<PlayerInput>();
        cameraControl.gameObject.SetActive(true);
    }
    
    public void DeInitPlayerCamera()
    {
        sceneCamera.GetComponent<AudioListener>().enabled = false;
        cameraControl._lookTarget = null;
        cameraControl.playerInput = null;
        cameraControl.transform.position = new Vector3(0, 0, -1);
        cameraControl.gameObject.SetActive(false);
    }
    
}
