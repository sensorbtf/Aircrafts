using Buildings;
using TMPro;
using UnityEngine;

namespace UI.HUD
{
    public class BuildingPanel: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _buildingHeader;
        [SerializeField] private BaseSubPanel _mainBaseSubPanel;
        [SerializeField] private ProductionSubpanel _productionSubpanel;

        private void Start()
        {
            ClosePanel();
        }
        
        public void OpenPanel(Building p_building)
        {
            gameObject.SetActive(true);
            _buildingHeader.text = p_building.BuildingData.Type.ToString();

            if (p_building is BaseBuilding baseBuilding)
            {
                _mainBaseSubPanel.gameObject.SetActive(true);
                _productionSubpanel.OnPanelClose();

                _mainBaseSubPanel.CustomStart(baseBuilding);
            }
            else if (p_building is ProductionBuilding productionBuilding)
            {
                _mainBaseSubPanel.OnPanelClose();
                _productionSubpanel.CustomStart(productionBuilding);
            }
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
            _mainBaseSubPanel.OnPanelClose();
            _productionSubpanel.OnPanelClose();
        }
    }
}