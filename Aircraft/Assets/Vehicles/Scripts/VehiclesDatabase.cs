using System.Linq;
using UnityEngine;

namespace Vehicles
{
    [CreateAssetMenu(fileName = "VehiclesDatabase", menuName = "Vehicle/VehiclesDatabase", order = 3)]
    public class VehiclesDatabase: ScriptableObject
    {
        public VehicleSO[] Vehicles;
        
        public VehicleSO GetVehicleByType(VehicleType p_type)
        {
            return Vehicles.FirstOrDefault(x => x.Type == p_type);
        }
    }
}