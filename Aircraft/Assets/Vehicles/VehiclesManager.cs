using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vehicles
{
    public class VehiclesManager : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private VehiclesDatabase _vehiclesDatabase;
        public VehicleController VehicleController;
        public List<Vehicle> AllVehicles;

        private void Update()
        {
            VehicleController.Update();
        }

        public void CustomStart()
        {
            VehicleController = new VehicleController();
            
            foreach (var vehicleSo in _vehiclesDatabase.Vehicles)
            {
                var newVehicle = Instantiate(vehicleSo.Prefab);
                
                switch (vehicleSo.Type)
                {
                    case VehicleType.Combat:
                        AllVehicles.Add(newVehicle.GetComponent<CombatVehicle>());
                        newVehicle.GetComponent<CombatVehicle>().Initialize(vehicleSo);
                        newVehicle.GetComponent<CombatVehicle>().OnVehicleClicked += SelectVehicle;
                        break;
                    case VehicleType.GeneralPurpose:
                        AllVehicles.Add(newVehicle.GetComponent<GPUVehicle>());
                        newVehicle.GetComponent<GPUVehicle>().Initialize(vehicleSo);
                        newVehicle.GetComponent<GPUVehicle>().OnVehicleClicked += SelectVehicle;
                        break;
                    case VehicleType.Transporter:
                        AllVehicles.Add(newVehicle.GetComponent<TransportingVehicle>());
                        newVehicle.GetComponent<TransportingVehicle>().Initialize(vehicleSo);
                        newVehicle.GetComponent<TransportingVehicle>().OnVehicleClicked += SelectVehicle;
                        break;
                    case VehicleType.Underground:
                        AllVehicles.Add(newVehicle.GetComponent<UndergroundVehicle>());
                        newVehicle.GetComponent<UndergroundVehicle>().Initialize(vehicleSo);
                        newVehicle.GetComponent<UndergroundVehicle>().OnVehicleClicked += SelectVehicle;
                        break;
                }
            }
        }

        public void SelectVehicle(Vehicle p_newVehicle)
        {
            if (VehicleController.CurrentVehicle != null)
            {
                VehicleController.CurrentVehicle.UnSelectVehicle();
            }

            VehicleController.CurrentVehicle = p_newVehicle;
            VehicleController.CurrentVehicle.SelectVehicle();
            _cameraController.PlayerTransform = VehicleController.CurrentVehicle.transform;
        }
    }
}