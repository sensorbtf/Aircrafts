using System;
using System.Collections.Generic;
using Buildings;
using Enemies;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;
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
            _unitsManager.OnUnitCreated += HandleUnitIconCreation;
        }

        private void OnDestroy()
        {
            _unitsManager.OnUnitCreated -= HandleUnitIconCreation;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                DeselectUnit();
            }
        }

        private void HandleUnitIconCreation(Unit p_unit)
        {
            GameObject newGo = null;
            HudIconRefs refs = null;

            if (p_unit is Vehicle vehicle)
            {
                newGo = Instantiate(_iconPrefab, vehicle.VehicleData.Type == VehicleType.Combat ? 
                    _combatVehicles.transform : _utilityVehicles.transform);

                refs = newGo.GetComponent<HudIconRefs>();
                refs.Icon.sprite = vehicle.VehicleData.Icon;
                refs.Button.navigation = new Navigation { mode = Navigation.Mode.None };
                refs.Button.onClick.AddListener(delegate
                {
                    SelectUnit(vehicle);
                });
                
                _createdIcons.Add(p_unit, refs.transform);
            }

            if (p_unit is Building building)
            {
                newGo = Instantiate(_iconPrefab, _buildings.transform);
                refs = newGo.GetComponent<HudIconRefs>();
                refs.Icon.sprite = building.BuildingData.Icon;
                refs.Button.navigation = new Navigation { mode = Navigation.Mode.None };
                refs.Button.onClick.AddListener(delegate
                {
                    SelectUnit(building);
                });
                
                _createdIcons.Add(p_unit, refs.transform);
            }
            
            if (p_unit is Enemy enemy)
            {
            }
            
            p_unit.OnUnitDied += RefreshIcons;
            p_unit.OnUnitClicked += SelectUnit;
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
            if (p_unit == null)
                return;

            if (p_unit.IsSelected)
            {
                _unitsManager.SelectUnit(_unitsManager.GetMainBase());
                return;
            }
            
            if (p_unit is Building building)
            {
                _unitsManager.SelectUnit(building);

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
                _unitsManager.SelectVehicle(vehicle);
                _rightDownPanelController.OpenPanel(PanelType.Vehicle, vehicle);
            }
            else if (p_unit is Enemy enemy)
            {
                _unitsManager.SelectUnit(enemy);

                _rightDownPanelController.OpenPanel(PanelType.Enemy, enemy);
            }
            
            EventSystem.current.SetSelectedGameObject(null);
        }

        private void DeselectUnit()
        {
            if (_unitsManager.SelectedUnit != null)
            {
                _unitsManager.SelectUnit(_unitsManager.GetMainBase());
            }
        }
    }
}