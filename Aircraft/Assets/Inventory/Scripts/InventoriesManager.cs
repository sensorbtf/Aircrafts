using System;
using System.Collections.Generic;
using Buildings;
using Resources.Scripts;
using Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources
{
    public class InventoriesManager : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<Unit, InventoryController> _inventories = new Dictionary<Unit, InventoryController>();

        [SerializeField] private ResourceDatabase _resourcesDatabase;
        [SerializeField] private InventoryController _mainInventory;
        [SerializeField] private GameObject _itemOnGround;

        public Dictionary<Unit, InventoryController> Inventories => _inventories;
        public ResourceDatabase ResourceDatabase => _resourcesDatabase;
        public InventoryController MainInventory => _mainInventory;

        public Action<ItemOnGround> OnItemOnGroundMade;

        public static InventoriesManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Game Events Manager in the scene.");
            }

            Instance = this;
        }

        public InventoryController CreateInventory(Unit p_unit) // need change in initial value
        {
            InventoryController newController;

            if (p_unit is Building building && building.BuildingData.Type == BuildingType.Base) // Main + others?
            {
                _mainInventory = newController = new InventoryController(p_unit.UnitData.Resources, _itemOnGround, true);
            }
            else
            {
                newController = new InventoryController(p_unit.UnitData.Resources, _itemOnGround, false);
            }

            _inventories.Add(p_unit, newController);
            return newController;
        }

        public void TryToDeleteInventory(Unit p_unit)
        {
            _inventories.Remove(p_unit);
        }

        public void CreateItemsOnDestroy(Unit p_unit)
        {
            foreach (var inventory in _inventories)
            {
                if (inventory.Key != p_unit)
                    continue;

                foreach (var item in inventory.Value.CurrentResources)
                {
                    var itemOnGround = Instantiate(_itemOnGround);
                    itemOnGround.transform.position = p_unit.transform.position;

                    var refs = itemOnGround.GetComponent<ItemOnGround>();

                    item.CurrentAmount = Random.Range(1, item.MaxAmount);
                    refs.Initialize(item);
                    OnItemOnGroundMade?.Invoke(refs);
                }

                break;
            }
        }

        public void CreateItem(ResourceInUnit p_resourceToCreate, Transform p_transform)
        {
            var itemOnGround = Instantiate(_itemOnGround);
            itemOnGround.transform.position = p_transform.position;

            var refs = itemOnGround.GetComponent<ItemOnGround>();

            refs.Initialize(p_resourceToCreate);
            OnItemOnGroundMade?.Invoke(refs);
        }
    }

    [Serializable]
    public class ResourceInUnit
    {
        private int _currentAmount;
        public ResourceSO Data;
        public int MaxAmount;
        [HideInInspector] public bool IsGlobalInventory;

        public int CurrentAmount
        {
            get => _currentAmount;
            set
            {
                _currentAmount = value;

                if (_currentAmount <= 3)
                {
                    
                }
                else if (_currentAmount is > 3 and <= 6)
                {
                    
                }
                else
                {
                    
                }
            }
        }

        public ResourceInUnit(ResourceSO p_data, int p_maxAmount, bool p_isGlobalInventory = false)
        {
            Data = p_data;
            CurrentAmount = p_data.InitialValue;
            MaxAmount = p_maxAmount;
            IsGlobalInventory = p_isGlobalInventory;
        }
    }
}