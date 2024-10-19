using Objects;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "Buildings/Data", order = 2)]
public class BuildingSO: UnitSO
{
    [SerializeField] private BuildingType _type;
    [SerializeField] private int _energyNeed;
    
    public BuildingType Type => _type;
    public int EnergyNeed => _energyNeed;
}

public enum BuildingType
{
    Base = 0,
    Oil_Rig = 1,
    Solar_Installation = 2,
    Sand_Collector = 3,
    Generator = 4,
    WindTurbine = 5,
    Ark = 6,
}