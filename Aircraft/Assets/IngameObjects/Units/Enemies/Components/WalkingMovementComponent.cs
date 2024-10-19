using Objects;
using UnityEngine;

namespace Enemies
{
    public class WalkingMovementComponent: MonoBehaviour, IEnemyMovementComponent
    {
        public void PhysicUpdate(Transform p_nearestPlayerUnit, Rigidbody2D p_enemyRb, Unit p_currentTarget, float p_speed)
        {
            if (p_currentTarget == null)
            {
                Vector2 direction = (p_nearestPlayerUnit.position - transform.position).normalized;
                MoveTowards(direction, p_enemyRb, p_speed);
            }
            else
            {
                StopMovement(p_enemyRb);
            }
        }

        private void MoveTowards(Vector2 p_direction, Rigidbody2D p_enemyRb, float p_speed)
        {
            p_enemyRb.AddForce(p_direction * p_speed, ForceMode2D.Force);
        }

        private void StopMovement(Rigidbody2D p_enemyRb)
        {
            p_enemyRb.velocity = Vector2.zero;
        }
        
        public void NormalUpdate()
        {
            
        }
    }
}