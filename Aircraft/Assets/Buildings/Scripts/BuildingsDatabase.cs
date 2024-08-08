using System.Linq;
using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(fileName = "Buildings", menuName = "Buidlings/BuildingsDatabase", order = 3)]
    public class BuildingsDatabase: ScriptableObject
    {
        public BuildingSO[] Buildings;

        public BuildingSO GetBuildingByType(BuildingType p_type)
        {
            return Buildings.FirstOrDefault(x => x.Type == p_type);
        }
    }
}