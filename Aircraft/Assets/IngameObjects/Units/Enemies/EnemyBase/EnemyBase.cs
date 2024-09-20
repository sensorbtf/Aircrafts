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
        [SerializeField] private Transform _groundSpawner; 
        [SerializeField] private Transform _flyingSpawner; 

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
                    if (enemyData.Type == EnemyType.GroundMelee)
                    {
                        OnEnemySpawn?.Invoke(enemyData, _groundSpawner);
                    }
                    else if (enemyData.Type == EnemyType.Flying)
                    {
                        OnEnemySpawn?.Invoke(enemyData, _flyingSpawner);
                    }
                }

                _currentSpawnCooldown = _spawnCooldown;
            }
        }
    }
}