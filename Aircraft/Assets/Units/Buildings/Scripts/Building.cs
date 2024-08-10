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
            Data = p_buildingData;

            HealthBar.maxValue = _buildingData.MaxHp;
            HealthBar.minValue = 0;
            CurrentHp = _buildingData.MaxHp;
            HealthBar.value = CurrentHp;

            Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
        
        public void OnPointerClick(PointerEventData p_eventData)
        {
            OnUnitClicked?.Invoke(this);
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