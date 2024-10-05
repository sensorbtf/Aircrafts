using Buildings;
using Enemies;
using Objects;
using UnityEngine;
using Objects.Vehicles;

namespace UI.HUD
{
    public class RightDownPanelController: MonoBehaviour
    {
        [SerializeField] private VehiclePanel _vehiclePanel;
        [SerializeField] private BuildingPanel _buildingPanel;
        [SerializeField] private EnemyPanel _enemyPanel;
        [SerializeField] private ItemPanel _itemPanel;

        private PanelType _currentPanelType;

        public void OpenPanel(PanelType p_panelType, Vehicle p_vehicle)
        {
            _currentPanelType = p_panelType;
            _vehiclePanel.OpenPanel(p_vehicle);
            _buildingPanel.ClosePanel();
            _enemyPanel.ClosePanel();
            _itemPanel.ClosePanel();
        }

        public void OpenPanel(PanelType p_panelType, Building p_building)
        {
            _currentPanelType = p_panelType;
            _buildingPanel.OpenPanel(p_building);
            _vehiclePanel.ClosePanel();
            _enemyPanel.ClosePanel();
            _itemPanel.ClosePanel();
        }
        
        public void OpenPanel(PanelType p_panelType, ItemOnGround p_item)
        {
            _currentPanelType = p_panelType;
            _itemPanel.OpenPanel(p_item);
            _vehiclePanel.ClosePanel();
            _buildingPanel.ClosePanel();
            _enemyPanel.ClosePanel();
        }
        
        public void OpenPanel(PanelType p_panelType, Enemy p_enemy)
        {
            _currentPanelType = p_panelType;
            _enemyPanel.OpenPanel(p_enemy);
            _vehiclePanel.ClosePanel();
            _buildingPanel.ClosePanel();
            _itemPanel.ClosePanel();
        }
    }

    public enum PanelType
    {
        Vehicle, Building, Enemy
    }
}