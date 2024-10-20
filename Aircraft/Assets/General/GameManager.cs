using System.Collections;
using System.Collections.Generic;
using Enemies;
using Resources;
using UI;
using Objects;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicles;
using World;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WorldManager _worldManager;
    [SerializeField] private UnitsManager _unitsManager;
    [SerializeField] private UIManager _uiManager;

    void Start()
    {
        _unitsManager.CustomStart();
        _uiManager.CustomStart();
    }
}
