using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Enemies/Data", order = 2)]
public class EnemySO : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Sprite _icon;
    [SerializeField] private EnemyType _type;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _speed;
    [SerializeField] private int _attackRange;
    [SerializeField] private int _attackCooldown;
    
    public Sprite Icon => _icon;
    public GameObject Prefab => _prefab;
    public EnemyType Type => _type;
    public int MaxHp => _maxHp;
    public int AttackRange => _attackRange;
    public int AttackCooldown => _attackCooldown;
    public int Speed => _speed;
}

public enum EnemyType
{
    GroundMelee = 0,
    GroundRange = 1,
    Flying = 2,
    Underground = 3,
}