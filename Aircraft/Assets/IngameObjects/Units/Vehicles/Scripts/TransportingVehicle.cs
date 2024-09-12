using Buildings;
using Resources.Scripts;
using UnityEngine;

namespace Objects.Vehicles
{
    public class TransportingVehicle: Vehicle
    {
        [SerializeField] private int _refuelAmount;

        public override void SelectedUpdate()
        {
            HandleNearestUnits();
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                HandleRefuel();
            }
        }

        private void HandleNearestUnits()
        {
            var nearbyUnits = GetNearbyObjects(new []{LayerManager.VehicleLayer, LayerManager.BuildingLayer, LayerManager.ItemsLayer}, UnitData.CheckingStateRange);
            
            foreach (var ingameObject in nearbyUnits)
            {
                if (ingameObject == null || ingameObject == this) 
                    continue;

                if (ingameObject is Vehicle vehicle)
                {
                    if (Inventory.GetResourceAmount(Resource.Petroleum) > 0)
                    {
                        vehicle.TryToActivateStateButtons(Actions.Refill, this);
                    }
                    
                    if (ingameObject is CombatVehicle combat)
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
                else if (ingameObject is Building building)
                {
                    if (building is ProductionBuilding prodBuilding)
                    {
                        prodBuilding.TryToActivateStateButtons(Actions.Collect, prodBuilding, this);
                    }
                }
                else if (ingameObject is ItemOnGround item)
                {
                    item.TryToActivateStateButtons(Actions.Collect, item, this);
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