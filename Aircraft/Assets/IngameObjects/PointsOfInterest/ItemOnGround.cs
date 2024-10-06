using Buildings;
using Objects.Vehicles;
using Resources;
using Resources.Scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

namespace Objects
{
    public class ItemOnGround : IngameObject
    {
        private ResourceInUnit _item;
        
        public ResourceInUnit Item => _item;

        public event Action<ItemOnGround> OnItemClicked; 

        internal void Initialize(ResourceInUnit p_item)
        {
            ObjectRenderer.sprite = p_item.Data.Icon;

            _item = p_item;
            _item.CurrentAmount = p_item.MaxAmount;

            Initialize();
            CanvasInfo.HealthBar.gameObject.SetActive(false);
        }

        public override void CheckState()
        {
            SetNewStateTexts(Actions.Collect);
        }

        public override void MakeAction(Actions p_actionType, IngameObject p_giver, IngameObject p_receiver)
        {
            switch (p_actionType)
            {
                case Actions.Collect:
                    if (p_receiver is Vehicle collector)
                    {
                        var resourceToGive = _item.CurrentAmount;
                        var spaceLeft = collector.Inventory.GetFreeSpace(_item.Data.Type);

                        if (resourceToGive >= spaceLeft)
                        {
                            _item.CurrentAmount -= spaceLeft;
                            p_receiver.Inventory.AddResource(_item.Data.Type, spaceLeft);
                        }
                        else
                        {
                            _item.CurrentAmount -= resourceToGive;
                            p_receiver.Inventory.AddResource(_item.Data.Type, resourceToGive);
                        }
                    }

                    break;
            }

            if (_item.CurrentAmount <= 0)
            {
                Destroy(gameObject);
            }
        }

        public override void OnPointerClick(PointerEventData p_eventData)
        {
            OnItemClicked?.Invoke(this);
        }
    }
}
