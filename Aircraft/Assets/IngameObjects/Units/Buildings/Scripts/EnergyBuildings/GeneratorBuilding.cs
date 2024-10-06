using System;
using Resources;
using Resources.Scripts;
using UnityEngine;

namespace Buildings
{
    public class GeneratorBuilding: Building
    {
        [SerializeField] private int _timeForBarellUsage;
        [SerializeField] private int _energyFromOneBarellCycle;
        [SerializeField] private int _maxBarellInput;

        private int _currentBarrels;
        private float _currentProductionTime;
        private bool _isPaused;
        private float _timer;
        
        public bool IsPaused => _currentBarrels == 0;
        public float CurrentProductionTime => _currentProductionTime;
        public int TimeToComplete => _timeForBarellUsage;

        public override void Initialize(BuildingSO p_buildingData)
        {
            base.Initialize(p_buildingData);
            _currentProductionTime = 0;
        }
        
        public override void Update()
        {
            base.Update();

            _timer += Time.deltaTime;
            if (!(_timer >= 1f)) return; // only every second
            _timer = 0f;

            if (_currentProductionTime >= _timeForBarellUsage)
            {
                _currentProductionTime = 0;
            }
        }
    }
}