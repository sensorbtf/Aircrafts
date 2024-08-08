using System;
using TMPro;
using UnityEngine;
using Vehicles;

namespace UI.HUD
{
    public class VehiclePanel: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _vehicleHeader;

        private void Start()
        {
            ClosePanel();
        }

        public void OpenPanel(Vehicle p_selectedVehicle)
        {
            gameObject.SetActive(true);
            
            _vehicleHeader.text = p_selectedVehicle.VehicleData.Type.ToString();
        }
        
        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}