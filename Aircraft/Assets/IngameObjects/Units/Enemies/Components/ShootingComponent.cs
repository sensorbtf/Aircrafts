using System.Linq;
using Objects;
using UnityEngine;

namespace Enemies
{
    public class ShootingComponent: MonoBehaviour, IEnemyCombatComponent
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _projectileSpeed = 5f;
        [SerializeField] private bool _isMain;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _detectionRange;
        [SerializeField] private float _attackRange;
        [SerializeField] private Transform _attackPoint;
        
        private float _currentAttackCooldown;
        public bool IsMain => _isMain;
        public float DetectionRange => _detectionRange;
        public float AttackRange => _attackRange;
        public float AttackCooldown => _attackCooldown;
        public Transform AttackPoint => _attackPoint;

        public void AttackUpdate(float p_attackCooldown, int p_attackDamage, Unit p_target, out bool p_isAttacking)
        {
            p_isAttacking = HandleAttackCooldown(p_attackCooldown, p_attackDamage, p_target);
        }

        public Unit TryToDetectUnit()
        {
            var hitColliders = Physics2D.OverlapCircleAll(AttackPoint.position, DetectionRange);
            var validTargets = hitColliders
                .Where(hitCollider => hitCollider != null &&
                                      (hitCollider.gameObject.CompareTag(LayerTagsManager.BuildingTag) ||
                                       hitCollider.gameObject.CompareTag(LayerTagsManager.VehicleTag))).ToList();

            if (!validTargets.Any())
                return null;

            var closestTarget = validTargets
                .OrderBy(hitCollider => Vector2.Distance(AttackPoint.position, hitCollider.transform.position))
                .FirstOrDefault();

            return closestTarget?.gameObject.GetComponent<Unit>();
        }

        private bool HandleAttackCooldown(float p_attackCooldown, int p_attackDamage, Unit p_currentTarget)
        {
            _currentAttackCooldown -= Time.deltaTime;

            var hitColliders = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject != p_currentTarget.gameObject)
                    continue;

                if (_currentAttackCooldown <= 0f && p_currentTarget != null)
                {
                    ShootProjectile(p_currentTarget, p_attackDamage);
                    _currentAttackCooldown = p_attackCooldown;
                }

                return true;
            }

            return false;
        }

        private void ShootProjectile(Unit p_target, int p_attackDamage)
        {
            Vector2 firePosition = AttackPoint.position;
            var position = p_target.gameObject.transform.position;
            var targetPosition = new Vector2(position.x, position.y);
            var direction = (targetPosition - firePosition).normalized;

            var projectile = Instantiate(_projectilePrefab, firePosition, Quaternion.identity);
            var projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectileScript.Initialize(direction * _projectileSpeed, p_attackDamage, true);
            }
        }
    }
}