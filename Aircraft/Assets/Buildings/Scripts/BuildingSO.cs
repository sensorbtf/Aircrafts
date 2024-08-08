using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "Buildings/Data", order = 2)]
public class BuildingSO : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Sprite _icon;
    [SerializeField] private BuildingType _type;
    [SerializeField] private int _maxHp;
    
    public Sprite Icon => _icon;
    public GameObject Prefab => _prefab;
    public BuildingType Type => _type;
    public int MaxHp => _maxHp;
}

public enum BuildingType
{
    Main_Base = 0,
    Oil_Rig = 1,
    Solar_Installation = 2,
    Sand_Collector = 3,
}