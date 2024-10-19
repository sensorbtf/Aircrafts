using Buildings;
using Enemies;
using Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace UI.HUD
{
    public class ItemPanel: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private Image _itemIcon;

        private void Start()
        {
            ClosePanel();
        }
        
        public void OpenPanel(ItemOnGround p_item)
        {
            gameObject.SetActive(true);

            _itemName.text = $"{p_item.ItemToCollect.Data.Type} ({p_item.ItemToCollect.CurrentAmount})";
            _itemIcon.sprite = p_item.ItemToCollect.Data.GetSpriteBasedOnAmount(p_item.ItemToCollect.CurrentAmount);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}