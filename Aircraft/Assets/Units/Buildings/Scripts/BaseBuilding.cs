using System.Collections.Generic;
using Resources.Scripts;
using Units.Vehicles;
using UnityEngine;

namespace Buildings
{
    public class BaseBuilding: Building
    {
        private List<Vehicle> _vehicles = new ();
        public List<Vehicle> Vehicles => _vehicles;
        
        public override void SelectedUpdate()
        {
            HandleNearestUnits();
        }
        
        private void HandleNearestUnits()
        {
            var nearbyUnits = GetNearbyUnits(new []{ LayerManager.VehicleLayer}, UnitData.CheckingStateRange);

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
            }
        }

        public void TryToAddVehicleToBase(Vehicle p_vehicle)
        {
            _vehicles.Add(p_vehicle);
            p_vehicle.AddVehicleToBase(UnitCollider);
        }
        
        public void RemoveVehicle(Vehicle p_vehicle)
        {
            _vehicles.Remove(p_vehicle);
        }
    }
}
