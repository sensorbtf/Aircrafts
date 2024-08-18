using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class CanvasInteractionRefs : MonoBehaviour
{
    public Actions Action;
    public Button Button;
    public Slider Slider;
    public TextMeshProUGUI TextInfo;
}

public enum Actions
{
    Noone, Refill, Arm, Repair
}