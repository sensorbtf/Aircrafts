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
    public abstract class PointOfInterest: IngameObject
    {
        [Header("PointOfInterest")] 

        public Action<PointOfInterest, bool> OnPoIClicked;

        public void Initialize()
        {
            base.Initialize();
            CanvasInfo.HealthBar.gameObject.SetActive(false);
        }
    }
}