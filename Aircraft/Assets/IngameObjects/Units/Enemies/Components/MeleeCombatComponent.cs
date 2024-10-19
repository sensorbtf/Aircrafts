using Objects;
using UnityEngine;

namespace Enemies
{
    public class MeleeCombatComponent : MonoBehaviour, IEnemyCombatComponent
    {
        private float _currentAttackCooldown;
        public bool IsMain { get; }
        public float DetectionRange { get; }
        public float AttackRange { get; }
        public float AttackCooldown { get; }
        public Transform AttackPoint { get; }

        public void AttackUpdate(float p_attackCooldown, int p_attackDamage, Unit p_target)
        {
            HandleAttackCooldown(p_attackCooldown, p_attackDamage, p_target);
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

        private void HandleAttackCooldown(float p_attackCooldown, int p_attackDamage, Unit p_currentTarget)
        {
            _currentAttackCooldown -= Time.deltaTime;

            if (_currentAttackCooldown <= 0f && p_currentTarget != null)
            {
                var hitCollider = Physics2D.OverlapCircle(AttackPoint.position, AttackRange);

                if (hitCollider != null && (hitCollider.gameObject.CompareTag(LayerTagsManager.BuildingTag) ||
                                            hitCollider.gameObject.CompareTag(LayerTagsManager.VehicleTag)))
                {
                    p_currentTarget.GetComponent<Unit>().ReceiveDamage(p_attackDamage);
                    _currentAttackCooldown = p_attackCooldown;
                }
            }
        }
    }
}