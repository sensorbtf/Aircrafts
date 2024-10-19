using System;
using Objects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buildings
{
    public abstract class EnergyBuilding: Building
    {
        // need to get energy threshold 0,25,50,75,100 and only then build up happens
        // Rethink energy output from energy buildings
        private int _currentEnergyGeneration;
        public int CurrentEnergyGeneration => _currentEnergyGeneration;
    }
}