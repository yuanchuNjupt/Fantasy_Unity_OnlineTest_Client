using System;
using FixedPhysics.Fixed_pointNumber.Core;
using FixedPhysics.FixedCollider.Colliders._3D;
using FixedPhysics.FixedCollider.Colliders.Types;
using FixedPhysics.FixedCollider.Core;

namespace Battle.CustomCollider
{
    /// <summary>
    /// 技能 Box 伤害碰撞体（用于矩形/扇形范围技能）
    /// 自动托管到 PhysicsManager3D，碰撞到 BattlePlayerCollider 时触发一次伤害回调
    /// </summary>
    public class SkillDamageBoxCollider : FixedIntBoxCollider
    {
        /// <summary>
        /// 命中 BattlePlayerCollider 时的伤害回调
        /// </summary>
        public Action<BattlePlayerCollider> OnHitPlayerCallBack;

        public SkillDamageBoxCollider(FixedIntVector3 pos, FixedInt rot, SkillDamageConfig config)
            : base(pos,FixedIntVector3.zero, config.boxSize, rot, FixedIntCollider3DType.OnlyYRotation)
        {
            OnCollisionEnterCallBack += OnCollisionEnter;
            PhysicsManager3D.Instance.AddCollider3D(this);
        }

        private void OnCollisionEnter(FixedIntCollider3D other)
        {
            if (other is not BattlePlayerCollider playerCollider) return;
            OnHitPlayerCallBack?.Invoke(playerCollider);
        }

        public override void OnDestroy()
        {
            OnHitPlayerCallBack = null;
            PhysicsManager3D.Instance.RemoveCollider3D(this);
            base.OnDestroy();
        }
    }
}
