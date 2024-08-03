using UnityEngine;

[CreateAssetMenu(fileName = "PlayerActionsData", menuName = "Player/Actions", order = 3)]
public class PlayerActionsData : ScriptableObject
{
    [SerializeField] internal GameObject ProjectilePrefab; // Reference to the projectile prefab
    [SerializeField] internal float AttackRange = 5.0f; // Range to search for enemies
    [SerializeField] internal float ProjectileSpeed = 5.0f; // Speed of the projectile
    [SerializeField] internal float ShootCooldown = 8.0f; // Cooldown between shots
    [SerializeField] internal float RepairCooldown = 1; // Cooldown between shots
    [SerializeField] internal float EnergyRecoveringCooldown = 0.5f; // Cooldown for energy recovery
}

public enum RobotMode
{
    Builder = 0,
    Harvester = 1,
    Attacker = 2,
    Repairer = 3,
    Explorer = 4,
}