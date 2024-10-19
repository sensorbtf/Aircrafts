using Objects;
using UnityEngine;

namespace Enemies
{
    public class ShootingComponent : MonoBehaviour, IEnemyCombatComponent
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _projectileSpeed = 5f;

        private float _currentAttackCooldown;
        public bool IsMain { get; }
        public float DetectionRange { get; }
        public float AttackRange { get; }
        public float AttackCooldown { get; }
        public Transform AttackPoint { get; }

        public void AttackUpdate(float p_attackCooldown, int p_attackDamage, Unit p_target, out bool p_isAttacking)
        {
            p_isAttacking = HandleAttackCooldown(p_attackCooldown, p_attackDamage, p_target);
        }

        public Unit TryToDetectUnit()
        {
            var hitCollider = Physics2D.OverlapCircle(AttackPoint.position, DetectionRange);

            if (hitCollider != null && (hitCollider.gameObject.CompareTag(LayerTagsManager.BuildingTag) ||
                                        hitCollider.gameObject.CompareTag(LayerTagsManager.VehicleTag)))
            {
                return hitCollider.gameObject.GetComponent<Unit>();
            }

            return null;
        }

        private bool HandleAttackCooldown(float p_attackCooldown, int p_attackDamage, Unit p_currentTarget)
        {
            _currentAttackCooldown -= Time.deltaTime;

            var hitCollider = Physics2D.OverlapCircle(AttackPoint.position, AttackRange);

            if (hitCollider != null && (hitCollider.gameObject.CompareTag(LayerTagsManager.BuildingTag) ||
                                        hitCollider.gameObject.CompareTag(LayerTagsManager.VehicleTag)))
            {
                if (_currentAttackCooldown <= 0f && p_currentTarget != null)
                {
                    ShootProjectile(p_currentTarget, p_attackDamage);
                    _currentAttackCooldown = p_attackCooldown;
                    
                    return true;
                }
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