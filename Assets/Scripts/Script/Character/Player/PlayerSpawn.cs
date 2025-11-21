using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    //在角色被网络生成之后 需要做的操作
    //生成一个TP_Camera 让他看着自己
    // Start is called before the first frame update
    public GameObject TP_camera;

    private void Awake()
    {
        this.transform.position = new Vector3(0, 0, 0);

        Instantiate(TP_camera , this.transform.position - Vector3.forward * 2 , Quaternion.identity);

    }

    void Start()
    {
        
        //重置位置 重置生成的Camera位置
        GameObject Scenec_Camera = GameObject.FindWithTag("SceneCamera");

        Scenec_Camera.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
