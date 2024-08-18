using System.Collections.Generic;
using Resources.Scripts;
using Units.Vehicles;
using UnityEngine;

namespace Buildings
{
    public class AdministrativeBuilding: Building
    {
        public override void Update()
        {
            base.Update();
            
            //andleNearestUnits();
        }

        private void HandleNearestUnits()
        {
            var nearbyUnits = GetNearbyUnits(new []{ LayerManager.BuildingLayer}, UnitData.CheckingStateRange);

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
    }
}