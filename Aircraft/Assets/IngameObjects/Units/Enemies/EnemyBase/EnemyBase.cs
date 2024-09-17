using Buildings;
using System;
using UnityEngine;
using Vehicles;

namespace Enemies
{
    public class EnemyBase: Enemy
    {
        [SerializeField] private EnemySO[] _enemiesToSpawn;
        [SerializeField] private float _spawnCooldown; 

        private float _currentSpawnCooldown; 

        public Action<EnemySO, Transform> OnEnemySpawn;

        public override void HandleSpecialAction()
        {
            if (_currentSpawnCooldown > 0)
            {
                _currentSpawnCooldown -= Time.deltaTime;
            }

            if (_currentSpawnCooldown <= 0)
            {
                foreach (var enemyData in _enemiesToSpawn)
                {
                    OnEnemySpawn?.Invoke(enemyData, gameObject.transform);
                }

                _currentSpawnCooldown = _spawnCooldown;
            }
        }
    }
}