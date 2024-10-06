using System;
using Resources;
using Resources.Scripts;
using UnityEngine;

namespace Buildings
{
    public class ArkBuilding: Building
    {
        [SerializeField] private int _energyToComplete;
        [SerializeField] private ResourceOnProgress[] ResourceOnProgress;

        private int _currentEnergy;
    }


    public class ResourceOnProgress
    {
        public Resource ResourceType;
        public int AmountNeeded;
    }
}