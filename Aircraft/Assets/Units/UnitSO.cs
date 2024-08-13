using UnityEngine;

namespace Units
{
    public abstract class UnitSO: ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _maxHp;
        [SerializeField] private int _speed;
        [SerializeField] private int _attackRange;
        [SerializeField] private int _attackCooldown;
        [SerializeField] private int _attackDamage;
    
        public Sprite Icon => _icon;
        public GameObject Prefab => _prefab;
        public int MaxHp => _maxHp;
        public int AttackRange => _attackRange;
        public int AttackCooldown => _attackCooldown;
        public int AttackDamage => _attackDamage;
        public int Speed => _speed;
    }
}