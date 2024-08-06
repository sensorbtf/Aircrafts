using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicles;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemiesManager _enemiesManager;
    [SerializeField] private VehiclesManager _vehiclesManager;
    [SerializeField] private UIManager _uiManager;

    void Start()
    {
        _vehiclesManager.CustomStart();
        _uiManager.CustomStart();
    }
}
