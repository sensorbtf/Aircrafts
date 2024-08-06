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

        internal Action<Vehicle> OnVehicleClicked;
        internal Action<Vehicle> OnVehicleDestroyed;
        
        public void Initialize(VehicleSO p_vehicleData)
        {
            VehicleData = p_vehicleData;
            CurrentFuel = 100;
            IsSelected = false;
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

        public void HandleMovement()
        {
            float move = Input.GetAxis("Horizontal") * VehicleData.Speed * Time.deltaTime;
            transform.Translate(move, 0, 0);
        }

        public void OnPointerClick(PointerEventData p_eventData)
        {
            OnVehicleClicked?.Invoke(this);
        }
    }
}