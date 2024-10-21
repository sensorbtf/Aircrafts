using System.Linq;
using Objects;
using UnityEngine;

namespace Enemies
{
    public class MeleeCombatComponent: MonoBehaviour, IEnemyCombatComponent
    {
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
                    p_currentTarget.GetComponent<Unit>().ReceiveDamage(p_attackDamage);
                    _currentAttackCooldown = p_attackCooldown;
                }

                return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var position = AttackPoint.position;
            Gizmos.DrawWireSphere(position, DetectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, AttackRange);
        }
    }
}