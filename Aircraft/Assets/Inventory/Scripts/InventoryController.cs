using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Objects;
using Resources.Scripts;
using UnityEngine;

namespace Resources
{
    public class InventoryController 
    {
        private GameObject _itemOnGround;
        private List<ResourceInUnit> _currentResources;

        public List<ResourceInUnit> CurrentResources => _currentResources;

        public Action<ResourceInUnit> OnResourceValueChanged;
        public Action<ItemOnGround> OnItemOnGroundMade;

        public InventoryController(ResourceInUnit[] p_specificResources, GameObject p_itemOnGround, bool p_isMain)
        {
            _itemOnGround = p_itemOnGround;

            _currentResources = new List<ResourceInUnit>();

            foreach (var resource in p_specificResources)
            {
                _currentResources.Add(new ResourceInUnit(resource.Data, resource.MaxAmount, p_isMain));
            }
        }

        public void AddResource(Resource p_resource, int p_amount)
        {
            var resInUnit = GetSpecificResource(p_resource);
            resInUnit.CurrentAmount += p_amount;

            if (resInUnit.CurrentAmount > resInUnit.MaxAmount)
            {
                resInUnit.CurrentAmount = resInUnit.MaxAmount;
            }

            EventsManager.Instance.MiscEvents.ResourceCollected(resInUnit);

            OnResourceValueChanged?.Invoke(resInUnit);
        }

        public void AddResource(ResourceSO p_resource, int p_amount)
        {
            var so = GetSpecificResource(p_resource.Type);
            so.CurrentAmount += p_amount;

            if (so.CurrentAmount > so.MaxAmount)
            {
                so.CurrentAmount = so.MaxAmount;
            }

            OnResourceValueChanged?.Invoke(so);
        }

        public void RemoveResource(Resource p_resource, int p_amount)
        {
            var so = GetSpecificResource(p_resource);

            so.CurrentAmount -= p_amount;

            if (so.CurrentAmount < 0)
            {
                so.CurrentAmount = 0;
            }

            OnResourceValueChanged?.Invoke(so);
        }

        public bool HaveAnyResources()
        {
            foreach (var resourceInUnit in _currentResources)
            {
                if (resourceInUnit.CurrentAmount > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetResourceAmount(Resource p_resource)
        {
            var res = GetSpecificResource(p_resource);
            return res?.CurrentAmount ?? 0;
        }

        public int GetResourceAmount(ResourceSO p_resource)
        {
            var res = GetSpecificResource(p_resource.Type);
            return res?.CurrentAmount ?? 0;
        }

        public int GetFreeSpace(Resource p_resource)
        {
            var res = GetSpecificResource(p_resource);
            return res.MaxAmount - res.CurrentAmount;
        }

        public ResourceInUnit GetSpecificResource(Resource p_resource)
        {
            return _currentResources.FirstOrDefault(x => x.Data.Type == p_resource);
        }
    }
}