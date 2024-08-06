using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vehicles;

public class HUD : MonoBehaviour
{
    [SerializeField] private VehiclesManager VehiclesManager;
    [SerializeField] private Slider _fuelSlider;
    [SerializeField] private TextMeshProUGUI _fuelSliderText;
    [SerializeField] private GameObject _combatVehicles;
    [SerializeField] private GameObject _utilityVehicles;
    
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

            var refs = newGo.GetComponent<VehicleIconRefs>();
            refs.Icon.sprite = vehicle.VehicleData.Icon;
            refs.Button.onClick.AddListener(delegate { SelectVehicle(vehicle);}); 
        }
    }

    private void SelectVehicle(Vehicle p_vehicle)
    {
        VehiclesManager.SelectVehicle(p_vehicle);
    }

    private void Update()
    {
    }
}
