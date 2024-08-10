using System;
using System.Collections;
using System.Collections.Generic;
using Buildings;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vehicles
{
    public abstract class Vehicle: Unit, IPointerClickHandler
    {
        public VehicleSO VehicleData { get; private set; }
        public bool IsSelected { get; private set; }

        public float CurrentFuel { get; private set; }

        internal Action<Vehicle> OnVehicleClicked;
        internal Action<Vehicle> OnVehicleDestroyed;
        
        public void Initialize(VehicleSO p_vehicleData)
        {
            VehicleData = p_vehicleData;
            CurrentFuel = 100;
            IsSelected = false;

            Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
                SpriteRenderer.flipX = false;
            }
            else if (moveHorizontal < 0)
            {
                force = Vector2.left * VehicleData.Speed;
                SpriteRenderer.flipX = true;
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
            
            var potentialVehicle = target.GetComponent<Vehicle>();
            
            if (potentialVehicle != null)
            {
                potentialVehicle.ReceiveDamage(VehicleData.AttackDamage);
            }
            else
            {
                var potentialBuilding = target.GetComponent<Building>();

                if (potentialBuilding != null)
                {
                    potentialBuilding.RecieveDamage(VehicleData.AttackDamage);
                }
            }
        }

        public virtual void HandleSpecialAction()
        {
            
        }

        public void OnPointerClick(PointerEventData p_eventData)
        {
            OnVehicleClicked?.Invoke(this);
        }

        public override void ReceiveDamage(int p_damage)
        {
            throw new NotImplementedException();
        }
    }
}