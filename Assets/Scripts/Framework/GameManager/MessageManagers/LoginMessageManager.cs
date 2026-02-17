using Config;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.LogicManagers;
using Framework.GameManagerFramework.WorldScripts;
using Generate;
using Lobby;
using UnityEngine;

namespace Framework.MessageManagers
{
    [WorldSource(typeof(LoginWorld))]
    public class LoginMessageManager : IMessageBehaviour
    {
        public void OnCreate()
        {
            
        }

        public async FTask<uint> SendRegisterAccountRequest(string account, string password)
        {
            NetWorkManager.Instance.Connect(NetWorkConfig.GateAddress , NetworkProtocolType.KCP);

            var req = new RegisterAccountRequest();
            req.account = account;
            req.pass = password;
            
            var res = await NetWorkManager.Instance.Call<RegisterAccountResponse>(req);
            NetWorkManager.Instance.DisConnect();
            return res.ErrorCode;
        }
        
        public async FTask<RegisterNameResponse> SendRegisterNameRequest(string accountName , string name)
        {
            var req = new RegisterNameRequest();
            req.accountName = accountName;
            req.name = name;
            return await NetWorkManager.Instance.Call<RegisterNameResponse>(req);
        }

        
        public async FTask<LoginResponse> SendLoginGameRequest(string accountName, string password)
        {
            NetWorkManager.Instance.Connect(NetWorkConfig.GateAddress, NetworkProtocolType.KCP);
        
            LoginRequest req = new LoginRequest();
            req.account = accountName;
            req.pass = password;

            var res = await NetWorkManager.Instance.Call<LoginResponse>(req);
            return res;
        }
        
        

        public void OnDestroy()
        {
            
        }
    }
}