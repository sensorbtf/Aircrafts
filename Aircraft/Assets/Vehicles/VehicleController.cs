using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vehicles
{
    public class VehicleController
    {
        public Vehicle CurrentVehicle;

        public void FixedUpdate()
        {
            if (CurrentVehicle != null)
            {
                CurrentVehicle.HandleMovement();
            }
        }

        public void Update()
        {
            if (CurrentVehicle != null)
            {
                CurrentVehicle.HandleMovement();
            }
        }
    }
}