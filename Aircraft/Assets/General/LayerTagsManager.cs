using UnityEngine;

public static class LayerTagsManager
{
    public static LayerMask VehicleLayer { get; private set; }
    public static LayerMask BuildingLayer { get; private set; }
    public static LayerMask ItemsLayer { get; private set; }
    public static LayerMask EnemyLayer { get; private set; } 
    public static LayerMask GroundLayer { get; private set; }
    public static LayerMask PointOfInterest { get; private set; }

    public static int VehicleLayerIndex { get; private set; }
    public static int BuildingLayerIndex { get; private set; }
    public static int ItemsLayerIndex { get; private set; }
    public static int EnemyLayerIndex { get; private set; }
    public static int GroundLayerIndex { get; private set; }
    public static int PointOfInterestIndex { get; private set; }
    
    
    public static string VehicleTag { get; private set; }
    public static string BuildingTag { get; private set; }
    public static string GroundTag { get; private set; }
    public static string EnemyTag { get; private set; }

    static LayerTagsManager()
    {
        BuildingTag = "Building";
        VehicleTag = "Vehicle";
        GroundTag = "Ground";
        EnemyTag = "Enemy";
        
        VehicleLayer = LayerMask.GetMask("Vehicles");
        BuildingLayer = LayerMask.GetMask("Buildings");
        EnemyLayer = LayerMask.GetMask("Enemies"); 
        GroundLayer = LayerMask.GetMask("Ground");
        ItemsLayer = LayerMask.GetMask("Items");
        PointOfInterest = LayerMask.GetMask("PoI");

        VehicleLayerIndex = LayerMask.NameToLayer("Vehicles");
        BuildingLayerIndex = LayerMask.NameToLayer("Buildings");
        EnemyLayerIndex = LayerMask.NameToLayer("Enemies");
        GroundLayerIndex = LayerMask.NameToLayer("Ground");
        ItemsLayerIndex = LayerMask.NameToLayer("Items");
        PointOfInterestIndex = LayerMask.NameToLayer("PoI");
    }
}