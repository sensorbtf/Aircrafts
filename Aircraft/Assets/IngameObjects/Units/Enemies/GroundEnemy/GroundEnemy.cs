using Buildings;
using UnityEngine;
using Vehicles;

namespace Enemies
{
    public class GroundEnemy: Enemy
    {
        private Transform _currentTarget;
        private float _attackCooldown;

        public override void HandleMovement(Transform p_nearestPlayerUnit)
        {
            HandleAttackCooldown();
            base.HandleMovement(p_nearestPlayerUnit);
        }
        // TODO przemyśleć t komponenty. Czy należy oddzielać atakowanie od wykrywania, czy lepiej zostawić je w jednym przez 
        //     kwestię zasięgu, jak określać, czy wróg powinien atakować z łapy czy z bliska,
        //     czy pozwolić przeciwnikom wyprowadzać dwa rodzaje ataku
        //     czy przyda isę komponent związany z kamikadze/wchodzeniem w kolizję, czy to już sprawa czołgu
        //         jak rozwiązać komponent przekopywania się
        //     jak rozwinąć komponent np. tarczy
        public override void HandleSpecialAction()
        {
            // var hitCollider = Physics2D.OverlapCircle(AttackPoint.position, 0.1f);
            // if (hitCollider != null && (hitCollider.gameObject.CompareTag("Building") ||
            //                             hitCollider.gameObject.CompareTag("Vehicle")))
            // {
            //     _currentTarget = hitCollider.transform;
            // }
            // else
            // {
            //     _currentTarget = null;
            // }
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