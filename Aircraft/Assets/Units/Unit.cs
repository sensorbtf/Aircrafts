using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Units
{
    public abstract class Unit: MonoBehaviour
    {
        public UnitSO Data;
        public Slider HealthBar;
        public Rigidbody2D Rigidbody2D;
        public SpriteRenderer UnitRenderer = null;
        public bool IsSelected;

        public Action<Unit> OnUnitClicked;
        public Action<Unit> OnUnitDied;
        public Action<Unit, Unit> OnUnitAttack;
        
        public int CurrentHp;

        public void Initialize(UnitSO p_data)
        {
            HealthBar.maxValue = p_data.MaxHp;
            HealthBar.minValue = 0;
            CurrentHp = p_data.MaxHp;
            HealthBar.value = CurrentHp;

            Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            UnitRenderer = gameObject.GetComponent<SpriteRenderer>();
            
            // Rigidbody2D.drag = EnemyData.Drag;  // Adjust drag to control sliding
            // Rigidbody2D.angularDrag = EnemyData.AngularDrag;  // Control rotational drag
        }

        public abstract void AttackTarget(GameObject p_target);
        public abstract void ReceiveDamage(int p_damage);
        public abstract void DestroyHandler();
    }
}