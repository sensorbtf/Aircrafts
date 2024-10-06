using Buildings;
using Enemies;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.HUD
{
    public class ItemPanel: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;

        private void Start()
        {
            ClosePanel();
        }
        
        public void OpenPanel(ItemOnGround p_item)
        {
            gameObject.SetActive(true);

            _itemName.text = $"{p_item.Item.Data.Type} ({p_item.Item.CurrentAmount})";
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}