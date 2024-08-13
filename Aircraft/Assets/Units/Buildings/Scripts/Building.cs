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
        
        public void OnPointerClick(PointerEventData p_eventData)
        {
            OnUnitClicked?.Invoke(this, true);
        }

        public override void ReceiveDamage(int p_damage)
        {
            CurrentHp -= p_damage;
            HealthBar.value = CurrentHp;
            
            if (CurrentHp <= 0)
            {
                OnUnitDied?.Invoke(this);
            }
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