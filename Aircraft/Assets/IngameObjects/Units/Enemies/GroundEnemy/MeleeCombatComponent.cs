using UnityEngine;

namespace Enemies
{
    public class MeleeCombatComponent: MonoBehaviour, IEnemyCombatComponent
    {
        [SerializeField] internal float _interactionRange = 4f;

        public float InteractionRange { get;}

        public float AttackCooldown { get; }
        public Transform AttackPoint { get; }

        public void AttackUpdate()
        {
            
        }
    }
}