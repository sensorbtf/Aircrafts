using Buildings;
using Objects;
using UnityEngine;
using Vehicles;

namespace Enemies
{
    public class KamikazeFlyingEnemy: FlyingEnemy
    {
        public KamikazeComponent KamikazeComponent;

        public override void HandleMovement(Transform p_nearestPlayerUnit)
        {
            if (!KamikazeComponent.TryToPerformKamikazeAttack(Rigidbody2D, CurrentTarget))
            {
                KamikazeComponent.CheckKamikazeMode(CurrentHp, UnitData.MaxHp);
                base.HandleMovement(p_nearestPlayerUnit);
            }
        }

        private void OnCollisionEnter2D(Collision2D p_collision)
        {
            KamikazeComponent.OnCollisionEnter2D(p_collision);
        }
    }
}
