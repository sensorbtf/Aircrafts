using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyAircraftPrefab;
    [SerializeField] private Transform _playerBase;
    [SerializeField] private int _numberOfEnemyAircraft = 5;
    [SerializeField] private float _spawnRadius = 10.0f;

    private List<GameObject> enemyAircrafts = new List<GameObject>();

    public void CustomStart(VehicleSO pFirstVehicle)
    {
        //SpawnEnemyAircrafts(p_firstAircraft);
    }
}
