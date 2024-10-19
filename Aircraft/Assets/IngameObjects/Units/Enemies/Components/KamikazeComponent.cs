using Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class KamikazeComponent : MonoBehaviour
    {
        [SerializeField] private float _kamikazeSpeedMultiplier = 2f;
        [SerializeField] private float _damageMultiplier = 2f;
        [SerializeField] private float _hpPercentageThreshold = 0.3f;
        private bool _isKamikazeMode = false;

        public void CheckKamikazeMode(int _currentHp, int p_unitDataMaxHp)
        {
            _isKamikazeMode = _currentHp < p_unitDataMaxHp * _hpPercentageThreshold;
        }

        public bool TryToPerformKamikazeAttack(Rigidbody2D p_rigidbody, Transform p_currentTarget)
        {
            if (_isKamikazeMode && p_currentTarget != null)
            {
                Vector2 direction = (p_currentTarget.position - transform.position).normalized;
                MoveTowardsTarget(direction, p_rigidbody);
                return true;
            }

            return false;
        }

        private void MoveTowardsTarget(Vector2 direction, Rigidbody2D rigidbody)
        {
            rigidbody.velocity = direction * _kamikazeSpeedMultiplier;
        }

        public void OnCollisionEnter2D(Collision2D p_collision)
        {
            if (!p_collision.gameObject.CompareTag("Building") && !p_collision.gameObject.CompareTag("Vehicle"))
                return;

            var unit = p_collision.gameObject.GetComponent<Unit>();
            if (unit != null)
            {
                if (_isKamikazeMode)
                {
                    unit.ReceiveDamage((int)(p_collision.relativeVelocity.magnitude * _damageMultiplier));
                    Destroy(gameObject);
                }
                else
                {
                    unit.ReceiveDamage((int)p_collision.relativeVelocity.magnitude);
                }
            }
        }
    }
}