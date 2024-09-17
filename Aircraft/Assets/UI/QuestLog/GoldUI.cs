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
        EventsManager.Instance.GoldEvents.OnGoldChange += GoldChange;
    }

    private void OnDisable() 
    {
        EventsManager.Instance.GoldEvents.OnGoldChange -= GoldChange;
    }

    private void GoldChange(int gold) 
    {
        goldText.text = gold.ToString();
    }
}
