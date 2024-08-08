﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buildings
{
    public abstract class Building: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private BuildingSO _buildingData;

        public BuildingSO BuildingData => _buildingData;
        
        internal Action<Building> OnBuildingClicked;
        
        public void OnPointerClick(PointerEventData p_eventData)
        {
            OnBuildingClicked?.Invoke(this);
        }
    }
}