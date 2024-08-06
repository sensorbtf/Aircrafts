using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vehicles
{
    public class VehicleController
    {
        public Vehicle CurrentVehicle;

        public void Update()
        {
            if (CurrentVehicle != null)
            {
                CurrentVehicle.HandleMovement();
            }
        }
    }
}