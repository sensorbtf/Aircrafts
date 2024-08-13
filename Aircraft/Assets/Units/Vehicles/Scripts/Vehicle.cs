using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using Enemies;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vehicles
{
    public abstract class Vehicle: Unit, IPointerClickHandler
    {
        public VehicleSO VehicleData { get; private set; }
        public float CurrentFuel { get; private set; }

        internal Action OnFireShot;

        public virtual void Initialize(VehicleSO p_vehicleData)
        {
            VehicleData = p_vehicleData;
            Data = p_vehicleData;
            CurrentFuel = p_vehicleData.MaxFuel;
            base.Initialize(p_vehicleData);
        }

        public void SelectVehicle()
        {
            IsSelected = true;
        }

        public void UnSelectVehicle()
        {
            IsSelected = false;
            // make AI logic
        }

        public void LowerFuel()
        {
            CurrentFuel--;
        }

        public virtual void HandleMovement()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");

            Vector2 force = Vector2.zero;

            if (moveHorizontal > 0)
            {
                force = Vector2.right * VehicleData.Speed;
                UnitRenderer.flipX = false;
            }
            else if (moveHorizontal < 0)
            {
                force = Vector2.left * VehicleData.Speed;
                UnitRenderer.flipX = true;
            }

            if (force != Vector2.zero)
            {
                Rigidbody2D.AddForce(force, ForceMode2D.Force);
            }

            float maxSpeed = 10f;
            if (Rigidbody2D.velocity.magnitude > maxSpeed)
            {
                Rigidbody2D.velocity = Rigidbody2D.velocity.normalized * maxSpeed;
            }
        }

        public override void AttackTarget(GameObject target)
        {
            Debug.Log($"Attacking {target.name}");

            var potentialEnemy = target.GetComponent<Enemy>();

            if (potentialEnemy != null)
            {
                potentialEnemy.ReceiveDamage(VehicleData.AttackDamage);
            }
        }

        public virtual void HandleSpecialAction()
        {
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
        
        public override void DestroyHandler()
        {
            Destroy(gameObject);
        }
    }
}