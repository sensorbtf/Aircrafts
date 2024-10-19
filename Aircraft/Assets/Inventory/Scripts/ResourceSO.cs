using UnityEngine;
using UnityEngine.Serialization;

namespace Resources.Scripts
{
    [CreateAssetMenu(fileName = "ResourceSO", menuName = "Resource/Data", order = 3)]
    public class ResourceSO: ScriptableObject
    {
        [SerializeField] private Sprite _smallIcon;
        [SerializeField] private Sprite _mediumIcon;
        [SerializeField] private Sprite _bigIcon;
        [SerializeField] private Sprite _hugeIcon;
        [FormerlySerializedAs("type")] [SerializeField] private Resource _type;
        [SerializeField] private int _initialValue;
        
        public Resource Type => _type;
        public int InitialValue => _initialValue;

        public Sprite GetSpriteBasedOnAmount(int p_currentAmount)
        {
            switch (p_currentAmount)
            {
                case <= 3:
                    return _smallIcon;
                case <= 6:
                    return _mediumIcon;
                case <= 9:
                    return _bigIcon;
                case > 9:
                    return _hugeIcon;
            }
        }
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