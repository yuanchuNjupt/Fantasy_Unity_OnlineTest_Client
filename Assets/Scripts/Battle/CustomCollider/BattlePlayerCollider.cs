using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders._3D;
using FixedPhysics.FixedCollider.Core;

namespace Battle.CustomCollider
{
    public class BattlePlayerCollider : FixedIntCylinderCollider
    {
        
        public BattlePlayerLogicLayer playerLogicLayer;
        
        private bool _isHostingCollider = false;
        
        public BattlePlayerCollider(FixedInt radius, FixedInt height, FixedIntVector3 position, FixedIntVector3 offset , BattlePlayerLogicLayer logicLayer) : base(radius, height, position, offset)
        {
            playerLogicLayer = logicLayer;
            
        }

        public void HostingCollider()
        {
            if(_isHostingCollider)
                return;
            PhysicsManager3D.Instance.AddCollider3D(this);
            _isHostingCollider = true;
        }
    }
}