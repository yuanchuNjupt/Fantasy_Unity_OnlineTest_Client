using System.Collections.Generic;
using Config;
using Fantasy;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using Framework.MessageManagers;
using Lobby;
using UIFramework.Core;
using UIFramework.ViewPath;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Scene = UnityEngine.SceneManagement.Scene;

namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(LobbyWorld))]
    public class LobbyPlayerLogicManager : ILogicBehaviour
    {
        private LobbyPlayerDataManager _lobbyPlayerDataManager;
        private LobbyPlayerMessageManager _lobbyPlayerMessageManager;
        private UserDataManager _userDataManager;
        
        public void OnCreate()
        {
            _lobbyPlayerDataManager = World.GetExitsDataManager<LobbyPlayerDataManager>();
            _lobbyPlayerMessageManager = World.GetExitsMessageManager<LobbyPlayerMessageManager>();
            _userDataManager = World.GetExitsDataManager<UserDataManager>();
        }

        public void OnDestroy()
        {
        }
        
        
        /// <summary>
        /// 进入大厅
        /// </summary>
        public async void EntryLobby()
        {
            //跳转场景
            var entryLobbyRes = await _lobbyPlayerMessageManager.SendEntryLobbyRequest(_userDataManager.UserData.AccountId);
            //场景跳转
            UIManager.MainInstance.HideAllLayersPanels();
            SceneManager.sceneLoaded += OnLobbySceneLoaded;
            SceneManager.LoadScene("Lobby");
        
            
            return;
            
            // 定义场景加载回调方法
            void OnLobbySceneLoaded(Scene scene, LoadSceneMode mode) {
                SceneManager.sceneLoaded -= OnLobbySceneLoaded;
                OnLocalEntryLobby(entryLobbyRes.selfData , entryLobbyRes.otherPlayerData);
                UIManager.MainInstance.ShowPanel<LobbyPlayerPanelView>();
            }
        }

        private void OnLocalEntryLobby(StateSyncData selfData , List<StateSyncData> resOtherPlayerData)
        {
            
            //实例化角色
            GameObject go = Resources.Load<GameObject>(ModelInfoConfig.LobbyModelName);
            GameObject player = Object.Instantiate(go);
            
            _lobbyPlayerDataManager.SetSelfPlayer(player.AddComponent<LobbyPlayer>());
            _lobbyPlayerDataManager.SelfPlayer.InitPos(selfData.position , selfData.inputDir);
            _lobbyPlayerDataManager.SelfPlayer.Init(_userDataManager.UserData.UserName ,PlayerType.Self);
            
            //初始化相机
            CameraInit.MainInstance.InitPlayerCamera(player.transform);
            
            
            var nameControl = player.GetComponent<LobbyPlayerName>();
            nameControl.Init(_lobbyPlayerDataManager.SelfPlayer.PlayerName , CameraInit.MainInstance.PlayerCamera.gameObject.transform);
            


            LobbyPlayer otherPlayer;
            
            Debug.Log("需要同步的其他玩家数量：" + resOtherPlayerData.Count);
            foreach (var syncData in resOtherPlayerData)
            {
                Debug.Log("其他玩家ID：" + syncData.playerId);
            }
            
            foreach (var playerData in resOtherPlayerData)
            {
                player = Object.Instantiate(go);
                player.GetComponent<LobbyPlayerName>().Init(playerData.PlayerName , CameraInit.MainInstance.PlayerCamera.gameObject.transform);
                //同步其他玩家位置和方向
                otherPlayer = player.AddComponent<LobbyPlayer>();
                otherPlayer.Init(playerData.PlayerName , PlayerType.Other);
                otherPlayer.InitPos(playerData.position , playerData.inputDir);
                _lobbyPlayerDataManager.AddOtherPlayer(playerData.playerId, otherPlayer);
            }
        }
        
        
        //同步自己的位置到服务器
        public async void SyncRoleState(StateSyncData syncData)
        {

            var res = await _lobbyPlayerMessageManager.SendStateSyncRequest(syncData);
            //处理返回结果
            if (res.ErrorCode != 0)
            {
                Debug.Log("同步状态失败 错误码：" + res.ErrorCode);
                return;
            }
            _lobbyPlayerDataManager.SelfPlayer.SyncPos(res.stateData.position , res.stateData.inputDir , (PlayerState)res.stateData.playerState);
            
        }
        
    }
}