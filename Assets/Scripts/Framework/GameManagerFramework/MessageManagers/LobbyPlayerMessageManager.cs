using Config;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using Generate;
using Lobby;
using UnityEngine;

namespace Framework.MessageManagers
{
    [WorldSource(typeof(LobbyWorld))]
    public class LobbyPlayerMessageManager : IMessageBehaviour
    {
        
        
        public void OnCreate()
        {
        }

        public void OnDestroy()
        {
            
        }
        
        
        public async FTask<EntryLobbyResponse> SendEntryLobbyRequest(long accountId)
        {
            var EntryLobbyReq = new EntryLobbyRequest();
            EntryLobbyReq.accountId = accountId;
            var EntryLobbyRes = await NetWorkManager.Instance.Call<EntryLobbyResponse>(EntryLobbyReq);
            return EntryLobbyRes;
        }

        /// <summary>
        /// 同步自己的位置给服务器
        /// </summary>
        /// <param name="syncData"></param>
        /// <returns></returns>
        public async FTask<StateSyncResponse> SendStateSyncRequest(stateSyncData syncData)
        {
            StateSyncRequest req = new StateSyncRequest();

            req.stateData = syncData;
            syncData.playerId = World.GetExitsDataManager<UserDataManager>().UserData.AccountId;
            
            return await NetWorkManager.Instance.Call<StateSyncResponse>(req);
        }
        
        
        //当其他玩家登录时
        public class OtherPlayerLoginMessageHandler : Message<OtherPlayerLoginMessage>
        {
            protected override async FTask Run(Session session, OtherPlayerLoginMessage message)
            {
                //生成对应的玩家并缓存
                Debug.Log("收到其他玩家登录消息 玩家ID：" + message.playerData.playerId);
                GameObject go = Resources.Load<GameObject>(ModelInfoConfig.LobbyModelName);
                GameObject player = Object.Instantiate(go);
                player.GetComponent<LobbyPlayerName>().Init(message.playerData.PlayerName , CameraInit.MainInstance.PlayerCamera.gameObject.transform);
                player.transform.position = Vector3.zero;
                LobbyPlayer playerScript = player.AddComponent<LobbyPlayer>();
                playerScript.Init(message.playerData.PlayerName, PlayerType.Other);
                playerScript.InitPos(message.playerData.position , message.playerData.inputDir);

                //缓存其他玩家
                World.GetExitsDataManager<LobbyPlayerDataManager>().AddOtherPlayer(message.playerData.playerId, playerScript);
                await FTask.CompletedTask;
            }
        }

        //当其他玩家下线时
        public class OtherPlayerLogoutMessageHandler : Message<OtherPlayerLogoutMessage>
        {
            protected override async FTask Run(Session session, OtherPlayerLogoutMessage message)
            {
                //移除对应的玩家
                var otherPlayer = World.GetExitsDataManager<LobbyPlayerDataManager>().GetOtherPlayer(message.playerId);
                
                if (otherPlayer == null)
                {
                    Debug.Log("不存在该玩家，无法移除 玩家ID：" + message.playerId);
                    return;
                }
                
                GameObject go = otherPlayer.gameObject;
                Object.Destroy(go);
                World.GetExitsDataManager<LobbyPlayerDataManager>().RemoveOtherPlayer(message.playerId);
                
                Debug.Log("玩家下线 玩家ID：" + message.playerId);
                await FTask.CompletedTask;
            }
        }
        
        //同步其他玩家状态
        public class OtherPlayerStateSyncMessageHandler : Message<OtherPlayerStateSyncMessage>
        {
            protected override async FTask Run(Session session, OtherPlayerStateSyncMessage message)
            {
                var otherPlayer = World.GetExitsDataManager<LobbyPlayerDataManager>().GetOtherPlayer(message.roleData.playerId);
                if (otherPlayer == null)
                {
                    Debug.LogError("不存在该玩家，无法同步位置 玩家ID：" + message.roleData.playerId);
                    return;
                }
                
                otherPlayer.SyncPos(message.roleData.position , message.roleData.inputDir);
                await FTask.CompletedTask;
            }
        }
        
        
    }
    
    
}