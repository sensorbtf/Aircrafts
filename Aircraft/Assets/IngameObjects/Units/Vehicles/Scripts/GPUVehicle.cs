using Buildings;
using UnityEngine;

namespace Objects.Vehicles
{
    public class GPUVehicle : Vehicle
    {
        [SerializeField] private float _repairingPercentage;

        public float RepairingPercentage => _repairingPercentage; // bonusese heere

        public override void CheckState()
        {
            var layersToCheck = new[] { LayerManager.VehicleLayer, LayerManager.BuildingLayer };
            var nearbyUnits = GetNearbyObjects(layersToCheck, UnitData.CheckingStateRange);

            foreach (var ingameObject in nearbyUnits)
            {
                if (ingameObject == null || ingameObject == this)
                    continue;

                if (ingameObject is Vehicle vehicle)
                {
                    vehicle.TryToActivateStateButtons(Actions.Repair, this);
                }
                else if (ingameObject is Building building)
                {
                    building.TryToActivateStateButtons(Actions.Repair, this);
                }
            }

            base.CheckState();
        }
    }
}