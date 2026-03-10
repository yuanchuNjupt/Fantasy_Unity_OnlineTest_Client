﻿using System.Collections.Generic;
using System.Linq;
using Battle;
using Config;
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


        private readonly Dictionary<long , BattlePlayerInstance> _battlePlayerList = new ();
        
        

        public void OnCreate()
        {
            // _battleDataManager = GameManager.Core.World.GetExitsDataManager<BattleDataManager>();
            Debug.Log("BattlePlayerLogicManager 创建完成");
        }

        public void InitPlayer()
        {
            _battleDataManager.BattlePlayerDataList.ForEach(player =>
            {
                BattlePlayerInstance battlePlayer = new BattlePlayerInstance(player.playerId, player.playerName);
                _battlePlayerList.Add(player.playerId , battlePlayer);
            });
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