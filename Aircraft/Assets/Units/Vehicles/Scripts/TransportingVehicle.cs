using Buildings;
using Resources.Scripts;
using UnityEngine;

namespace Units.Vehicles
{
    public class TransportingVehicle: Vehicle
    {
        [SerializeField] private int _refuelAmount;

        public override void HandleSpecialAction()
        {
            HandleNearestUnits();
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                HandleRefuel();
            }
        }

        private void HandleNearestUnits()
        {
            var nearbyUnits = GetNearbyUnits(new []{LayerManager.VehicleLayer, LayerManager.BuildingLayer}, UnitData.CheckingStateRange);
            
            foreach (var unit in nearbyUnits)
            {
                if (unit == null || unit == this) 
                    continue;

                if (unit is Vehicle vehicle)
                {
                    if (Inventory.GetResourceAmount(Resource.Petroleum) > 0)
                    {
                        vehicle.TryToActivateStateButtons(Actions.Refill, this);
                    }
                    
                    if (unit is CombatVehicle combat)
                    {
                        foreach (var weapon in combat.Weapons)
                        {
                            if (Inventory.GetResourceAmount(weapon.Data.AmmoType) > 0)
                            {
                                vehicle.TryToActivateStateButtons(Actions.Arm, this);
                            }
                        }
                    }
                }
                else if (unit is Building building)
                {
                    
                }
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