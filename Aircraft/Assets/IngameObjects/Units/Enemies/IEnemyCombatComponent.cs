using System;
using UnityEngine;

namespace Enemies
{
    public interface IEnemyCombatComponent
    {
        public float AttackCooldown { get;}
        public Transform AttackPoint { get;}
        void AttackUpdate();
    }
}