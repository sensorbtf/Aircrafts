using Objects;
using UnityEngine;

namespace Enemies
{
    public class MeleeCombatComponent : MonoBehaviour, IEnemyCombatComponent
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
                    p_currentTarget.GetComponent<Unit>().ReceiveDamage(p_attackDamage);
                    _currentAttackCooldown = p_attackCooldown;
                }

                return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            if (AttackPoint != null)
            {
                Gizmos.color = Color.yellow;
                var position = AttackPoint.position;
                Gizmos.DrawWireSphere(position, DetectionRange);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(position, AttackRange);
            }
        }
    }
}