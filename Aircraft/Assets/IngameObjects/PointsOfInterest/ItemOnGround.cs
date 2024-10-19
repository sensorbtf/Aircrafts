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
        private ResourceInUnit _itemToCollect;
        private bool _isMerging = false;

        public ResourceInUnit ItemToCollect => _itemToCollect;

        public event Action<ItemOnGround> OnItemClicked;

        internal void Initialize(ResourceInUnit p_item)
        {
            ObjectRenderer.sprite = p_item.Data.GetSpriteBasedOnAmount(p_item.CurrentAmount);

            _itemToCollect = p_item;
            _itemToCollect.CurrentAmount = p_item.CurrentAmount;

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
                        var resourceToGive = _itemToCollect.CurrentAmount;
                        var spaceLeft = collector.Inventory.GetFreeSpace(_itemToCollect.Data.Type);

                        if (resourceToGive >= spaceLeft)
                        {
                            _itemToCollect.CurrentAmount -= spaceLeft;
                            p_receiver.Inventory.AddResource(_itemToCollect.Data.Type, spaceLeft);
                        }
                        else
                        {
                            _itemToCollect.CurrentAmount -= resourceToGive;
                            p_receiver.Inventory.AddResource(_itemToCollect.Data.Type, resourceToGive);
                        }
                    }

                    break;
            }

            if (_itemToCollect.CurrentAmount <= 0)
            {
                Destroy(gameObject);
            }
        }

        public override void OnPointerClick(PointerEventData p_eventData)
        {
            OnItemClicked?.Invoke(this);
        }
        
        private void OnCollisionEnter2D(Collision2D p_collision)
        {
            if (_isMerging) 
                return;

            if (p_collision.gameObject.layer != LayerTagsManager.ItemsLayerIndex) 
                return;

            var itemOnGround = p_collision.gameObject.GetComponent<ItemOnGround>();

            if (itemOnGround.ItemToCollect.Data.Type != _itemToCollect.Data.Type) 
                return;

            itemOnGround.SetMergingFlag();

            JoinItem(itemOnGround);
            Destroy(itemOnGround.gameObject);
        }

        private void JoinItem(ItemOnGround p_itemOnGround)
        {
            _itemToCollect.CurrentAmount += p_itemOnGround.ItemToCollect.CurrentAmount;

            ObjectRenderer.sprite = _itemToCollect.Data.GetSpriteBasedOnAmount(_itemToCollect.CurrentAmount);
        }

        private void SetMergingFlag()
        {
            _isMerging = true;
        }
    }
}