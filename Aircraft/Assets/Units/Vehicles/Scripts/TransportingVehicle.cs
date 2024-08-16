using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units.Vehicles
{
    public class TransportingVehicle : Vehicle
    {
        [Header("Transporting Vehicle")]
        [SerializeField] private float _checkRange;
        [SerializeField] private LayerMask _vehicleLayerMask;
        [SerializeField] private int _refuelAmount; 
        
        private void CheckForNearbyVehicles()
        {
            foreach (var vehicleCollider in Physics2D.OverlapCircleAll(transform.position, _checkRange, _vehicleLayerMask))
            {
                var nearbyVehicle = vehicleCollider.GetComponent<Vehicle>();

                if (nearbyVehicle != null && nearbyVehicle != this)
                {
                    if (nearbyVehicle.CurrentFuel < nearbyVehicle.VehicleData.MaxFuel)
                    {
                        nearbyVehicle.SetNewStateTexts(Actions.Refill, this);
                    }

                    if (nearbyVehicle is CombatVehicle combat)
                    {
                        if (combat.Weapons.Any(x => x.CurrentAmmo < x.Data.MaxAmmo))
                        {
                            nearbyVehicle.SetNewStateTexts(Actions.Arm, this);
                        }
                    }
                }
            }
        }

        public override void HandleSpecialAction()
        {
            CheckForNearbyVehicles();
            if (Input.GetKeyDown(KeyCode.R)) // Press 'R' to manually refuel
            {
                HandleRefuel();
            }
        }

        private void HandleRefuel()
        {
            if (CurrentFuel < VehicleData.MaxFuel)
            {
                RiseFuel(_refuelAmount);
                OnFuelChange?.Invoke();
                Debug.Log("Vehicle refueled. Current fuel: " + CurrentFuel);
            }
            else
            {
                Debug.Log("Fuel is already full.");
            }
        }
    }
}