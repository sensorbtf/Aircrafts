using Buildings;
using TMPro;
using UnityEngine;

namespace UI.HUD
{
    public class BuildingPanel: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _buildingHeader;

        private void Start()
        {
            ClosePanel();
        }
        
        public void OpenPanel(Building p_building)
        {
            gameObject.SetActive(true);

            _buildingHeader.text = p_building.BuildingData.Type.ToString();
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}