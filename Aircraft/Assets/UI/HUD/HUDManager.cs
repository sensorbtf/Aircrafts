using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Enemies;
using Resources;
using Resources.Scripts;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Units.Vehicles;
using UnityEngine.Serialization;

namespace UI.HUD
{
    public class HUDManager : MonoBehaviour
    {
        [Header("Panels")] [SerializeField] private RightDownPanelController _rightDownPanelController;

        [FormerlySerializedAs("_inventoryManager")]
        [Header("Managers")] 
        [SerializeField] private InventoriesManager _inventoriesManager;
        [SerializeField] private UnitsManager _unitsManager;
        [Header("Refs")] 
        [SerializeField] private Slider _fuelSlider;
        [SerializeField] private TextMeshProUGUI _fuelSliderText;
        [SerializeField] private GameObject _resourcesTab;
        [SerializeField] private GameObject _combatVehicles;
        [SerializeField] private GameObject _utilityVehicles;
        [SerializeField] private GameObject _buildings;
        
        private Dictionary<Unit, Transform> _createdIcons = new Dictionary<Unit, Transform>();
        private Dictionary<ResourceSO, HudIconRefs> _createdResources = new Dictionary<ResourceSO, HudIconRefs>();

        [Header("Prefabs")] [SerializeField] private GameObject _iconPrefab;

        public void CustomStart()
        {
            HandleResourcesCreation();
        }

        private void Awake()
        {
            _unitsManager.OnUnitCreated += HandleUnitIconCreation;
            _unitsManager.OnUnitSelected += SelectUnit;
            _inventoriesManager.OnGlobalResourceValueChanged += RefreshResourcesIcons;
        }

        private void OnDestroy()
        {
            _unitsManager.OnUnitCreated -= HandleUnitIconCreation;
            _unitsManager.OnUnitSelected -= SelectUnit;
            _inventoriesManager.OnGlobalResourceValueChanged -= RefreshResourcesIcons;
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
                newGo = Instantiate(_iconPrefab,
                    vehicle.VehicleData.Type == VehicleType.Combat
                        ? _combatVehicles.transform
                        : _utilityVehicles.transform);

                refs = newGo.GetComponent<HudIconRefs>();
                refs.Icon.sprite = vehicle.VehicleData.Icon;
                refs.Button.navigation = new Navigation { mode = Navigation.Mode.None };
                refs.Button.onClick.AddListener(delegate { _unitsManager.SelectUnit(vehicle, true); });

                _createdIcons.Add(p_unit, refs.transform);
            }
            else if (p_unit is Building building)
            {
                newGo = Instantiate(_iconPrefab, _buildings.transform);
                refs = newGo.GetComponent<HudIconRefs>();
                refs.Icon.sprite = building.BuildingData.Icon;
                refs.Button.navigation = new Navigation { mode = Navigation.Mode.None };
                refs.Button.onClick.AddListener(delegate { _unitsManager.SelectUnit(building, true); });

                _createdIcons.Add(p_unit, refs.transform);
            }
            else if (p_unit is Enemy enemy)
            {
            }

            p_unit.OnUnitDied += RefreshUnitsIcons;
        }

        private void HandleResourcesCreation()
        {
            foreach (var resource in _inventoriesManager.ResourceDatabase.Resources)
            {
                var newGo = Instantiate(_iconPrefab, _resourcesTab.transform);

                var refs = newGo.GetComponent<HudIconRefs>();
                refs.Icon.sprite = resource.Icon;
                refs.Text.text = resource.ToString();

                _createdResources.Add(resource, refs);
            }
        }

        private void RefreshResourcesIcons(ResourceSO p_resource)
        {
            foreach (var icon in _createdResources)
            {
                if (icon.Key == p_resource)
                {
                    icon.Value.Text.text = _inventoriesManager.GetResourceAmountFromMainInventory(p_resource).ToString();
                    return;
                }
            }
        }

        private void RefreshUnitsIcons(Unit p_units)
        {
            foreach (var icons in _createdIcons.ToList())
            {
                if (icons.Key != p_units)
                    continue;

                Destroy(icons.Value.gameObject);
                _createdIcons.Remove(icons.Key);
            }
        }

        private void SelectUnit(Unit p_unit)
        {
            if (p_unit == null)
                return;

            if (p_unit is Building building)
            {
                _unitsManager.SelectUnit(building);

                if (building.BuildingData.Type == BuildingType.Main_Base)
                {
                    _rightDownPanelController.OpenPanel(PanelType.Building, building);
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
                _unitsManager.SelectUnit(_unitsManager.GetMainBase(), true);
            }
        }
    }
}