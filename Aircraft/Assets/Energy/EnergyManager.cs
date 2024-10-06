using System;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    private int _currentEnergyNeed;
    private int _currentEnergyInput;

    public int CurrentEnergyNeed { get => _currentEnergyNeed; set => _currentEnergyInput = value; }
    public int CurrentEnergyInput { get => _currentEnergyInput; set => _currentEnergyInput = value; }

    public static EnergyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Game Energy Manager in the scene.");
        }
        Instance = this;
    }

    private void Start()
    {
        _currentEnergyNeed = 0;
        _currentEnergyInput = 0;
    }
}
