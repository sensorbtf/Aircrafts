using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleSO", menuName = "Vehicle/Data", order = 2)]
public class VehicleSO : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Sprite _icon;
    [SerializeField] private VehicleType _type;
    [SerializeField] private Weapon[] _weaponsType;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _speed;
    [SerializeField] private int _maneuverability;
    
    
    public Sprite Icon => _icon;
    public GameObject Prefab => _prefab;
    public VehicleType Type => _type;
    public Weapon[] WeaponsType => _weaponsType;
    public int MaxHp => _maxHp;
    public int Speed => _speed;
    public int Maneuverability => _maneuverability;
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