using Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vehicles;
namespace UI.HUD
{
    public class HUDManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private RightDownPanelController _rightDownPanelController;
        
        [SerializeField] private VehiclesManager VehiclesManager;
        [SerializeField] private BuildingsManager BuildingsManager;
        [SerializeField] private Slider _fuelSlider;
        [SerializeField] private TextMeshProUGUI _fuelSliderText;
        [SerializeField] private GameObject _combatVehicles;
        [SerializeField] private GameObject _utilityVehicles;
        [SerializeField] private GameObject _buildings;

        [Header("Prefabs")] 
        [SerializeField] private GameObject _vehicleIconPrefab;

        public void CustomStart()
        {
            foreach (var vehicle in VehiclesManager.AllVehicles)
            {
                GameObject newGo = null;

                if (vehicle.VehicleData.Type == VehicleType.Combat)
                {
                    newGo = Instantiate(_vehicleIconPrefab, _combatVehicles.transform);
                }
                else
                {
                    newGo = Instantiate(_vehicleIconPrefab, _utilityVehicles.transform);
                }

                var refs = newGo.GetComponent<HudIconRefs>();
                refs.Icon.sprite = vehicle.VehicleData.Icon;
                refs.Button.onClick.AddListener(delegate { SelectVehicle(vehicle); });
                vehicle.OnVehicleClicked += SelectVehicle;
            }
            
            foreach (var building in BuildingsManager.Buildings)
            {
                var newGo = Instantiate(_vehicleIconPrefab, _buildings.transform);
                var refs = newGo.GetComponent<HudIconRefs>();
                refs.Icon.sprite = building.BuildingData.Icon;
                refs.Button.onClick.AddListener(delegate { SelectBuilding(building); });
                building.OnBuildingClicked += SelectBuilding;
            }
        }

        private void SelectVehicle(Vehicle p_vehicle)
        {
            VehiclesManager.SelectVehicle(p_vehicle);
            _rightDownPanelController.OpenPanel(PanelType.Vehicle, p_vehicle);
        }
        
        private void SelectBuilding(Building p_building)
        {
            if (p_building.BuildingData.Type == BuildingType.Main_Base)
            {
                // Open research window
            }
            else
            {
                _rightDownPanelController.OpenPanel(PanelType.Building, p_building);
            }
        }

        private void Update()
        {
        }
    }
}