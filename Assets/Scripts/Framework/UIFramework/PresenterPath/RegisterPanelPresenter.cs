using System;
using System.Collections;
using System.Collections.Generic;
using Fantasy;
using Fantasy.Network;
using Generate;
using UIFramework.Core;
using UIFramework.Presenter;
using UIFramework.ViewPath;
using UnityEngine;

public class RegisterPanelPresenter : BasePresenter<RegisterPanelView>
{
    private void Awake()
    {
        View.Exit.onClick.AddListener(() =>
        {
            //将自己隐藏
           UIManager.MainInstance.HideTopPanel(UILayer.Main); 
        });
        
        View.Register.onClick.AddListener(async () =>
        {
            
            if(String.IsNullOrEmpty(View.account.text) || String.IsNullOrEmpty(View.password.text))
            {
                Debug.LogWarning("账号或密码不能为空");
                return;
            }
            
            
            NetWorkManager.Instance.Connect("192.168.101.193:20000", NetworkProtocolType.KCP);

            var req = new RegisterAccountRequest();
            req.account = View.account.text;
            req.pass = View.password.text;

            var res = await NetWorkManager.Instance.Call<RegisterAccountResponse>(req);
            if (res.ErrorCode != 0)
            {
                Debug.LogError("注册账号失败：" + res.ErrorCode);
                return;
            }
            
            Debug.Log("注册账号成功，账号ID：" + res.account);
            NetWorkManager.Instance.DisConnect();
            
            
        });
    }

    void Update()
    {
        
    }
}
