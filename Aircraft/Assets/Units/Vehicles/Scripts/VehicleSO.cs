using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleSO", menuName = "Vehicle/Data", order = 2)]
public class VehicleSO: UnitSO
{
    [SerializeField] private VehicleType _type;
    
    public VehicleType Type => _type;
}

public enum VehicleType
{
    Combat = 0,
    GeneralPurpose = 1,
    Transporter = 2,
    Underground = 3,
}

public enum WeaponType
{
    GatlingGun = 0,
}

[Serializable]
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