using System;
using System.Linq;
using Resources;
using Resources.Scripts;
using Units.Vehicles;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Units
{
    public abstract class Unit : MonoBehaviour
    {
        [Header("Unit")] private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _unitRenderer = null;
        private InventoryController _inventory;
        private bool _isSelected;

        private UnitSO _unitData;
        public InfoCanvasRefs CanvasInfo;

        public Action<Unit, bool> OnUnitClicked;
        public Action<Unit> OnUnitDied;
        public Action<Unit, Unit> OnUnitAttack;

        public int CurrentHp;
        public bool IsSelected => _isSelected;
        public SpriteRenderer UnitRenderer => _unitRenderer;
        public Rigidbody2D Rigidbody2D => _rigidbody2D;
        public InventoryController Inventory => _inventory;

        public UnitSO UnitData
        {
            get => _unitData;
            protected set => _unitData = value;
        }

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

        public virtual void Update()
        {
            CheckState();
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

        protected void CompactStateInfo()
        {
            int targetIndex = 0; 

            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                if (!string.IsNullOrEmpty(CanvasInfo.StateInfo[i].TextInfo.text))
                {
                    if (targetIndex != i)
                    {
                        CanvasInfo.StateInfo[targetIndex].TextInfo.text = CanvasInfo.StateInfo[i].TextInfo.text;
                        CanvasInfo.StateInfo[targetIndex].Action = CanvasInfo.StateInfo[i].Action;
                        CanvasInfo.StateInfo[targetIndex].Button.interactable =
                            CanvasInfo.StateInfo[i].Button.interactable;
                        CanvasInfo.StateInfo[targetIndex].Button.onClick.RemoveAllListeners();
                        CanvasInfo.StateInfo[targetIndex].Button.onClick.AddListener(() =>
                        {
                            MakeAction(CanvasInfo.StateInfo[i].Action, this, this);
                        });

                        CanvasInfo.StateInfo[i].TextInfo.text = "";
                        CanvasInfo.StateInfo[i].Action = Actions.Noone;
                        CanvasInfo.StateInfo[i].Button.interactable = false;
                        CanvasInfo.StateInfo[i].Button.onClick.RemoveAllListeners();
                    }

                    targetIndex++; 
                }
            }
        }

        protected void SetNewStateTexts(Actions p_actionType)
        {
            CompactStateInfo();

            if (CanvasInfo.StateInfo.Any(x => x.Action == p_actionType))
                return;

            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                if (string.IsNullOrEmpty(CanvasInfo.StateInfo[i].TextInfo.text))
                {
                    CanvasInfo.StateInfo[i].Button.interactable = false;
                    CanvasInfo.StateInfo[i].TextInfo.text = p_actionType.ToString();
                    CanvasInfo.StateInfo[i].Action = p_actionType;
                    return;
                }
            }

            CanvasInfo.StateInfo[2].Button.interactable = false;
            CanvasInfo.StateInfo[2].TextInfo.text = p_actionType.ToString();
            CanvasInfo.StateInfo[2].Action = p_actionType;
        }

        protected void ResetStateText(Actions p_previousAction)
        {
            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                if (CanvasInfo.StateInfo[i].Action == p_previousAction)
                {
                    CanvasInfo.StateInfo[i].TextInfo.text = "";
                    CanvasInfo.StateInfo[i].Button.interactable = false;
                    CanvasInfo.StateInfo[i].Action = Actions.Noone;
                    CompactStateInfo(); 
                    return;
                }
            }
        }

        public void TryToActivateStateButtons(Actions p_actionType, Vehicle p_giver)
        {
            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                if (CanvasInfo.StateInfo[i].Action == p_actionType)
                {
                    SetAction(p_actionType, p_giver, i);
                    CompactStateInfo(); 
                    return;
                }
            }
        }


        private void SetAction(Actions p_actionType, Unit p_giver, int p_index)
        {
            CanvasInfo.StateInfo[p_index].TextInfo.text = p_actionType.ToString();

            CanvasInfo.StateInfo[p_index].Button.interactable = true;
            CanvasInfo.StateInfo[p_index].Button.onClick.RemoveAllListeners();
            CanvasInfo.StateInfo[p_index].Button.onClick.AddListener(delegate
            {
                MakeAction(p_actionType, p_giver, this);
            });
        }

        public void ResetStateButtons()
        {
            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                CanvasInfo.StateInfo[i].Action = Actions.Noone;
                CanvasInfo.StateInfo[i].Button.interactable = false;
                CanvasInfo.StateInfo[i].Button.onClick.RemoveAllListeners();
                CanvasInfo.StateInfo[i].TextInfo.text = "";
            }
        }

        private void MakeAction(Actions p_actionType, Unit p_giver, Unit p_receiver)
        {
            switch (p_actionType)
            {
                case Actions.Refill:
                    if (p_receiver is Vehicle vehicle)
                    {
                        var availableFuel = p_giver.Inventory.GetResourceAmount(Resource.Petroleum);
                        var neededFuel = vehicle.VehicleData.MaxFuel - vehicle.CurrentFuel;

                        if (availableFuel >= neededFuel)
                        {
                            p_giver.Inventory.RemoveResource(Resource.Petroleum, neededFuel);
                            vehicle.RiseFuel(neededFuel);
                        }
                        else
                        {
                            p_giver.Inventory.RemoveResource(Resource.Petroleum, availableFuel);
                            vehicle.RiseFuel(availableFuel);
                        }
                    }

                    break;
                case Actions.Arm:
                    if (p_receiver is CombatVehicle combatVehicle)
                    {
                        foreach (var weapon in combatVehicle.Weapons)
                        {
                            var availableAmmo = p_giver.Inventory.GetResourceAmount(weapon.Data.AmmoType);
                            var ammoNeededForMax = weapon.Data.MaxAmmo - weapon.CurrentAmmo;

                            if (availableAmmo >= ammoNeededForMax)
                            {
                                p_giver.Inventory.RemoveResource(weapon.Data.AmmoType, ammoNeededForMax);
                                weapon.CurrentAmmo += ammoNeededForMax;
                            }
                            else
                            {
                                p_giver.Inventory.RemoveResource(weapon.Data.AmmoType, availableAmmo);
                                weapon.CurrentAmmo += availableAmmo;
                            }
                        }
                    }

                    break;
                case Actions.Repair:
                    break;
            }
        }

        public abstract void AttackTarget(GameObject p_target);
        public abstract void ReceiveDamage(int p_damage);
        public abstract void DestroyHandler();
        public abstract void CheckState();
    }
}