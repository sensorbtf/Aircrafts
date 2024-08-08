using Buildings;
using UnityEngine;
using Vehicles;

namespace UI.HUD
{
    public class RightDownPanelController: MonoBehaviour
    {
        [SerializeField] private VehiclePanel _vehiclePanel;
        [SerializeField] private BuildingPanel _buildingPanel;

        private PanelType _currentPanelType;

        public void OpenPanel(PanelType p_panelType, Vehicle p_vehicle)
        {
            _currentPanelType = p_panelType;
            _vehiclePanel.OpenPanel(p_vehicle);
            _buildingPanel.ClosePanel();
        }

        public void OpenPanel(PanelType p_panelType, Building p_building)
        {
            _currentPanelType = p_panelType;
            _buildingPanel.OpenPanel(p_building);
            _vehiclePanel.ClosePanel();
        }
    }

    public enum PanelType
    {
        Vehicle, Building
    }
}