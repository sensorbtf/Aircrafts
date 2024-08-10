using System;
using UnityEngine;
using UnityEngine.UI;

namespace Units
{
    public abstract class Unit: MonoBehaviour
    {
        public UnitSO Data;
        public Slider HealthBar;
        public Rigidbody2D Rigidbody2D;
        public SpriteRenderer SpriteRenderer = null;

        public Action<Unit> OnUnitClicked;
        public Action<Unit> OnUnitDied;
        public Action<Unit, Unit> OnUnitAttack;
        
        public int CurrentHp;

        public abstract void AttackTarget(GameObject p_target);
        public abstract void ReceiveDamage(int p_damage);
        public abstract void DestroyHandler();
    }
}