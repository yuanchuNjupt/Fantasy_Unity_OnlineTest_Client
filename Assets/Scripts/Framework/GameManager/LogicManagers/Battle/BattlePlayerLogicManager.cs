﻿using System.Collections.Generic;
using System.Linq;
using Battle;
using Config;
using Fantasy;
using Framework.GameManager.Base;
using Framework.GameManager.Core;
using Framework.GameManagerFramework.DataManagers;
using Framework.GameManagerFramework.WorldScripts;
using UIFramework.Core;
using UIFramework.ViewPath;
using UnityEngine;

namespace Framework.GameManagerFramework.LogicManagers
{
    [WorldSource(typeof(BattleWorld))]
    public class BattlePlayerLogicManager : ILogicBehaviour
    {
        [Inject]private BattleDataManager _battleDataManager;
        [Inject]private UserDataManager _userDataManager;


        private readonly Dictionary<long , BattlePlayerInstance> _battlePlayerList = new ();
        
        

        public void OnCreate()
        {
            // battleDataManager = GameManager.Core.World.GetExitsDataManager<BattleDataManager>();
            Debug.Log("BattlePlayerLogicManager 创建完成");
        }

        public void InitPlayer()
        {
            //一定要先创建自己并绑定摄像机，再创建其他玩家，否则会出现摄像机绑定错误的问题
            var selfInstance = _battleDataManager.BattlePlayerDataList.First(x => x.playerId == _userDataManager.UserData.AccountId);
            BattlePlayerInstance battlePlayer = new BattlePlayerInstance(selfInstance.playerId, selfInstance.playerName);
            _battlePlayerList.Add(selfInstance.playerId , battlePlayer);
            foreach (BattlePlayerData playerData in _battleDataManager.BattlePlayerDataList)
            {
                if(playerData.playerId == selfInstance.playerId)
                    continue;
                
                BattlePlayerInstance battleOtherPlayer = new BattlePlayerInstance(playerData.playerId, playerData.playerName);
                
                _battlePlayerList.Add(playerData.playerId , battleOtherPlayer);
                var presenter = UIManager.MainInstance.GetPanel<BattleMainPanelView>().GetComponent<BattleMainPanelPresenter>();
                presenter.AddEnemyHpRectView(playerData.playerId, playerData.playerName);
            }
        }

        public BattlePlayerInstance GetBattlePlayerInstance(long playerId)
        {
            if (_battlePlayerList.TryGetValue(playerId, out var battlePlayer))
            {
                return battlePlayer;
            }
            else
            {
                Debug.LogError("未找到玩家实例，玩家ID：" + playerId);
                return null;
            }
        }

        /// <summary>
        /// 获取所有战斗玩家实例（只读遍历）
        /// </summary>
        public IReadOnlyDictionary<long, BattlePlayerInstance> GetAllBattlePlayers()
        {
            return _battlePlayerList;
        }
        

        public void OnLogicFrameUpdate()
        {
            //每帧更新角色逻辑层
            foreach (var battlePlayerInstance in _battlePlayerList.Values)
            {
                battlePlayerInstance.logicLayer.OnLogicFrameUpdate();
            }
        }


        public void OnDestroy()
        {
        }
    }
}