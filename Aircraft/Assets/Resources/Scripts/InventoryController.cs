using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Resources.Scripts;
using UnityEngine;

namespace Resources
{
    public class InventoryController
    {
        private Dictionary<ResourceSO, int> _currentResources;
        
        public Dictionary<ResourceSO, int> CurrentResources => _currentResources;

        public Action<ResourceSO> OnResourceValueChanged;

        public InventoryController(ResourceSO[] p_specificResources)
        {
            _currentResources = new Dictionary<ResourceSO, int>();
            
            foreach (var resource in p_specificResources)
            {
                _currentResources.Add(resource, resource.InitialValue);
            } 
        }

        public bool IsEnoughResources(Resource p_resource, int p_amount)
        {
            return GetResourceAmount(p_resource) >= p_amount;
        }
        
        public void AddResource(Resource p_resource, int p_amount)
        {
            _currentResources[GetResourceSO(p_resource)] += p_amount;

            if (GetResourceAmount(p_resource) > 999)
            {
                _currentResources[GetResourceSO(p_resource)] = 999;
            }
            
            OnResourceValueChanged?.Invoke(GetResourceSO(p_resource));
        }
        
        public void RemoveResource(Resource p_resource, int p_amount)
        {
            _currentResources[GetResourceSO(p_resource)] -= p_amount;

            if (GetResourceAmount(p_resource) < 0)
            {
                _currentResources[GetResourceSO(p_resource)] = 0;
            }
            
            OnResourceValueChanged?.Invoke(GetResourceSO(p_resource));
        }
        
        public int GetResourceAmount(Resource p_resource)
        {
            var res = GetResourceSO(p_resource);
            return res != null ? _currentResources[res] : 0;
        }

        public int GetResourceAmount(ResourceSO p_resource)
        {
            return _currentResources[p_resource];
        }
        
        public ResourceSO GetResourceSO(Resource p_resource)
        {
            return _currentResources.FirstOrDefault(x => x.Key.Resource == p_resource).Key;
        }
    }
}