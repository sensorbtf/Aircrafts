using System;
using Objects;
using UnityEngine;

namespace Enemies
{
    public interface IEnemyMovementComponent
    {
        void PhysicUpdate(Transform p_nearestPlayerUnit, Rigidbody2D p_enemyRb, Unit p_currentTarget, float p_speed);
    }
}