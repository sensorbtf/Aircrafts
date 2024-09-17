using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        EventsManager.Instance.PlayerEvents.OnPlayerExperienceChange += PlayerExperienceChange;
        EventsManager.Instance.PlayerEvents.OnPlayerLevelChange += PlayerLevelChange;
    }

    private void OnDisable() 
    {
        EventsManager.Instance.PlayerEvents.OnPlayerExperienceChange -= PlayerExperienceChange;
        EventsManager.Instance.PlayerEvents.OnPlayerLevelChange -= PlayerLevelChange;
    }

    private void PlayerExperienceChange(int experience) 
    {
        xpSlider.value = (float) experience / (float) 100;
        xpText.text = experience + " / " + 100;
    }

    private void PlayerLevelChange(int level) 
    {
        levelText.text = "Level " + level;
    }
}
