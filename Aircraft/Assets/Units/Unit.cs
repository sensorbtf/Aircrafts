using System;
using System.Linq;
using Resources;
using Units.Vehicles;
using UnityEditor.Timeline.Actions;
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
        private InventoryController _inventory;
        private bool _isSelected;
        
        public UnitSO UnitData;
        public InfoCanvasRefs CanvasInfo;

        public Action<Unit, bool> OnUnitClicked;
        public Action<Unit> OnUnitDied;
        public Action<Unit, Unit> OnUnitAttack;
        
        public int CurrentHp;
        public bool IsSelected => _isSelected;
        public SpriteRenderer UnitRenderer => _unitRenderer;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        private InventoryController Inventory => _inventory;
        
        internal Action OnFireShot;
        internal Action OnWeaponSwitch;

        public virtual void Initialize(UnitSO p_data)
        {
            CurrentHp = p_data.MaxHp;
            
            CanvasInfo.HealthBar.maxValue = p_data.MaxHp;
            CanvasInfo.HealthBar.minValue = 0;
            CanvasInfo.HealthBar.value = CurrentHp;

            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                CanvasInfo.StateInfo[i].TextInfo.text = "";
            }
            
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _unitRenderer = gameObject.GetComponent<SpriteRenderer>();

            // Rigidbody2D.drag = EnemyData.Drag;  // Adjust drag to control sliding
            // Rigidbody2D.angularDrag = EnemyData.AngularDrag;  // Control rotational drag
        }
       
        public virtual void PostInitialize(InventoryController p_newInventory)
        {
            _inventory = p_newInventory;
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

        public void SetNewStateTexts(Actions p_actionType, Vehicle p_giver)
        {
            if (CanvasInfo.StateInfo.Any(x=> x.TextInfo.text == p_actionType.ToString()))
                return;
            
            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                if (string.IsNullOrEmpty(CanvasInfo.StateInfo[i].TextInfo.text))
                {
                    CanvasInfo.StateInfo[i].TextInfo.text = p_actionType.ToString();
                    CanvasInfo.StateInfo[i].Button.onClick.RemoveAllListeners();;
                    CanvasInfo.StateInfo[i].Button.onClick.AddListener(delegate
                    {
                        MakeAction(p_actionType, p_giver, this);
                    });
                    return;
                }
            }

            CanvasInfo.StateInfo[2].TextInfo.text = p_actionType.ToString();
        }

        private void MakeAction(Actions p_actionType, Vehicle p_giver, Unit p_receiver)
        {
            switch (p_actionType)
            {
                case Actions.Refill:
                    break;
                case Actions.Arm:
                    break;
                case Actions.Repair:
                    break;
            }
        }

        public abstract void AttackTarget(GameObject p_target);
        public abstract void ReceiveDamage(int p_damage);
        public abstract void DestroyHandler();
    }
}