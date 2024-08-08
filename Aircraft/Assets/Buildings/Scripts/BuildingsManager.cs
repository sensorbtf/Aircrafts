using System.Collections.Generic;
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
    }
}