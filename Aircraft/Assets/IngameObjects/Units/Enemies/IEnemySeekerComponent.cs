using System;
using UnityEngine;

namespace Enemies
{
    public interface IEnemySeekerComponent
    {
        public float InteractionRange { get;}
        // priority = building
    }
}