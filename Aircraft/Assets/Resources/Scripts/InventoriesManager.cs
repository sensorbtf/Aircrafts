using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using Resources.Scripts;
using Units;
using UnityEngine;

namespace Resources
{
    public class InventoriesManager : MonoBehaviour
    {
        [SerializeField] private Dictionary<Unit, InventoryController> _inventories = new Dictionary<Unit, InventoryController>();
        [SerializeField] private ResourceDatabase _resourcesDatabase;
        [SerializeField] private InventoryController _mainInventory;
        
        public Dictionary<Unit, InventoryController> Inventories => _inventories;
        public ResourceDatabase ResourceDatabase => _resourcesDatabase;
        public InventoryController MainInventory => _mainInventory;

        public Action<ResourceSO> OnGlobalResourceValueChanged;
        
        public InventoryController  CreateInventory(Unit p_unit) // need change in initial value
        {
            var newController = new InventoryController(_resourcesDatabase.Resources
                .Where(x => p_unit.UnitData.Resources.Contains(x.Type)).ToArray());
            
            if (p_unit is Building building && building.BuildingData.Type == BuildingType.Base) // Main + others?
            {
                _mainInventory = newController;
            }
            
            _inventories.Add(p_unit, newController);
            return newController;
        }
        
        public int GetResourceAmount(Unit p_unit, Resource p_resource)
        {
            var res = GetResourceSO(p_unit, p_resource);
            return res != null ? _inventories[p_unit].CurrentResources[res] : 0;
        }        
        
        public int GetResourceAmountFromMainInventory(ResourceSO p_resource)
        {
            return _mainInventory.CurrentResources.GetValueOrDefault(p_resource, 0);
        }
        
        public int GetResourceAmount(Unit p_unit, ResourceSO p_resource)
        {
            return _inventories[p_unit].CurrentResources[p_resource];
        }
        
        public bool IsEnoughResources(Unit p_unitInventory, Resource p_resource, int p_amount)
        {
            return GetResourceAmount(p_unitInventory, p_resource) >= p_amount;
        }
        
        public void AddResource(Unit p_unitInventory, Resource p_resource, int p_amount)
        {
            _inventories[p_unitInventory].CurrentResources[GetResourceSO(p_unitInventory, p_resource)] += p_amount;

            if (GetResourceAmount(p_unitInventory, p_resource) > 999)
            {
                _inventories[p_unitInventory].CurrentResources[GetResourceSO(p_unitInventory, p_resource)] = 999;
            }

            if (_inventories[p_unitInventory] == _mainInventory)
            {
                OnGlobalResourceValueChanged?.Invoke(GetResourceSO(p_unitInventory, p_resource));
            }
        }
        
        public void RemoveResource(Unit p_unitInventory, Resource p_resource, int p_amount)
        {
            _inventories[p_unitInventory].CurrentResources[GetResourceSO(p_unitInventory, p_resource)] -= p_amount;

            if (GetResourceAmount(p_unitInventory, p_resource) < 0)
            {
                _inventories[p_unitInventory].CurrentResources[GetResourceSO(p_unitInventory, p_resource)] = 0;
            }
            
            if (_inventories[p_unitInventory] == _mainInventory)
            {
                OnGlobalResourceValueChanged?.Invoke(GetResourceSO(p_unitInventory, p_resource));
            }
        }

        public ResourceSO GetResourceSO(Unit p_unitInventory, Resource p_resource)
        {
            return _inventories[p_unitInventory].CurrentResources.FirstOrDefault(x => x.Key.Resource == p_resource).Key;
        }
    }
}