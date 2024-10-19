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
        }
        //     jak rozwiązać komponent przekopywania się
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
    }
}