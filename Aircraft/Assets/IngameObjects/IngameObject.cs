using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Resources;
using Resources.Scripts;
using Objects.Vehicles;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Objects
{
    public abstract class IngameObject : MonoBehaviour, IPointerClickHandler
    {
        private SpriteRenderer _unitRenderer;
        private Collider2D _unitCollider;
        private InventoryController _inventory;
        public InfoCanvasRefs CanvasInfo;

        protected List<Unit> _unitsInRange = new List<Unit>();
        private bool _isSelected;

        public bool IsSelected => _isSelected;
        public SpriteRenderer UnitRenderer => _unitRenderer;
        public Collider2D UnitCollider => _unitCollider;
        public InventoryController Inventory => _inventory;

        public void Initialize()
        {
            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                CanvasInfo.StateInfo[i].TextInfo.text = "";
            }

            _unitRenderer = gameObject.GetComponent<SpriteRenderer>();
            _unitCollider = gameObject.GetComponent<Collider2D>();

            // Rigidbody2D.drag = EnemyData.Drag;  // Adjust drag to control sliding
            // Rigidbody2D.angularDrag = EnemyData.AngularDrag;  // Control rotational drag
        }

        public void PostInitialize(InventoryController p_newInventory)
        {
            _inventory = p_newInventory;
        }

        public virtual void Update() // used for things needed to check constantly
        {
            CheckState();
        }

        public virtual void SelectedFixedUpdate()
        {
            if (!_isSelected)
                return;
        }

        public virtual void SelectedUpdate()
        {
            if (!_isSelected)
                return;
        }

        public virtual void SelectObject()
        {
            _isSelected = true;
            Reorder(true);
        }

        public virtual void UnSelectObject()
        {
            _isSelected = false;
            Reorder(false);

            foreach (var unit in _unitsInRange)
            {
                unit.ResetStateButtons();
            }

            // make AI logic
        }

        private void Reorder(bool p_hide)
        {
            if (p_hide)
            {
                UnitRenderer.sortingOrder = 2;
            }
            else
            {
                UnitRenderer.sortingOrder = 1;
            }
        }

        #region Actions

        public void TryToActivateStateButtons(Actions p_actionType, Unit p_giver)
        {
            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                if (CanvasInfo.StateInfo[i].Action == p_actionType)
                {
                    SetAction(p_actionType, p_giver, i);
                    CompactStateInfo();
                    return;
                }
            }
        }

        public void TryToActivateStateButtons(Actions p_actionType, Unit p_giver, Unit p_receiver)
        {
            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                if (CanvasInfo.StateInfo[i].Action == p_actionType)
                {
                    SetAction(p_actionType, p_giver, i, p_receiver);
                    CompactStateInfo();
                    return;
                }
            }
        }

        private void SetAction(Actions p_actionType, Unit p_giver, int p_index, Unit p_receiver = null)
        {
            CanvasInfo.Reorder(true);

            CanvasInfo.StateInfo[p_index].TextInfo.text = p_actionType.ToString();

            CanvasInfo.StateInfo[p_index].Button.interactable = true;
            CanvasInfo.StateInfo[p_index].Button.onClick.RemoveAllListeners();
            CanvasInfo.StateInfo[p_index].Button.onClick.AddListener(delegate
            {
                if (p_receiver == null)
                {
                    MakeAction(p_actionType, p_giver, this);
                }
                else
                {
                    MakeAction(p_actionType, p_giver, p_receiver);
                }
            });
        }

        public virtual void MakeAction(Actions p_actionType, IngameObject p_giver, IngameObject p_receiver)
        {

        }

        protected List<Unit> GetNearbyUnits(LayerMask[] p_unitLayerMasks, float p_checkRange)
        {
            LayerMask combinedLayerMask = 0;
            foreach (var layerMask in p_unitLayerMasks)
            {
                combinedLayerMask |= layerMask;
            }

            var nearbyUnits = new List<Unit>();
            var colliders = Physics2D.OverlapCircleAll(transform.position, p_checkRange, combinedLayerMask);

            foreach (var collider in colliders)
            {
                var nearbyUnit = collider.GetComponent<Unit>();

                if (nearbyUnit != null && !Equals(nearbyUnit, this))
                {
                    nearbyUnits.Add(nearbyUnit);

                    if (!_unitsInRange.Contains(nearbyUnit))
                    {
                        _unitsInRange.Add(nearbyUnit);
                    }
                }
            }

            for (int i = _unitsInRange.Count - 1; i >= 0; i--)
            {
                if (!nearbyUnits.Contains(_unitsInRange[i]))
                {
                    _unitsInRange[i].ResetStateButtons();
                    _unitsInRange.RemoveAt(i);
                }
            }

            return nearbyUnits;
        }

        protected void SetNewStateTexts(Actions p_actionType)
        {
            CompactStateInfo();

            if (CanvasInfo.StateInfo.Any(x => x.Action == p_actionType))
                return;

            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                if (string.IsNullOrEmpty(CanvasInfo.StateInfo[i].TextInfo.text))
                {
                    CanvasInfo.StateInfo[i].Button.interactable = false;
                    CanvasInfo.StateInfo[i].TextInfo.text = p_actionType.ToString();
                    CanvasInfo.StateInfo[i].Action = p_actionType;
                    return;
                }
            }

            CanvasInfo.StateInfo[2].Button.interactable = false;
            CanvasInfo.StateInfo[2].TextInfo.text = p_actionType.ToString();
            CanvasInfo.StateInfo[2].Action = p_actionType;
        }

        protected void CompactStateInfo()
        {
            int targetIndex = 0;

            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                if (!string.IsNullOrEmpty(CanvasInfo.StateInfo[i].TextInfo.text))
                {
                    if (targetIndex != i)
                    {
                        CanvasInfo.StateInfo[targetIndex].TextInfo.text = CanvasInfo.StateInfo[i].TextInfo.text;
                        CanvasInfo.StateInfo[targetIndex].Action = CanvasInfo.StateInfo[i].Action;
                        CanvasInfo.StateInfo[targetIndex].Button.interactable =
                            CanvasInfo.StateInfo[i].Button.interactable;

                        int capturedIndex = i;
                        CanvasInfo.StateInfo[targetIndex].Button.onClick.RemoveAllListeners();
                        CanvasInfo.StateInfo[targetIndex].Button.onClick.AddListener(() =>
                        {
                            MakeAction(CanvasInfo.StateInfo[capturedIndex].Action, this, this);
                        });

                        CanvasInfo.StateInfo[i].TextInfo.text = "";
                        CanvasInfo.StateInfo[i].Action = Actions.Noone;
                        CanvasInfo.StateInfo[i].Button.interactable = false;
                        CanvasInfo.StateInfo[i].Button.onClick.RemoveAllListeners();
                    }

                    targetIndex++;
                }
            }
        }

        protected void ResetStateText(Actions p_previousAction)
        {
            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                if (CanvasInfo.StateInfo[i].Action == p_previousAction)
                {
                    CanvasInfo.StateInfo[i].TextInfo.text = "";
                    CanvasInfo.StateInfo[i].Button.interactable = false;
                    CanvasInfo.StateInfo[i].Action = Actions.Noone;
                    CompactStateInfo();
                    return;
                }
            }
        }

        private void ResetStateButtons()
        {
            CanvasInfo.Reorder(false);

            for (int i = 0; i < CanvasInfo.StateInfo.Length; i++)
            {
                CanvasInfo.StateInfo[i].Action = Actions.Noone;
                CanvasInfo.StateInfo[i].Button.interactable = false;
                CanvasInfo.StateInfo[i].Button.onClick.RemoveAllListeners();
                CanvasInfo.StateInfo[i].TextInfo.text = "";
            }
        }

        #endregion

        public abstract void CheckState();
        public abstract void OnPointerClick(PointerEventData p_eventData);
    }
}