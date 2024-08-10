using UnityEngine;
using UnityEngine.UI;

namespace Units
{
    public abstract class Unit: MonoBehaviour
    {
        public Slider HealthBar;
        public Rigidbody2D Rigidbody2D;
        public SpriteRenderer SpriteRenderer = null;

        public int CurrentHp;
        
        public abstract void AttackTarget(GameObject p_target);
        public abstract void ReceiveDamage(int p_damage);
    }
}