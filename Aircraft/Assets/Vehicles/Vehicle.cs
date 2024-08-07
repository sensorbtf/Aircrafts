using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vehicles
{
    public abstract class Vehicle: MonoBehaviour, IPointerClickHandler
    {
        public VehicleSO VehicleData { get; private set; }
        public bool IsSelected { get; private set; }

        public float CurrentFuel { get; private set; }
        public Rigidbody2D Rigidbody2D;
        public SpriteRenderer Renderer = null;


        internal Action<Vehicle> OnVehicleClicked;
        internal Action<Vehicle> OnVehicleDestroyed;
        
        public void Initialize(VehicleSO p_vehicleData)
        {
            VehicleData = p_vehicleData;
            CurrentFuel = 100;
            IsSelected = false;

            Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            Renderer = gameObject.GetComponent<SpriteRenderer>();
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
            float move = Input.GetAxis("Horizontal") * VehicleData.Speed * Time.deltaTime;
            transform.Translate(move, 0, 0);
        }

        public virtual void HandleSpecialAction()
        {
            
        }

        public void OnPointerClick(PointerEventData p_eventData)
        {
            OnVehicleClicked?.Invoke(this);
        }
    }
}