using Buildings;
using UnityEngine;
using Vehicles;

namespace Enemies
{
    public class FlyingEnemy: Enemy
    {
        [SerializeField] internal float _targetAltitude = 5f;
        public ShootingComponent ShootingComponent;

        private void Start()
        {
            ShootingComponent.ShootingAttackPoint = transform;  
        }

        public override void HandleMovement(Transform p_nearestPlayerUnit)
        {
            if (CurrentTarget == null)
            {
                Vector2 direction = (p_nearestPlayerUnit.position - transform.position).normalized;
                MoveTowards(direction);
            }
            else
            {
                PerformManeuversAndAttack();
            }
        }

        public override void HandleSpecialAction()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, ShootingComponent.InteractionRange);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Building") || hitCollider.gameObject.CompareTag("Vehicle"))
                {
                    CurrentTarget = hitCollider.transform;
                    return;
                }
            }

            CurrentTarget = null;
        }

        private void MoveTowards(Vector2 direction)
        {
            Vector2 currentPos = transform.position;
            var targetPos = new Vector2(direction.x, direction.y + _targetAltitude);
            var moveDirection = (targetPos - currentPos).normalized;
            Rigidbody2D.AddForce(moveDirection * EnemyData.Speed, ForceMode2D.Force);
        }
        
        private void MoveOnAttacks()
        {
            float distanceToTarget = Vector2.Distance(transform.position, CurrentTarget.position);
            float acceleration = Mathf.Clamp(distanceToTarget / 10f, 0.5f, 1.5f); 

            Rigidbody2D.AddForce(transform.up * (EnemyData.Speed * acceleration), ForceMode2D.Force);
        }

        private void PerformManeuversAndAttack()
        {
            ShootingComponent.MakeRangeAttack(EnemyData.AttackCooldown, EnemyData.AttackDamage, CurrentTarget);
            MoveOnAttacks();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Building") || collision.gameObject.CompareTag("Vehicle"))
            {
                //Handle kamikaze collision if needed
            }
        }
    }
}
