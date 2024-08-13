using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Units
{
    public abstract class Unit: MonoBehaviour
    {
        [Header("Unit")]
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _unitRenderer = null;
        private bool _isSelected;
        
        public UnitSO UnitData;
        public Slider HealthBar;

        public Action<Unit, bool> OnUnitClicked;
        public Action<Unit> OnUnitDied;
        public Action<Unit, Unit> OnUnitAttack;
        
        public int CurrentHp;
        public bool IsSelected => _isSelected;
        public SpriteRenderer UnitRenderer => _unitRenderer;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        
        internal Action OnFireShot;
        internal Action OnWeaponSwitch;

        public void Initialize(UnitSO p_data)
        {
            HealthBar.maxValue = p_data.MaxHp;
            HealthBar.minValue = 0;
            CurrentHp = p_data.MaxHp;
            HealthBar.value = CurrentHp;

            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _unitRenderer = gameObject.GetComponent<SpriteRenderer>();
            
            // Rigidbody2D.drag = EnemyData.Drag;  // Adjust drag to control sliding
            // Rigidbody2D.angularDrag = EnemyData.AngularDrag;  // Control rotational drag
        }
        
        public virtual void SelectUnit()
        {
            _isSelected = true;
        }

        public virtual void UnSelectUnit()
        {
            _isSelected = false;
            // make AI logic
        }

        public abstract void AttackTarget(GameObject p_target);
        public abstract void ReceiveDamage(int p_damage);
        public abstract void DestroyHandler();
    }
}