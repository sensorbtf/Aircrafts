using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aircraft
{
    public readonly AircraftSO Data;

    public float CurrentFuel;
    
    public Aircraft(AircraftSO p_data)
    {
        Data = p_data;
        CurrentFuel = 100;
    }

    public void LowerFuel()
    {
        CurrentFuel--;
    }
}