using System;
using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Enemies/Data", order = 2)]
public class EnemySO: UnitSO
{
    [SerializeField] private EnemyType _type;
    
    public EnemyType Type => _type;
}

public enum EnemyType
{
    GroundMelee = 0,
    GroundRange = 1,
    Flying = 2,
    Underground = 3,
}