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
        
        public InventoryController  CreateInventory(Unit p_unit) // need change in initial value
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