using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyAircraftPrefab;
    [SerializeField] private Transform _playerBase;
    [SerializeField] private int _numberOfEnemyAircraft = 5;
    [SerializeField] private float _spawnRadius = 10.0f;

    private List<GameObject> enemyAircrafts = new List<GameObject>();

    public void CustomStart(AircraftSO p_firstAircraft)
    {
        //SpawnEnemyAircrafts(p_firstAircraft);
    }

    void SpawnEnemyAircrafts(AircraftSO p_firstAircraft)
    {
        for (int i = 0; i < _numberOfEnemyAircraft; i++)
        {
            var spawnPosition = _playerBase.position;
            var newEnemyAircraft = Instantiate(_enemyAircraftPrefab, spawnPosition, Quaternion.identity);
            newEnemyAircraft.GetComponent<EnemyAircraftController>().CustomStart(new Aircraft(p_firstAircraft));
            enemyAircrafts.Add(newEnemyAircraft);
        }
    }
}
