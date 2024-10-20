using System;
using System.Collections;
using System.Collections.Generic;
using Resources.Scripts;
using Objects;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleSO", menuName = "Vehicle/Data", order = 2)]
public class VehicleSO : UnitSO
{
    [Header("VehicleSO")]
    [SerializeField] private VehicleType _type;
    [SerializeField] private int _maxFuel;
    [SerializeField] private int _maxSpeed;
    [SerializeField] private int _literUsageInterval;
    [SerializeField] private WeaponData[] _weapons;

    public VehicleType Type => _type;
    public WeaponData[] Weapons => _weapons;
    public int MaxFuel => _maxFuel;
    public int MaxSpeed => _maxSpeed;
    public int LiterUsageInterval => _literUsageInterval;
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
    MainGun = 1,
}

[Serializable]
public struct WeaponData // tutaj lokalizacja samej broni przy finishu
{
    public WeaponType Type;
    public Resource AmmoType;
    public Sprite Icon;
    public int Damage;
    public float FireRate;
    public int MaxAmmo;
    public float ProjectileSpeed;
    public float MinFireAngle;
    public float MaxFireAngle;
    public float RecoilForce;
    public float TrajectoryModifier;
}

[Serializable]
public class Weapon
{
    public WeaponData Data;
    public int CurrentAmmo;
    public float CurrentTimer;

    public Weapon(WeaponData p_weaponData)
    {
        Data = p_weaponData;
        CurrentAmmo = p_weaponData.MaxAmmo;

        CurrentTimer = p_weaponData.FireRate;
    }
}