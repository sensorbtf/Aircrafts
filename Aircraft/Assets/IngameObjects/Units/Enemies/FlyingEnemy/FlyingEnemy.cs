using Buildings;
using UnityEngine;
using Vehicles;

namespace Enemies
{
    public class FlyingEnemy: Enemy
    {
        internal Transform _currentTarget;
        private float _attackCooldown;

        [SerializeField] internal GameObject _projectilePrefab;
        [SerializeField] internal float _shootingRange = 4f;
        [SerializeField] private float _maneuverSpeed = 5f;  
        [SerializeField] internal float _targetAltitude = 5f; 

        public override void HandleMovement(Transform p_playerBase)
        {
            if (_currentTarget == null)
            {
                Vector2 direction = (p_playerBase.position - transform.position).normalized;
                MoveTowards(direction);
            }
            else
            {
                MoveOnAttacks();
                PerformManeuversAndAttack();
            }
        }

        public override void HandleSpecialAction()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _shootingRange);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Building") || hitCollider.gameObject.CompareTag("Vehicle"))
                {
                    _currentTarget = hitCollider.transform;
                    return;
                }
            }

            _currentTarget = null;
        }

        private void MoveTowards(Vector2 p_direction)
        {
            Vector2 currentPos = transform.position;
            var targetPos = new Vector2(p_direction.x, p_direction.y + _targetAltitude);
            var direction = (targetPos - currentPos).normalized;
            Rigidbody2D.AddForce(direction * EnemyData.Speed, ForceMode2D.Force);
        }

        private void PerformManeuversAndAttack()
        {
            if (_currentTarget == null)
            {
                MoveOnAttacks();
                return;
            }

            HandleAttackCooldown();
        }

        private void MoveOnAttacks()
        {
            float distanceToTarget = Vector2.Distance(transform.position, _currentTarget.position);
            float acceleration = Mathf.Clamp(distanceToTarget / 10f, 0.5f, 1.5f); 

            Rigidbody2D.AddForce(transform.up * (EnemyData.Speed * acceleration), ForceMode2D.Force);
        }

        private void HandleAttackCooldown()
        {
            _attackCooldown -= Time.deltaTime;

            if (_attackCooldown <= 0f && _currentTarget != null)
            {
                ShootProjectile(_currentTarget);
                _attackCooldown = EnemyData.AttackCooldown;
            }
        }

        private void ShootProjectile(Transform p_target)
        {
            Vector2 firePosition = AttackPoint.position; 
            var targetPosition = new Vector2(p_target.position.x, p_target.position.y);
            var direction = (targetPosition - firePosition).normalized;
            var projectile = Instantiate(_projectilePrefab, firePosition, Quaternion.identity);

            var projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Initialize(direction * 5f, UnitData.AttackDamage, true); 
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) // kamikadze
        {
            if (collision.gameObject.CompareTag("Building") || collision.gameObject.CompareTag("Vehicle"))
            {
                //Destroy(gameObject); 
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _shootingRange);
        }
    }
}
