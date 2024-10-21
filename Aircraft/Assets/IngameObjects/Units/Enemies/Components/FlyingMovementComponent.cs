using System.Linq;
using UnityEngine;
using Objects;

namespace Enemies
{
    public class FlyingMovementComponent: MonoBehaviour, IEnemyMovementComponent
    {
        [SerializeField] internal float _targetAltitude = 5f;
        [SerializeField] internal bool _performManevuers;

        public void PhysicUpdate(Transform p_nearestPlayerUnit, Rigidbody2D p_enemyRb, Unit p_currentTarget,
            float p_speed, bool p_isAttacking)
        {
            if (p_isAttacking)
            {
                if (_performManevuers)
                {
                    if (p_currentTarget != null)
                    {
                        MoveWhileAttacking(p_enemyRb, p_currentTarget, p_speed);
                    }
                    else
                    {
                        Vector2 direction = p_currentTarget != null ? (p_currentTarget.gameObject.transform
                            .position - transform.position).normalized : (p_nearestPlayerUnit.position 
                                                                          - transform.position).normalized;
                
                        MoveTowards(direction, p_enemyRb, p_speed);
                    }
                }
                else
                {
                    StopMovement(p_enemyRb);
                }
            }
            else
            {
                Vector2 direction = p_currentTarget != null ? (p_currentTarget.gameObject.transform
                    .position - transform.position).normalized : (p_nearestPlayerUnit.position 
                                                                  - transform.position).normalized;
                
                MoveTowards(direction, p_enemyRb, p_speed);
            }
        }

        private void MoveWhileAttacking(Rigidbody2D p_enemyRb, Unit p_currentTarget, float p_speed)
        {
            float distanceToTarget = Vector2.Distance(transform.position, p_currentTarget.gameObject.transform.position);
            float acceleration = Mathf.Clamp(distanceToTarget / 10f, 0.5f, 1.5f); 
        
            p_enemyRb.AddForce(transform.up * (p_speed * acceleration), ForceMode2D.Force);
        }

        private void MoveTowards(Vector2 p_direction, Rigidbody2D p_enemyRb, float p_speed)
        {
            Vector2 currentPos = transform.position;
            var targetPos = new Vector2(p_direction.x, p_direction.y + _targetAltitude);
            var moveDirection = (targetPos - currentPos).normalized;
            p_enemyRb.AddForce(moveDirection * p_speed, ForceMode2D.Force);
        }

        private void StopMovement(Rigidbody2D p_enemyRb)
        {
            p_enemyRb.velocity = Vector2.zero;
        }
    }
}