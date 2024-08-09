using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Buildings
{
    public class BuildingsManager: MonoBehaviour
    {
        [SerializeField] private BuildingsDatabase _buildingsDatabase;
        public List<Building> Buildings;

        public void CustomStart()
        {
            foreach (var building in Buildings)
            {
            }
        }

        public Building GetMainBase()
        {
            return Buildings.FirstOrDefault(x => x.BuildingData.Type == BuildingType.Main_Base);
        }
    }
}