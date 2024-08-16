using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Vehicles
{
    public class VehicleController
    {
        private Vehicle _currentVehicle;

        public Vehicle CurrentVehicle => _currentVehicle;

        public void FixedUpdate()
        {
            if (_currentVehicle != null)
            {
                _currentVehicle.HandleMovement();
            }
        }

        public void Update()
        {
            if (_currentVehicle != null)
            {
                _currentVehicle.HandleSpecialAction();
            }
        }

        public void SetNewVehicle(Vehicle p_newlySelectedVehicle)
        {
            _currentVehicle = p_newlySelectedVehicle;
        }
    }
}