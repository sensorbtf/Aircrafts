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
        MainGameEventsManager.Instance.goldEvents.OnGoldChange += GoldChange;
    }

    private void OnDisable() 
    {
        MainGameEventsManager.Instance.goldEvents.OnGoldChange -= GoldChange;
    }

    private void GoldChange(int gold) 
    {
        goldText.text = gold.ToString();
    }
}
