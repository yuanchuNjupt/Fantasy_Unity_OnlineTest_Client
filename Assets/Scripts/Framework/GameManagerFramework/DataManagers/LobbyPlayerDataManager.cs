using System.Collections.Generic;
using Framework.GameManagerFramework.WorldScripts;

namespace Framework.GameManagerFramework.DataManagers
{
    [WorldSource(typeof(LobbyWorld))]
    public class LobbyPlayerDataManager : IDataBehaviour
    {
        //大厅所有玩家的数据
        public Dictionary<long, LobbyPlayer> OtherPlayers { get; private set; }
        
        public LobbyPlayer SelfPlayer { get; private set; }
        
        
        
        
        public void OnCreate()
        {
            OtherPlayers = new Dictionary<long, LobbyPlayer>();
        }
        
        public void SetSelfPlayer(LobbyPlayer player)
        {
            SelfPlayer = player;
        }

        public void AddOtherPlayer(long playerId, LobbyPlayer player)
        {
            if (!OtherPlayers.TryAdd(playerId, player))
            {
                UnityEngine.Debug.LogWarning("玩家已存在，无法添加，PlayerId:" + playerId);
                return;
            }
        }
        
        public LobbyPlayer GetOtherPlayer(long playerId)
        {
            return OtherPlayers.GetValueOrDefault(playerId);
        }

        public void RemoveOtherPlayer(long playerId)
        {
            if (!OtherPlayers.Remove(playerId))
            {
                UnityEngine.Debug.LogWarning("玩家不存在，无法移除，PlayerId:" + playerId);
            }
        }
        
        
        

        public void OnDestroy()
        {
            
        }
    }
}