using Buildings;
using UnityEngine;
using Vehicles;

namespace Enemies
{
    public class GroundEnemy: Enemy
    {
        private Transform _currentTarget;
        private float _attackCooldown;

        public override void HandleMovement(Transform p_playerBase)
        {
            if (_currentTarget == null)
            {
                Vector2 direction = (p_playerBase.position - transform.position).normalized;
                MoveTowards(direction);
            }
            else
            {
                StopMovement();
                HandleAttackCooldown();
            }
        }

        public override void HandleSpecialAction()
        {
            var hitCollider = Physics2D.OverlapCircle(AttackPoint.position, 0.1f);
            if (hitCollider != null && (hitCollider.gameObject.CompareTag("Building") ||
                                        hitCollider.gameObject.CompareTag("Vehicle")))
            {
                _currentTarget = hitCollider.transform;
            }
            else
            {
                _currentTarget = null;
            }
        }

        private void MoveTowards(Vector2 direction)
        {
            Rigidbody2D.AddForce(direction * EnemyData.Speed, ForceMode2D.Force);
        }

        private void StopMovement()
        {
            // Stop the enemy's movement by setting the velocity to zero
            Rigidbody2D.velocity = Vector2.zero;
        }

        private void HandleAttackCooldown()
        {
            _attackCooldown -= Time.deltaTime;

            if (_attackCooldown <= 0f && _currentTarget != null)
            {
                AttackTarget(_currentTarget.gameObject);
                _attackCooldown = EnemyData.AttackCooldown;
            }
        }
    }
}