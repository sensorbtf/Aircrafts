using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private HUD _hud;
    
    public void CustomStart()
    {
        _hud.CustomStart();
    }
}
