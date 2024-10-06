using System.Collections.Generic;
using Objects;
using Resources.Scripts;
using Objects.Vehicles;
using UnityEngine;

namespace Buildings
{
    public class BaseBuilding: Building
    {
        [SerializeField] private float _percentageRepairAmount = 0.05f;
        [SerializeField] private int _timeToRepair = 5;

        private List<Vehicle> _vehiclesInBase = new();
        public List<Vehicle> VehiclesInBase => _vehiclesInBase;

        public override void Initialize(BuildingSO p_buildingData)
        {
            base.Initialize(p_buildingData);
        }

        public override void Update()
        {
            base.Update();
            
            foreach (var vehicle in _vehiclesInBase)
            {
                if (!vehicle.IsInBase)
                    continue;

                vehicle.TryToRepairInBase(_timeToRepair, _percentageRepairAmount);
            }
        }

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
            HandleNearestUnits();
        }

        private void HandleNearestUnits()
        {
            var nearbyUnits = GetNearbyObjects(new[] { LayerManager.VehicleLayer }, UnitData.CheckingStateRange);

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
                    else if (unit is TransportingVehicle TV)
                    {
                        TV.SetNewStateTexts(Actions.Deposit);
                        TV.TryToActivateStateButtons(Actions.Deposit, this, TV, false);
                    }
                }
            }
        }

        public void TryToAddVehicleToBase(Vehicle p_vehicle)
        {
            _vehiclesInBase.Add(p_vehicle);
            p_vehicle.AddVehicleToBase(ObjectCollider);

            p_vehicle.OnUnitDied += RemoveVehicle;
        }

        private void RemoveVehicle(Unit p_unit)
        {
            if (p_unit is Vehicle veh)
            {
                RemoveVehicle(veh);
            }
        }

        public void RemoveVehicle(Vehicle p_vehicle)
        {
            _vehiclesInBase.Remove(p_vehicle);
        }
    }
}