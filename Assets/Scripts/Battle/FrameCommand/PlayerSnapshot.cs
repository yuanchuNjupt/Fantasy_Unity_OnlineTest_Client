using System;
using Fantasy.Pool;
using FixedPhysics.Fixed_pointNumber.Core;

namespace Battle.FrameCommand
{
    /// <summary>
    /// 玩家状态快照数据
    /// </summary>
    public class PlayerSnapshot : IPool , IDisposable
    {
        public long uid;
        
        public FixedIntVector3 LogicPos;
        public FixedIntVector3 LogicForwardDir;

        public FixedInt Hp;

        public LogicObjectActionState ActionState;
        
        
        
        
        
        
        public void Dispose()
        {
            uid = 0;
            LogicPos = new FixedIntVector3(0, 0, 0);
            LogicForwardDir = new FixedIntVector3(0, 0, 1);
            Hp = 0;
            ActionState = LogicObjectActionState.Idle;
            
            
            Pool<PlayerSnapshot>.Return(this);
            
        }

        public static PlayerSnapshot Create(long uid , LogicActor actor)
        {
            var snapshot = Pool<PlayerSnapshot>.Rent();
            snapshot.uid = uid;
            snapshot.LogicPos = actor.LogicPos;
            snapshot.LogicForwardDir = actor.LogicForwardDir;
            snapshot.Hp = actor.HP;
            snapshot.ActionState = actor.ActionState;
            return snapshot;
        }

        public override string ToString()
        {
            return $"UID:{uid} Pos:{LogicPos} ForwardDir:{LogicForwardDir} Hp:{Hp}";
        }


        #region Pool

        private bool _isPool;
        
        public bool IsPool()
        {
            return _isPool;
        }

        public void SetIsPool(bool isPool)
        {
            _isPool = isPool;
        }

        #endregion
 

    }
}