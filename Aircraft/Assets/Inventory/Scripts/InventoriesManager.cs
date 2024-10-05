using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Resources.Scripts;
using Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Resources
{
    public class InventoriesManager : MonoBehaviour
    {
        [SerializeField] private Dictionary<Unit, InventoryController> _inventories = new Dictionary<Unit, InventoryController>();
        [SerializeField] private ResourceDatabase _resourcesDatabase;
        [SerializeField] private InventoryController _mainInventory;
        [SerializeField] private GameObject _itemOnGround;
        
        public Dictionary<Unit, InventoryController> Inventories => _inventories;
        public ResourceDatabase ResourceDatabase => _resourcesDatabase;
        public InventoryController MainInventory => _mainInventory;
        
        public Action<ItemOnGround> OnItemOnGroundMade;
        
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
            newController.OnItemOnGroundMade += InvokeItemCreation;
            return newController;
        }

        private void InvokeItemCreation(ItemOnGround p_item)
        {
            OnItemOnGroundMade?.Invoke(p_item);
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
                    itemOnGround.transform.position = new Vector3(
                        p_unit.transform.position.x + UnityEngine.Random.Range(-2, 2),
                        p_unit.transform.position.y, p_unit.transform.position.z);

                    var refs = itemOnGround.GetComponent<ItemOnGround>();
                    
                    refs.Initialize(item);
                    OnItemOnGroundMade?.Invoke(refs);
                }
                break;
            }
        }
    }
    
    [Serializable]
    public class ResourceInUnit
    {
        public ResourceSO Data;
        [HideInInspector] public int CurrentAmount;
        public int MaxAmount;
        [HideInInspector] public bool IsGlobalInventory;

        public ResourceInUnit(ResourceSO p_data, int p_maxAmount, bool p_isGlobalInventory = false)
        {
            Data = p_data;
            CurrentAmount = p_data.InitialValue;
            MaxAmount = p_maxAmount;
            IsGlobalInventory = p_isGlobalInventory;
        }
    }
}