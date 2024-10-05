using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Enemies;
using Resources;
using Resources.Scripts;
using TMPro;
using Objects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Objects.Vehicles;
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
        [SerializeField] private GameObject _bases;
        
        private Dictionary<BaseBuilding, BaseUiRefs> _createdBaseIcons = new Dictionary<BaseBuilding, BaseUiRefs>();
        private Dictionary<ResourceSO, HudIconRefs> _createdResources = new Dictionary<ResourceSO, HudIconRefs>();

        [Header("Prefabs")] [SerializeField] private GameObject _iconPrefab;
        [SerializeField] private GameObject _basePrefab;

        public void CustomStart()
        {
            HandleResourcesCreation();
            
            _inventoriesManager.MainInventory.OnResourceValueChanged += RefreshResourcesIcons;
        }

        private void Awake()
        {
            _unitsManager.OnUnitCreated += HandleUnitIconCreation;
            _unitsManager.OnUnitSelected += SelectUnit;
            _inventoriesManager.OnItemOnGroundMade += ListenToItem;
        }

        private void ListenToItem(ItemOnGround p_item)
        {
            p_item.OnItemClicked += SelectItem;
        }

        private void OnDestroy()
        {
            _unitsManager.OnUnitCreated -= HandleUnitIconCreation;
            _unitsManager.OnUnitSelected -= SelectUnit;
            _inventoriesManager.OnItemOnGroundMade -= ListenToItem;
            _inventoriesManager.MainInventory.OnResourceValueChanged -= RefreshResourcesIcons;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                DeselectUnit();
            }

            foreach (var refs in _createdBaseIcons)
            {
                foreach (var vehicle in refs.Value.Vehicles)
                {
                    if (vehicle.Vehicle == null)
                        continue;

                    if (vehicle.Vehicle.IsInBase)
                    {
                        vehicle.Icon.color = Color.gray;
                    }
                    else
                    {
                        vehicle.Icon.color = Color.white;
                    }
                }
            }
        }

        private void HandleUnitIconCreation(Unit p_unit)
        {
            if (p_unit is Vehicle vehicle)
            {
                var specificBase = _unitsManager.GetBaseOfVehicle(vehicle);

                foreach (var baseIcon in _createdBaseIcons)
                {
                    if (baseIcon.Key != specificBase) 
                        continue;
                    
                    foreach (var vehicleUI in baseIcon.Value.Vehicles)
                    {
                        if (vehicleUI.Vehicle != null)
                            continue;
                        
                        vehicleUI.Vehicle = vehicle;
                        vehicleUI.Icon.sprite = vehicle.VehicleData.Icon;
                        vehicleUI.Button.navigation = new Navigation { mode = Navigation.Mode.None };
                        vehicleUI.Button.onClick.RemoveAllListeners();
                        vehicleUI.Button.onClick.AddListener(delegate { _unitsManager.SelectUnit(vehicle, true); });
                        break;
                    }
                }
                
            }
            else if (p_unit is Building building)
            {
                if (building is BaseBuilding newBase)
                {
                    var newGo = Instantiate(_basePrefab, _bases.transform);
                    var baseRefs = newGo.GetComponent<BaseUiRefs>();
                    baseRefs.BaseIcon.sprite = newBase.BuildingData.Icon;
                    _createdBaseIcons.Add(newBase, baseRefs);
                }
            }
            else if (p_unit is Enemy)
            {
                return; // or map
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
                refs.Text.text = _inventoriesManager.MainInventory.GetResourceAmount(resource.Type).ToString();

                _createdResources.Add(resource, refs);
            }
        }

        private void RefreshResourcesIcons(ResourceInUnit p_resourceInUnit)
        {
            if (p_resourceInUnit.IsGlobalInventory)
            {
                foreach (var icon in _createdResources)
                {
                    if (icon.Key == p_resourceInUnit.Data)
                    {
                        icon.Value.Text.text = _inventoriesManager.MainInventory.GetResourceAmount(p_resourceInUnit.Data.Type).ToString();
                        return;
                    }
                }
            }
        }

        private void RefreshUnitsIcons(Unit p_unit)
        {
            if (p_unit is Vehicle vehicle)
            {
                foreach (var baseIcon in _createdBaseIcons)
                {
                    if (baseIcon.Key != _unitsManager.GetBaseOfVehicle(vehicle)) 
                        continue;
                    
                    foreach (var vehicleUI in baseIcon.Value.Vehicles)
                    {
                        if (vehicleUI.Vehicle != vehicle)
                            continue;
                        
                        vehicleUI.Icon.sprite = null;
                        vehicleUI.Vehicle = null;
                        vehicleUI.Button.navigation = new Navigation { mode = Navigation.Mode.None };
                        vehicleUI.Button.onClick.RemoveAllListeners();
                    }
                }  
            }
            else if (p_unit is BaseBuilding baseBuilding)
            {
                foreach (var baseIcon in _createdBaseIcons.ToList())
                {
                    if (baseIcon.Key != baseBuilding) 
                        continue;
                    
                    foreach (var vehicleUI in baseIcon.Value.Vehicles)
                    {
                        if (vehicleUI.Vehicle == null)
                            continue;
                        
                        vehicleUI.Vehicle.OnBaseExit();
                    }

                    Destroy(baseIcon.Value.gameObject);
                    _createdBaseIcons.Remove(baseBuilding);
                }  
            }
        }
        
        private void SelectItem(ItemOnGround p_item)
        {
            if (p_item == null)
                return;
            
            _rightDownPanelController.OpenPanel(PanelType.Item, p_item);
        }
        
        private void SelectUnit(Unit p_unit)
        {
            if (p_unit == null)
                return;

            if (p_unit is Building building)
            {
                _unitsManager.SelectUnit(building);

                if (building.BuildingData.Type == BuildingType.Base)
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
                _unitsManager.SelectUnit(vehicle);
                _rightDownPanelController.OpenPanel(PanelType.Vehicle, vehicle);
            }
            else if (p_unit is Enemy enemy)
            {
                //_unitsManager.SelectUnit(enemy);

                //_rightDownPanelController.OpenPanel(PanelType.Enemy, enemy);
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