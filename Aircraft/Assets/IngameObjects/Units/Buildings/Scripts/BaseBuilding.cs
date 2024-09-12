using System.Collections.Generic;
using Resources.Scripts;
using Objects.Vehicles;
using UnityEngine;

namespace Buildings
{
    public class BaseBuilding: Building
    {
        private List<Vehicle> _vehiclesInBase = new ();
        public List<Vehicle> VehiclesInBase => _vehiclesInBase;

        public override void Initialize(BuildingSO p_buildingData)
        {
            base.Initialize(p_buildingData);
        }

        public override void SelectedUpdate()
        {
            HandleNearestUnits();
        }
        
        private void HandleNearestUnits()
        {
            var nearbyUnits = GetNearbyObjects(new []{ LayerManager.VehicleLayer}, UnitData.CheckingStateRange);

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
            _vehiclesInBase.Add(p_vehicle);
            p_vehicle.AddVehicleToBase(ObjectCollider);
        }
        
        public void RemoveVehicle(Vehicle p_vehicle)
        {
            _vehiclesInBase.Remove(p_vehicle);
        }
    }
}
