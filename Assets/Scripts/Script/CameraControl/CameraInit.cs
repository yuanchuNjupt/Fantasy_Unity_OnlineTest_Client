using System.Collections;
using System.Collections.Generic;
using GGG.Tool.Singleton;
using UnityEngine;

public class CameraInit : Singleton<CameraInit>
{
    public TP_CameraControl _cameraControl;
    
    public Camera SceneCamera;
    
    public void InitPlayerCamera(Transform target)
    {
        SceneCamera.GetComponent<AudioListener>().enabled = false;
        _cameraControl.gameObject.transform.position = target.transform.position - target.transform.forward;
        _cameraControl._lookTarget = target.Find("CameraLookTarget").transform;
        _cameraControl.gameObject.SetActive(true);
    }
    
    public void DeInitPlayerCamera()
    {
        SceneCamera.GetComponent<AudioListener>().enabled = false;
        _cameraControl._lookTarget = null;
        _cameraControl.transform.position = new Vector3(0, 0, -1);
        _cameraControl.gameObject.SetActive(false);
    }
    
}
