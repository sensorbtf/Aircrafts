using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Slider _powerSlider;
    [SerializeField] private TextMeshProUGUI _powerSliderText;
    
    [SerializeField] private Slider _fuelSlider;
    [SerializeField] private TextMeshProUGUI _fuelSliderText;
    
    // Start is called before the first frame update
    void Start()
    {
        _powerSlider.minValue = 0;
        _powerSlider.maxValue = 100;        
        _powerSlider.value = Mathf.CeilToInt(_playerController.CurrentThrust);
        _powerSliderText.text = Mathf.CeilToInt(_playerController.CurrentThrust).ToString();
            
        _fuelSlider.minValue = 0;
        _fuelSlider.maxValue = 100;
        _fuelSlider.value = _playerController.CurrentAircraftFuel;
        _fuelSliderText.text = Mathf.CeilToInt(_playerController.CurrentAircraftFuel).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _powerSlider.value = Mathf.CeilToInt(_playerController.CurrentThrust);
        _powerSliderText.text = Mathf.CeilToInt(_playerController.CurrentThrust).ToString();
        _fuelSlider.value = _playerController.CurrentAircraftFuel;
        _fuelSliderText.text = Mathf.CeilToInt(_playerController.CurrentAircraftFuel).ToString();
    }
}
