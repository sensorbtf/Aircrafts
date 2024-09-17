using System;
using System.Linq;
using Buildings;
using Enemies;
using Resources.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Objects.Vehicles
{
    public abstract class Vehicle: Unit
    {
        [Header("Vehicle")] private VehicleSO _vehicleData;
        private int _currentFuel;
        private bool _isInBase;
        private int _fuelUsageInterval;
        private Collider2D _baseArea;

        public int CurrentFuel => _currentFuel;
        public bool IsInBase => _isInBase;
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
        
        public override void Update()
        {
            base.Update();
        }
        
        public override void SelectedFixedUpdate()
        {
            base.SelectedFixedUpdate();
            
            var moveHorizontal = Input.GetAxis("Horizontal");
            var force = Vector2.zero;

            if (moveHorizontal > 0)
            {
                force = Vector2.right * _vehicleData.Speed;
                ObjectRenderer.flipX = false;
            }
            else if (moveHorizontal < 0)
            {
                force = Vector2.left * _vehicleData.Speed;
                ObjectRenderer.flipX = true;
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
        
        public override void OnPointerClick(PointerEventData p_eventData)
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
            if (_isInBase)
                return;

            var nearbyUnits = GetNearbyObjects(new[] { LayerManager.PointOfInterest }, UnitData.CheckingStateRange);

            foreach (var ingameObject in nearbyUnits)
            {
                if (ingameObject == null || ingameObject == this)
                    continue;

                if (ingameObject is PointOfInterest poi)
                {
                    poi.TryToActivateStateButtons(Actions.AcceptQuest, this);
                }
            }

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
        }

        public void LowerFuel(int p_amount)
        {
            _currentFuel -= p_amount;
        }

        public void RiseFuel(int p_amount)
        {
            _currentFuel += p_amount;
        }
        
        public override void SelectObject()
        {
            base.SelectObject();

            OnBaseExit();
        }

        public override void UnSelectObject()
        {
            base.UnSelectObject();

            if (_baseArea != null && _baseArea.OverlapPoint(transform.position))
            {
                if (!_isInBase)
                {
                    OnBaseEntry();
                }
            }
            else
            {
                if (_isInBase)
                {
                    OnBaseExit();
                }
            }
        }

        public void AddVehicleToBase(Collider2D p_baseArea)
        {
            _baseArea = p_baseArea;
            OnBaseEntry();
        }
        
        public void OnBaseEntry()
        {
            _isInBase = true;
            gameObject.SetActive(false);
        }
        
        public void OnBaseExit()
        {
            _isInBase = false;
            gameObject.SetActive(true);
        }
    }
}