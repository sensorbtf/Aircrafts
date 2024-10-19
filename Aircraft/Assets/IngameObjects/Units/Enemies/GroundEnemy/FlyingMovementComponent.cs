using UnityEngine;

namespace Enemies
{
    public class FlyingMovementComponent: MonoBehaviour
    {
        public float InteractionRange { get;}
        public bool IsMovement { get; }
        public void PhysicUpdate(Transform p_nearestPlayerUnit, Rigidbody2D p_enemyRb, Transform p_currentTarget, float p_speed)
        {
            if (p_currentTarget == null)
            {
                Vector2 direction = (p_nearestPlayerUnit.position - transform.position).normalized;
                //MoveTowards(direction, p_enemyRb, p_speed);
            }
            else
            {
                //StopMovement(p_enemyRb);
            }
        }


        public void NormalUpdate()
        {
            
        }
    }
}