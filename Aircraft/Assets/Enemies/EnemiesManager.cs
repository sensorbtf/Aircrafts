using System;
using System.Collections.Generic;
using Buildings;
using Enemies;
using UnityEngine;

namespace Enemies
{
    public class EnemiesManager : MonoBehaviour
    {
        [SerializeField] private Transform _playerBase;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private EnemyDatabase _enemyDatabase;
        [SerializeField] private int _numberOfEnemyAircraft = 5;

        private List<Enemy> _enemies = new List<Enemy>();

        public void CustomStart()
        {
            foreach (var enemy in _enemyDatabase.Enemies)
            {
                var newEnemy = Instantiate(enemy.Prefab, gameObject.transform);
                newEnemy.transform.position = _spawnPoint.localPosition;
                
                switch (enemy.Type)
                {
                    case EnemyType.GroundMelee:
                        _enemies.Add(newEnemy.GetComponent<GroundEnemy>());
                        newEnemy.GetComponent<GroundEnemy>().Initialize(enemy);
                        newEnemy.GetComponent<GroundEnemy>().OnEnemyClicked += SelectEnemy;
                        break;
                    case EnemyType.GroundRange:
                        break;
                    case EnemyType.Flying:
                        break;
                }
            }
        }

        private void Update()
        {
            foreach (var enemy in _enemies)
            {
                enemy.HandleSpecialAction();
            }
        }
        
        private void FixedUpdate()
        {
            foreach (var enemy in _enemies)
            {
                enemy.HandleMovement(_playerBase);
            }
        }

        public void SelectEnemy(Enemy p_newVehicle)
        {
            //???
        }
    }
}