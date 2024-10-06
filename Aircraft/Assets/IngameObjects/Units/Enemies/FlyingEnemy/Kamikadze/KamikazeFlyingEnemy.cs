using Buildings;
using Objects;
using UnityEngine;
using Vehicles;

namespace Enemies
{
    public class KamikazeFlyingEnemy: FlyingEnemy
    {
        public override void HandleMovement(Transform p_playerBase)
        {
            if (CurrentHp < UnitData.MaxHp * 0.3f && _currentTarget != null)
            {
                MoveTowards((_currentTarget.position - transform.position).normalized);
            }
            else
            {
                base.HandleMovement(p_playerBase);
            }
        }

        public override void HandleSpecialAction()
        {
            base.HandleSpecialAction();
        }

        private void MoveTowards(Vector2 p_direction) // kamikadze mode
        {
            Rigidbody2D.velocity = p_direction * EnemyData.Speed * 2;
        }

        private void OnCollisionEnter2D(Collision2D collision) // kamikadze
        {
            if (collision.gameObject.CompareTag("Building") || collision.gameObject.CompareTag("Vehicle"))
            {
                var unit = collision.gameObject.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.ReceiveDamage(EnemyData.AttackDamage*2);
                }

                ReceiveDamage(100000);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _shootingRange);
        }
    }
}
