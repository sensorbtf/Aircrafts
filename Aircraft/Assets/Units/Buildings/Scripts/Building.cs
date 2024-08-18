using System;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buildings
{
    public abstract class Building: Unit, IPointerClickHandler
    {
        [SerializeField] private BuildingSO _buildingData;

        private bool _isBroken;
        private bool _isFunctioning;
        
        public BuildingSO BuildingData => _buildingData;
        
        public void Initialize(BuildingSO p_buildingData)
        {
            _buildingData = p_buildingData;
            UnitData = p_buildingData;

            base.Initialize(p_buildingData);
        }

        public override void Update()
        {
            base.Update();

            CheckForNearbyVehicles();
        }

        public void OnPointerClick(PointerEventData p_eventData)
        {
            OnUnitClicked?.Invoke(this, true);
        }

        public virtual void CheckForNearbyVehicles()
        {
            
        }

        public override void ReceiveDamage(int p_damage)
        {
            CurrentHp -= p_damage;
            CanvasInfo.HealthBar.value = CurrentHp;
            
            if (CurrentHp <= 0)
            {
                OnUnitDied?.Invoke(this);
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