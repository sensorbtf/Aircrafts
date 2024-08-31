using Objects;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "Buildings/Data", order = 2)]
public class BuildingSO: UnitSO
{
    [SerializeField] private BuildingType _type;
    
    public BuildingType Type => _type;
}

public enum BuildingType
{
    Base = 0,
    Oil_Rig = 1,
    Solar_Installation = 2,
    Sand_Collector = 3,
    Workshop = 4,
}