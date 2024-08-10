using System;
using System.Collections.Generic;
using Buildings;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vehicles;

namespace UI.HUD
{
    public class HUDManager : MonoBehaviour
    {
        [Header("Panels")] [SerializeField] private RightDownPanelController _rightDownPanelController;

        [SerializeField] private UnitsManager _unitsManager;
        [SerializeField] private Slider _fuelSlider;
        [SerializeField] private TextMeshProUGUI _fuelSliderText;
        [SerializeField] private GameObject _combatVehicles;
        [SerializeField] private GameObject _utilityVehicles;
        [SerializeField] private GameObject _buildings;
        [SerializeField] private Dictionary<Unit, Transform> _createdIcons = new Dictionary<Unit, Transform>();

        [Header("Prefabs")] 
        [SerializeField] private GameObject _iconPrefab;

        private void Awake()
        {
            _unitsManager.OnUnitCreated += HandleUnitCreation;
        }

        public void HandleUnitCreation(Unit p_unit)
        {
            GameObject newGo = null;
            HudIconRefs refs = null;

            if (p_unit is Vehicle vehicle)
            {
                if (vehicle.VehicleData.Type == VehicleType.Combat)
                {
                    newGo = Instantiate(_iconPrefab, _combatVehicles.transform);
                }
                else
                {
                    newGo = Instantiate(_iconPrefab, _utilityVehicles.transform);
                }

                refs = newGo.GetComponent<HudIconRefs>();
                refs.Icon.sprite = vehicle.VehicleData.Icon;
                refs.Button.onClick.AddListener(delegate { SelectUnit(vehicle); });
                _createdIcons.Add(p_unit, refs.transform);
                vehicle.OnVehicleClicked += SelectUnit;
                p_unit.OnUnitDied += RefreshIcons;
            }

            if (p_unit is Building building)
            {
                newGo = Instantiate(_iconPrefab, _buildings.transform);
                refs = newGo.GetComponent<HudIconRefs>();
                refs.Icon.sprite = building.BuildingData.Icon;
                refs.Button.onClick.AddListener(delegate { SelectUnit(building); });
                _createdIcons.Add(p_unit, refs.transform);
                building.OnUnitClicked += SelectUnit;
                p_unit.OnUnitDied += RefreshIcons;
            }
        }

        private void RefreshIcons(Unit p_units)
        {
            foreach (var icons in _createdIcons)
            {
                if (icons.Key != p_units)
                    continue;
                
                Destroy(icons.Value.gameObject);
            }
        }

        private void SelectUnit(Unit p_unit)
        {
            if (p_unit is Building building)
            {
                if (building.BuildingData.Type == BuildingType.Main_Base)
                {
                    // Open research window
                }
                else
                {
                    _rightDownPanelController.OpenPanel(PanelType.Building, building);
                }
            }
            else if (p_unit is Vehicle vehicle)
            {
                _unitsManager.SelectUnit(vehicle);
                _rightDownPanelController.OpenPanel(PanelType.Vehicle, vehicle);
            }
        }

        private void Update()
        {
        }
    }
}