using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoCanvasRefs : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private CanvasInteractionRefs[] _stateInfo; // Refuell, Repair, Arm
    
    public Slider HealthBar => _healthBar;
    public CanvasInteractionRefs[] StateInfo => _stateInfo;
}
