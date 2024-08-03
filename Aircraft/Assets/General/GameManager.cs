using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemiesManager _enemiesManager;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private AircraftSO _firstAircraft;

    void Start()
    {
        _playerController.CustomStart(_firstAircraft);
        _enemiesManager.CustomStart(_firstAircraft);
    }
}
