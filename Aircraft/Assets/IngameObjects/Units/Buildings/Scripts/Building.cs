using System;
using Objects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buildings
{
    public abstract class Building : Unit
    {
        [SerializeField] private BuildingSO _buildingData;
        [SerializeField] private bool _isBroken;

        private bool _isFunctioning;
        private bool _isConnectedToGrid;

        public BuildingSO BuildingData => _buildingData;
        public bool IsBroken => _isBroken;
        public bool IsFunctioning => _isFunctioning;

        public virtual void Initialize(BuildingSO p_buildingData)
        {
            _isBroken = false;
            _buildingData = p_buildingData;
            UnitData = p_buildingData;
            base.Initialize(p_buildingData);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SelectedFixedUpdate()
        {
            base.SelectedFixedUpdate();
        }

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
        }

        public override void OnPointerClick(PointerEventData p_eventData)
        {
            OnUnitClicked?.Invoke(this, true);
        }

        public override void ReceiveDamage(int p_damage)
        {
            CurrentHp -= p_damage;
            CanvasInfo.HealthBar.value = CurrentHp;

            if (CurrentHp < _buildingData.MaxHp * 0.5f)
            {
                _isBroken = true;
            }

            if (CurrentHp <= 0)
            {
                OnUnitDied?.Invoke(this);
            }
        }

        public override void Repair(int p_repaired)
        {
            base.Repair(p_repaired);

            if (CurrentHp >= _buildingData.MaxHp * 0.5f)
            {
                _isBroken = true;
            }
        }

        public override void CheckState()
        {
            if (CurrentHp < UnitData.MaxHp)
            {
                SetNewStateTexts(Actions.Repair);
            }
            else
            {
                ResetStateText(Actions.Repair);
            }

            if (this is ProductionBuilding prod)
            {
                if (Inventory.GetResourceAmount(prod.OutputProduction.Type) > 0)
                {
                    SetNewStateTexts(Actions.Collect);
                }
                else
                {
                    ResetStateText(Actions.Collect);
                }
            }

            // if (this is CombatVehicle combat)// combatbuilding?
            // {
            //     foreach (var weapon in combat.Weapons)
            //     {
            //         if (weapon.CurrentAmmo < weapon.Data.MaxAmmo && Inventory.GetResourceAmount(weapon.Data.AmmoType) > 0)
            //         {
            //             combat.SetNewStateTexts(Actions.Arm, this);
            //         }
            //     }
            // }
        }

        public override void AttackTarget(GameObject p_target)
        {
            throw new NotImplementedException();
        }

        public override void DestroyHandler()
        {
            Destroy(gameObject);
        }
    }
}