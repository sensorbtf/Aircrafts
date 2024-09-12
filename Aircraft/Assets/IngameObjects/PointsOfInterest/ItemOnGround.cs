using Resources;
using Resources.Scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Objects
{
    public class ItemOnGround: IngameObject
    {
        private int _currentAmount;
        private Transform _transform;


        internal void Initialize(ResourceInUnit p_item)
        {
            ObjectRenderer.sprite = p_item.Data.Icon;

            _currentAmount = p_item.CurrentAmount;

            Initialize();
            CanvasInfo.HealthBar.gameObject.SetActive(false);
            SetNewStateTexts(Actions.Collect);
        }

        public override void OnPointerClick(PointerEventData p_eventData)
        {

        }

        public override void CheckState()
        {

        }
    }
}
