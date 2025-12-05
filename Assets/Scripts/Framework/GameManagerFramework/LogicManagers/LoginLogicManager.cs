using System;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using Framework.MessageManagers;
using Lobby;
using UIFramework.Core;
using UIFramework.ViewPath;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(LoginWorld))]
    public class LoginLogicManager : ILogicBehaviour
    {
        private LoginMessageManager _loginMessageManager;
        private UserDataManager _userDataManager;
        
        public void OnCreate()
        {
            _userDataManager = World.GetExitsDataManager<UserDataManager>();
            _loginMessageManager = World.GetExitsMessageManager<LoginMessageManager>();
        }

        public async void RegisterAccount(string account, string password)
        {
            if (String.IsNullOrEmpty(account) || String.IsNullOrEmpty(password))
            {
                Debug.LogWarning("账号或密码不能为空");
                return;
            }

            var res = await _loginMessageManager
                .SendRegisterAccountRequest(account, password);


            if (res != 0)
            {
                Debug.LogError("注册账号失败：" + res);
                return;
            }

            Debug.Log("注册账号成功，账号ID：" + res);
        }

        public async void RegisterName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("不可输入空的名称");
                return;
            }
            var res = await _loginMessageManager.SendRegisterNameRequest(_userDataManager.UserData.AccountName , name);

            if (res.ErrorCode != 0)
            {
                Debug.LogWarning("注册失败");
                return;
            }
        
            Debug.Log("注册成功，名称：" + res.name);
            _userDataManager.UserData.UserName = res.name;
            //注册成功，返回登陆界面
        
            WorldManager.CreateWorld<LobbyWorld>();
            WorldManager.DestroyWorld<LoginWorld>();
        }

        public async void LoginGame(string account, string password)
        {
            if(String.IsNullOrEmpty(account) || String.IsNullOrEmpty(password))
            {
                Debug.LogWarning("账号或密码不能为空");
                return;
            }
        
            //连接服务器并发送登录请求
        
            var res = await _loginMessageManager.SendLoginGameRequest(account, password);
        
            if (res.ErrorCode != 0)
            {
                Debug.LogError("登录失败 错误码：" + res.ErrorCode);
                return;
            }
            Debug.Log("登录成功 玩家ID：" + res.accountId);

            _userDataManager.UserData.UserName = res.accountName;
            _userDataManager.UserData.AccountId = res.accountId;
            _userDataManager.UserData.AccountName = account;


            if (res.accountName == null)
            {
                //需注册昵称
                UIManager.MainInstance.ShowPanel<RegisterNamePanelView>();
                UIManager.MainInstance.HidePanel<BeginPanelView>();
                return;
            }
        
            
            WorldManager.CreateWorld<LobbyWorld>();
            WorldManager.DestroyWorld<LoginWorld>();
        }
        

        public void OnDestroy()
        {
            _userDataManager = null;
            _loginMessageManager = null;
        }
    }
}