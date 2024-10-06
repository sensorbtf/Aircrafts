using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable() 
    {
        EventsManager.Instance.EnergyEvents.OnGoldChange += EnergyChange;
    }

    private void OnDisable() 
    {
        EventsManager.Instance.EnergyEvents.OnGoldChange -= EnergyChange;
    }

    private void EnergyChange(int gold) 
    {
        goldText.text = gold.ToString();
    }
}
