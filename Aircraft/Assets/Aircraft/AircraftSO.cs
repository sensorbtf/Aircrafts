using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aircraft_SO", menuName = "Aircraft/Data", order = 2)]
public class AircraftSO : ScriptableObject
{
    public Sprite Icon;
    public Sprite IngameModel;
    public AircraftType Type;
    public Weapon[] WeaponsType;
    public int MaxHp;
    public int Speed;
    public int Maneuverability;
}

public enum AircraftType
{
    First = 0,
}

public enum WeaponType
{
    GatlingGun = 0,
}

public class Weapon
{
    public WeaponType Type;
    public int CurrentAmmo;
    public int MaxAmmo;

    public Weapon(WeaponType p_type, int p_maxAmmo, int p_currentAmmo)
    {
        Type = p_type;
        MaxAmmo = p_maxAmmo;
        CurrentAmmo = p_currentAmmo;
    }
}