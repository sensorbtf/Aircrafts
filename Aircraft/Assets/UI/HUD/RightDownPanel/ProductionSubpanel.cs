using System.Collections;
using System.Collections.Generic;
using Buildings;
using Resources.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionSubpanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _progress;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _stateImage;
    [SerializeField] private TextMeshProUGUI _stateAmount;
    [SerializeField] private Image _output;
    [SerializeField] private Button _stateButton;

    [SerializeField] private Sprite _brokenSprite;
    [SerializeField] private Sprite _pausedSprite;
    [SerializeField] private Sprite _workingSprite;

    private ProductionBuilding _currentProductionBuilding;

    private float _currentFillAmount;
    private float _currentPercent;
    private float _smoothTime = 0.2f;
    private float _velocity = 0f;
    private float _textVelocity = 0f;

    public void CustomStart(ProductionBuilding p_currentProductionBuilding)
    {
        _currentProductionBuilding = p_currentProductionBuilding;
        _output.sprite = _currentProductionBuilding.OutputProduction.Icon;

        if (_currentProductionBuilding.IsBroken)
        {
            _stateImage.sprite = _brokenSprite;
        }
        else
        {
            _stateImage.sprite = _currentProductionBuilding.IsPaused ? _pausedSprite : _workingSprite;
        }

        p_currentProductionBuilding.Inventory.OnResourceValueChanged += RefreshCurrentAmount;
        RefreshCurrentAmount(_currentProductionBuilding.OutputProduction);
    }
    
    public void OnPanelClose()
    {
        _currentProductionBuilding.Inventory.OnResourceValueChanged -= RefreshCurrentAmount;
        _currentProductionBuilding = null;
    }

    private void RefreshCurrentAmount(ResourceSO p_resource)
    {
        _stateAmount.text = _currentProductionBuilding.Inventory.GetResourceAmount(p_resource).ToString();
    }

    private void Update()
    {
        if (_currentProductionBuilding == null)
            return;

        SetProductionPercent();
    }

    private void SetProductionPercent()
    {
        // Calculate the target progress value
        var targetProgress = _currentProductionBuilding.TimeToComplete == 0
            ? 0
            : _currentProductionBuilding.CurrentProductionTime / _currentProductionBuilding.TimeToComplete;

        _currentFillAmount =
            Mathf.SmoothDamp(_currentFillAmount, Mathf.Min(1, targetProgress), ref _velocity, _smoothTime);

        _fill.fillAmount = _currentFillAmount;

        float targetPercent = targetProgress * 100f;

        _currentPercent = Mathf.SmoothDamp(_currentPercent, targetPercent, ref _textVelocity, _smoothTime);

        _progress.text = $"{(int)_currentPercent}%";

        _fill.color = _currentProductionBuilding.IsFunctioning ? Color.white : Color.gray;
    }
}