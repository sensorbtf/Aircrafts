using System;
using System.Linq;
using Enemies;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Units.Vehicles
{
    public abstract class Vehicle : Unit, IPointerClickHandler
    {
        [Header("Vehicle")] private VehicleSO _vehicleData;
        private int _currentFuel;
        private int _fuelUsageInterval;
        public int CurrentFuel => _currentFuel;
        public VehicleSO VehicleData => _vehicleData;

        public Action OnFuelChange;

        public virtual void Initialize(VehicleSO p_vehicleData)
        {
            _vehicleData = p_vehicleData;
            UnitData = p_vehicleData;
            _currentFuel = p_vehicleData.MaxFuel;
            _fuelUsageInterval = 0;
            base.Initialize(p_vehicleData);
        }
        
        public virtual void HandleMovement()
        {
            var moveHorizontal = Input.GetAxis("Horizontal");
            var force = Vector2.zero;

            if (moveHorizontal > 0)
            {
                force = Vector2.right * _vehicleData.Speed;
                UnitRenderer.flipX = false;
            }
            else if (moveHorizontal < 0)
            {
                force = Vector2.left * _vehicleData.Speed;
                UnitRenderer.flipX = true;
            }

            if (force != Vector2.zero)
            {
                Rigidbody2D.AddForce(force, ForceMode2D.Force);
                HandleFuelUsage();
            }

            float maxSpeed = 10f;
            if (Rigidbody2D.velocity.magnitude > maxSpeed)
            {
                Rigidbody2D.velocity = Rigidbody2D.velocity.normalized * maxSpeed;
            }
        }

        public virtual void HandleFuelUsage()
        {
            _fuelUsageInterval++;

            if (_fuelUsageInterval >= _vehicleData.LiterUsageInterval)
            {
                LowerFuel(1);
                _fuelUsageInterval = 0;
                OnFuelChange?.Invoke();
            }
        }

        public override void AttackTarget(GameObject target)
        {
            var potentialEnemy = target.GetComponent<Enemy>();

            if (potentialEnemy != null)
            {
                potentialEnemy.ReceiveDamage(_vehicleData.AttackDamage);
            }
        }

        public virtual void HandleSpecialAction()
        {
        }

        public void OnPointerClick(PointerEventData p_eventData)
        {
            OnUnitClicked?.Invoke(this, true);
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

        public override void DestroyHandler()
        {
            Destroy(gameObject);
        }
        
        public override void CheckState()
        {
            if (CurrentFuel < VehicleData.MaxFuel)
            {
                SetNewStateTexts(Actions.Refill);
            }
            else
            {
                ResetStateText(Actions.Refill);
            }

            if (CurrentHp < UnitData.MaxHp)
            {
                SetNewStateTexts(Actions.Repair);
            }
            else
            {
                ResetStateText(Actions.Repair);
            }

            if (this is CombatVehicle combat)
            {
                if (combat.Weapons.Any(x=>x.CurrentAmmo < x.Data.MaxAmmo))
                {
                    combat.SetNewStateTexts(Actions.Arm);
                }
                else
                {
                    combat.ResetStateText(Actions.Arm);
                }
            }
        }

        public void LowerFuel(int p_amount)
        {
            _currentFuel -= p_amount;
        }

        public void RiseFuel(int p_amount)
        {
            _currentFuel += p_amount;
        }
    }
}