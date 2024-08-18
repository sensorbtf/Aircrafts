using Resources.Scripts;
using UnityEngine;

namespace Units
{
    public abstract class UnitSO: ScriptableObject
    {
        [Header("UnitSO")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ResourceType[] _resources;
        [SerializeField] private int _maxHp;
        [SerializeField] private int _speed;
        [SerializeField] private int _attackRange;
        [SerializeField] private int _attackCooldown;
        [SerializeField] private int _attackDamage;
        [SerializeField] private float _checkingStateRange;
    
        public GameObject Prefab => _prefab;
        public Sprite Icon => _icon;
        public ResourceType[] Resources => _resources;

        public int MaxHp => _maxHp;
        public int AttackRange => _attackRange;
        public int AttackCooldown => _attackCooldown;
        public int AttackDamage => _attackDamage;
        public int Speed => _speed;
        public float CheckingStateRange => _checkingStateRange;
    }
}