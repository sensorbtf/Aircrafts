using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicles;
using World;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WorldManager _worldManager;
    [SerializeField] private EnemiesManager _enemiesManager;
    [SerializeField] private VehiclesManager _vehiclesManager;
    [SerializeField] private UIManager _uiManager;

    void Start()
    {
        _vehiclesManager.CustomStart();
        _uiManager.CustomStart();
    }
}
