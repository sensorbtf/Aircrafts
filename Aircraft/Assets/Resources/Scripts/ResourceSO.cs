using UnityEngine;

namespace Resources.Scripts
{
    [CreateAssetMenu(fileName = "ResourceSO", menuName = "Resource/Data", order = 3)]
    public class ResourceSO: ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private ResourceType _type;
        [SerializeField] private Resource _resource;
        [SerializeField] private int _initialValue;
        
        public Sprite Icon => _icon;
        public ResourceType Type => _type;
        public Resource Resource => _resource;
        public int InitialValue => _initialValue;
    }

    public enum ResourceType
    {
        Fuel = 0,
        RawMaterial = 1,
        AdvancedMaterial = 2,
        Ammunition = 3
    }
    
    public enum Resource
    {
        Petroleum = 0,
        Junk = 1,
        Ammo105mm = 2,
        Ammo35mmm = 3,
        Sand = 4,
        Stone = 5,
        Iron = 6,
        Gold = 7,
        Uranium = 8,
        Titanium = 9,
        Graphene = 10,
        ElectronicCircuits = 11,
        QuantumCircuits = 12,
        ExplosiveMaterials = 13,
    }
}