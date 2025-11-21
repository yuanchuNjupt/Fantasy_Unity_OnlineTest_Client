using System;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using UnityEngine;

namespace Generate
{
    public class NetWorkManager
    {
        private static NetWorkManager _instance;

        public static NetWorkManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NetWorkManager();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Fantasy Scene
        /// </summary>
        private Scene mScene;

        /// <summary>
        /// 网络会话Socket
        /// </summary>
        private Session mSession;

        private int mHeartbeatInterval = 5000;

        /// <summary>
        /// 连接成功回调
        /// </summary>
        public Action OnConnectSuccess;

        /// <summary>
        /// 连接失败回调
        /// </summary>
        public Action OnConnectFailure;

        /// <summary>
        /// 连接断开
        /// </summary>
        public Action OnConnectDisConnect;

        /// <summary>
        /// 初始化网络层
        /// </summary>
        public async FTask<Scene> Initlization()
        {
            //原因：我们只使用Fanstay网络层
            await Fantasy.Platform.Unity.Entry.Initialize();
            //Scene客户端场景
            return mScene = await Scene.Create(SceneRuntimeMode.MainThread);
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="romoteAddress"></param>
        /// <param name="networkProtocolType"></param>
        /// <returns></returns>
        /// 这里暴力接口的目的是去适配不同游戏的做法，有些游戏为了保证连接和重连的稳定性，会设置多个服务器地址 如 www.dnf1.com www.dnf2.com www.dnf3.com
        /// 可能和CDN节点有关，会提供一个主域名和多个子域名 主地址不通的时候，可以自动切换到子地址进行连接
        /// 优点1：能够保证连接的稳定性，假设一个不通，还有其他的iP进行连接。
        /// 连接不通的原因：如用户网络问题，开了VPN CDN节点问题。
        /// 通常这些地址会有服务下发，或向服务端拉取。
        public Session Connect(string romoteAddress, NetworkProtocolType networkProtocolType, Action onConnectComplete = null,
            Action onConnectFail = null, Action onConnectDisconnect = null, int connectOutTime = 5000)
        {
            mSession = mScene.Connect(romoteAddress, NetworkProtocolType.KCP, () =>
                {
                    ConnectSuccess();
                    onConnectComplete?.Invoke();
                },
                () =>
                {
                    ConnectFailure();
                    onConnectFail?.Invoke();
                }, () =>
                {
                    ConnectDisConnect();
                    onConnectDisconnect?.Invoke();
                }, false , connectOutTime);
            return mSession;
        }

        public void DisConnect()
        {
            mSession?.Dispose();
        }

        /// <summary>
        /// 发送一条RPC消息到服务端，并等待响应触发
        /// </summary>
        /// <param name="request"></param>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public async FTask<T> Call<T>(IRequest request, long routeId = 0) where T : IResponse
        {
            T res = (T)await mSession.Call(request, routeId);
            return res;
        }

        /// <summary>
        /// 发送消息到服务端，不需要响应
        /// </summary>
        /// <param name="message"></param>
        /// <param name="rpcId"></param>
        /// <param name="routeId"></param>
        public void Send(IMessage message, uint rpcId = 0, long routeId = 0)
        {
            mSession.Send(message, rpcId, routeId);
        }

        /// <summary>
        /// 链接服务器成功
        /// </summary>
        public void ConnectSuccess()
        {
            Debug.Log("链接服务器成功...");
            OnConnectSuccess?.Invoke();
            mSession.AddComponent<SessionHeartbeatComponent>().Start(mHeartbeatInterval);
        }

        /// <summary>
        /// 链接服务器失败
        /// </summary>
        public void ConnectFailure()
        {
            Debug.Log("链接服务器失败...");
            OnConnectFailure?.Invoke();
        }

        /// <summary>
        /// 服务器链接终端 在处于链接状态中，Socket断掉了会触发这个接口
        /// </summary>
        public void ConnectDisConnect()
        {
            Debug.Log("服务器链接中断..");
            OnConnectDisConnect?.Invoke();
        }


        /// <summary>
        /// 释放网络层
        /// </summary>
        public void OnRelease()
        {
            mSession?.Dispose();
            mScene?.Dispose();
            _instance = null;
        }
    }
}