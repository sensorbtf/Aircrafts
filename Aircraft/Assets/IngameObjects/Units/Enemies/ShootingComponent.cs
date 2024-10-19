using UnityEngine;

namespace Enemies
{
    public class ShootingComponent: MonoBehaviour, IEnemyCombatComponent
    {
        [SerializeField] internal GameObject _projectilePrefab;
        [SerializeField] internal float _interactionRange = 4f;
        [SerializeField] internal bool _isMovement;
        private float _attackCooldown;

        public Transform ShootingAttackPoint { get; set; }  
        public float InteractionRange => _interactionRange;

        public bool IsMovement => _isMovement;

        public float AttackCooldown { get; }
        public Transform AttackPoint { get; }

        public void AttackUpdate()
        {
            
        }

        public void MakeRangeAttack(float attackCooldown, int attackDamage, Transform p_currentTarget)
        {
            if (p_currentTarget == null)
                return;

            HandleAttackCooldown(attackCooldown, attackDamage, p_currentTarget);
        }

        private void HandleAttackCooldown(float attackCooldown, int attackDamage, Transform p_currentTarget)
        {
            _attackCooldown -= Time.deltaTime;

            if (_attackCooldown <= 0f && p_currentTarget != null)
            {
                ShootProjectile(p_currentTarget, attackDamage);
                _attackCooldown = attackCooldown;
            }
        }

        private void ShootProjectile(Transform target, int attackDamage)
        {
            Vector2 firePosition = ShootingAttackPoint.position;
            Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
            Vector2 direction = (targetPosition - firePosition).normalized;

            var projectile = Instantiate(_projectilePrefab, firePosition, Quaternion.identity);
            var projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectileScript.Initialize(direction * 5f, attackDamage, true);
            }
        }
    }
}