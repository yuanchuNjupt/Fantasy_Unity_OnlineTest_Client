using System;
using System.Collections.Generic;
using Fantasy.Pool;
using Framework.GameManagerFramework.LogicManagers;

namespace Battle.FrameCommand
{
    public class WorldSnapshot : IPool , IDisposable
    {

        private readonly SortedDictionary<long, PlayerSnapshot> _playerSnapshots = new SortedDictionary<long, PlayerSnapshot>();





        public static WorldSnapshot Create()
        {
            return Pool<WorldSnapshot>.Rent();
        }
        
        public void Capture(BattlePlayerLogicManager playerLogicManager)
        {
            foreach (var (uid , instance) in playerLogicManager.GetAllBattlePlayers())
            {
                _playerSnapshots.Add(uid, PlayerSnapshot.Create(uid , instance.logicLayer));
            }
        }

        public void Restore(BattlePlayerLogicManager playerLogicManager)
        {
            foreach (var (uid , snapshot) in _playerSnapshots)
            {
                playerLogicManager.GetBattlePlayerInstance(uid).logicLayer.Restore(snapshot);
            }
        }
        
        
        
        private bool _isPool;
        
        public bool IsPool()
        {
            return _isPool;
        }

        public void SetIsPool(bool isPool)
        {
            _isPool = isPool;
        }

        public void Dispose()
        {
            foreach (var (_, snapshot) in _playerSnapshots)
            {
                snapshot.Dispose();
            }
            _playerSnapshots.Clear();
            Pool<WorldSnapshot>.Return(this);
        }
    }
}