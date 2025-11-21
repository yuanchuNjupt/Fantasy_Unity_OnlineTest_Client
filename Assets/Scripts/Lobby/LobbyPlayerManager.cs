using System.Collections.Generic;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Generate;
using GGG.Tool.Singleton;
using Helper;
using UnityEngine;

namespace Lobby
{
    public class LobbyPlayerManager : Singleton<LobbyPlayerManager>
    {
        public Dictionary<long, LobbyPlayer> otherPlayers = new();
        
        public LobbyPlayer selfPlayer;

        //本机登陆
        public async void OnLocalEntryLobby(PlayerData selfData , List<PlayerData> resOtherPlayerData)
        {
            
            
            // //向服务器发送登录请求
            // // var res = await Init.MainInstance._session.LoginRequest();
            // LoginRequest req = new LoginRequest();
            // var res = await NetWorkManager.Instance.Call<LoginResponse>(req);
            // if (res.ErrorCode == 0)
            // {
            //     Main.MainInstance.UserData.AccountId = res.playerId;
            //     Debug.Log("登录成功 玩家ID：" + res.playerId);
            // }
            // else
            // {
            //     Debug.LogError("登录失败 错误码：" + res.ErrorCode);
            //     return;
            // }
            
            
            //实例化角色
            GameObject go = Resources.Load<GameObject>("PlayerModel");
            GameObject player = Instantiate(go);
            // player.transform.position = Vector3.zero;
            selfPlayer = player.AddComponent<LobbyPlayer>();
            selfPlayer.InitPos(selfData.position , selfData.renderDir);
            selfPlayer.Init(Main.MainInstance.UserData.AccountId , PlayerType.Self);
            
            //初始化相机
            CameraInit.MainInstance.InitPlayerCamera(player.transform);
            

            //实例化其他玩家
            // var resOtherPlayerData = res.otherPlayerData;
            LobbyPlayer otherPlayer;
            
            Debug.Log("需要同步的其他玩家数量：" + resOtherPlayerData.Count);
            foreach (var VARIABLE in resOtherPlayerData)
            {
                Debug.Log("其他玩家ID：" + VARIABLE.playerId);
            }
            
            foreach (var playerData in resOtherPlayerData)
            {
                player = Instantiate(go);
                //同步其他玩家位置和方向
                otherPlayer = player.AddComponent<LobbyPlayer>();
                otherPlayer.Init(playerData.playerId , PlayerType.Other);
                otherPlayer.InitPos(playerData.position , playerData.renderDir);
                otherPlayers.Add(playerData.playerId, otherPlayer);
            }
        }

        //本机登出
        public void OnLocalExitLobby()
        {
            LogoutMessage message = new LogoutMessage();
            message.playerId = Main.MainInstance.UserData.AccountId;
            
            NetWorkManager.Instance.Send(message);
            
            // Init.MainInstance._session.Send(message);
            //移除自己和其他所有玩家
            
            Destroy(selfPlayer.gameObject);
            
            //还原相机配置
            CameraInit.MainInstance.DeInitPlayerCamera();

            foreach (var otherPlayer in otherPlayers.Values)
            {
                Destroy(otherPlayer.gameObject);
            }
            
            //清空其他玩家列表
            otherPlayers.Clear();
        }
        
        //同步自己的位置到服务器
        public async void SyncRoleState(stateSyncData syncData)
        {
            StateSyncRequest req = new StateSyncRequest();

            req.stateData = syncData;
            syncData.playerId = Main.MainInstance.UserData.AccountId;
            
            // var res = await Init.MainInstance._session.Call(req) as StateSyncResponse;
            var res = await NetWorkManager.Instance.Call<StateSyncResponse>(req);
            
            //处理返回结果
            if (res.ErrorCode != 0)
            {
                Debug.Log("同步状态失败 错误码：" + res.ErrorCode);
                return;
            }
            
            selfPlayer.SyncPos(res.stateData.position , res.stateData.inputDir);
            
        }
        
        //获取某个玩家
        public LobbyPlayer GetPlayer(long playerId)
        {
            otherPlayers.TryGetValue(playerId, out LobbyPlayer player);
            if (player != null) return player;
            Debug.Log("不存在该玩家 玩家ID：" + playerId);
            return null;
        }

    }

    //当其他玩家登录时
    public class OtherPlayerLoginMessageHandler : Message<OtherPlayerLoginMessage>
    {
        protected override async FTask Run(Session session, OtherPlayerLoginMessage message)
        {
            //生成对应的玩家并缓存
            Debug.Log("收到其他玩家登录消息 玩家ID：" + message.playerId);
            GameObject go = Resources.Load<GameObject>("PlayerModel");
            GameObject player = GameObject.Instantiate(go);
            player.transform.position = Vector3.zero;
            LobbyPlayer playerScript = player.AddComponent<LobbyPlayer>();
            playerScript.Init(message.playerId , PlayerType.Other);

            //缓存其他玩家
            LobbyPlayerManager.MainInstance.otherPlayers.Add(message.playerId, playerScript);

            await FTask.CompletedTask;
        }
    }

    //当其他玩家下线时
    public class OtherPlayerLogoutMessageHandler : Message<OtherPlayerLogoutMessage>
    {
        protected override async FTask Run(Session session, OtherPlayerLogoutMessage message)
        {
            //移除对应的玩家
            var otherPlayers = LobbyPlayerManager.MainInstance.otherPlayers;
            
            if (!otherPlayers.ContainsKey(message.playerId))
            {
                Debug.Log("不存在该玩家，无法移除 玩家ID：" + message.playerId);
                return;
            }
            
            GameObject go = otherPlayers[message.playerId].gameObject;
            GameObject.Destroy(go);
            otherPlayers.Remove(message.playerId);
            
            Debug.Log("玩家下线 玩家ID：" + message.playerId);
            
            await FTask.CompletedTask;
        }
    }
    
    //同步其他玩家状态
    public class OtherPlayerStateSyncMessageHandler : Message<OtherPlayerStateSyncMessage>
    {
        protected override async FTask Run(Session session, OtherPlayerStateSyncMessage message)
        {
            var otherPlayer = LobbyPlayerManager.MainInstance.GetPlayer(message.roleData.playerId);
            
            otherPlayer.SyncPos(message.roleData.position , message.roleData.inputDir);

            
            await FTask.CompletedTask;
        }
    }

}







