using System;
using Objects;
using UnityEngine;

namespace Enemies
{
    public interface IEnemyCombatComponent
    {
        public bool IsMain { get;}
        public float DetectionRange { get;}
        public float AttackRange { get;}
        public float AttackCooldown { get;}
        public Transform AttackPoint { get;}
        void AttackUpdate(float p_attackCooldown, int p_attackDamage, Unit p_target);
        Unit TryToDetectUnit();
    }
}