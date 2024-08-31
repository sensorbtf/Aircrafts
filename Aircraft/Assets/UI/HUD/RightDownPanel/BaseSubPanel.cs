using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Units;
using UnityEngine;

public class BaseSubPanel : MonoBehaviour
{
    [SerializeField] private BaseUiRefs _baseUiRefs;
    private BaseBuilding _baseBuilding;
    private UnitsManager _unitsManager;

    private void Start()
    {
        _unitsManager = FindObjectOfType<UnitsManager>();
    }

    public void CustomStart(BaseBuilding p_baseBuilding)
    {
        _baseBuilding = p_baseBuilding;

        foreach (var vehicle in _baseBuilding.VehiclesInBase)
        {
            var vehicleUi = _baseUiRefs.Vehicles.FirstOrDefault(x => x.Vehicle == null);
            vehicleUi.Vehicle = vehicle;
            vehicleUi.Icon.sprite = vehicle.VehicleData.Icon;
            vehicleUi.Button.onClick.RemoveAllListeners();
            vehicleUi.Button.onClick.AddListener(delegate { _unitsManager.SelectUnit(vehicle, true); });
        }
        
        gameObject.SetActive(true);
    }
    
    public void OnPanelClose()
    {
        _baseBuilding = null;
        gameObject.SetActive(false);
    }
}