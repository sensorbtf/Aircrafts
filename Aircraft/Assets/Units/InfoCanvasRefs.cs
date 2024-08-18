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

    public void Reorder(bool p_hide)
    {
        if (p_hide)
        {
            GetComponent<Canvas>().sortingOrder = 2;

            //gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -0.5f);
        }
        else
        {
            // gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
            GetComponent<Canvas>().sortingOrder = 1;
        }
    }
}
