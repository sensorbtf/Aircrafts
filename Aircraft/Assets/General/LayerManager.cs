﻿using UnityEngine;

public static class LayerManager
{
    public static LayerMask VehicleLayer { get; private set; }
    public static LayerMask BuildingLayer { get; private set; }
    public static LayerMask ItemsLayer { get; private set; }
    public static LayerMask EnemyLayer { get; private set; } 
    public static LayerMask GroundLayer { get; private set; }
    public static LayerMask PointOfInterest { get; private set; }

    static LayerManager()
    {
        VehicleLayer = LayerMask.GetMask("Vehicles");
        BuildingLayer = LayerMask.GetMask("Buildings");
        EnemyLayer = LayerMask.GetMask("Enemies"); 
        GroundLayer = LayerMask.GetMask("Ground");
        ItemsLayer = LayerMask.GetMask("Items");
        PointOfInterest = LayerMask.GetMask("PoI"); 
    }
}