using UnityEngine;

namespace Vehicles
{
    [CreateAssetMenu(fileName = "VehiclesDatabase", menuName = "Vehicle/VehiclesDatabase", order = 3)]
    public class VehiclesDatabase: ScriptableObject
    {
        public VehicleSO[] Vehicles;
    }
}