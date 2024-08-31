using System.Collections;
using System.Collections.Generic;
using Buildings;
using UnityEngine;

namespace Objects.Vehicles
{
    public class SelectedUnitController
    {
        private Unit _currentUnit;

        public Unit CurrentUnit => _currentUnit;

        public void FixedUpdate()
        {
            if (_currentUnit != null)
            {
                _currentUnit.SelectedFixedUpdate();
            }
        }

        public void Update()
        {
            if (_currentUnit != null)
            {
                _currentUnit.SelectedUpdate();
            }
        }

        public void SetNewUnit(Unit p_unit)
        {
            _currentUnit = p_unit;
        }
    }
}