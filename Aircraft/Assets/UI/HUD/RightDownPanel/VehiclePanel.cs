using System;
using System.Collections.Generic;
using System.Linq;
using Resources;
using Resources.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Units.Vehicles;

namespace UI.HUD
{
    public class VehiclePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _vehicleHeader;
        [SerializeField] private GameObject _weaponsGrid;
        [SerializeField] private GameObject _weaponPrefab;
        [SerializeField] private GameObject _resourcesGrid;
        [SerializeField] private GameObject _resourcesPrefab;
        [SerializeField] private Slider _fuelSlider;
        [SerializeField] private TextMeshProUGUI _fuelText;

        private Dictionary<GameObject, WeaponPrefabRefs> _createdWeapons;
        private Dictionary<ResourceSO, HudIconRefs> _createdResources;
        private Vehicle _currentVehicle;
        
        private void Start()
        {
            _createdWeapons = new Dictionary<GameObject, WeaponPrefabRefs>();
            _createdResources = new Dictionary<ResourceSO, HudIconRefs>();
            ClosePanel();
        }

        private void Update()
        {
            if (_currentVehicle == null)
                return;

            if (_currentVehicle is CombatVehicle vehicle)
            {
                foreach (var refs in _createdWeapons)
                {
                    var weapon = vehicle.Weapons.FirstOrDefault(x => x.Data.Type == refs.Value.Weapon);
                    var targetValue = weapon.CurrentTimer >= weapon.Data.FireRate ? 0 : 1 - (weapon.CurrentTimer / weapon.Data.FireRate);

                    refs.Value.Timer.value = Mathf.Lerp(refs.Value.Timer.value, targetValue, Time.deltaTime * 10f);
                }
            }
        }

        public void OpenPanel(Vehicle p_selectedVehicle)
        {
            ClosePanel();
            gameObject.SetActive(true);

            _vehicleHeader.text = p_selectedVehicle.VehicleData.Type.ToString();
            _currentVehicle = p_selectedVehicle;
            
            if (p_selectedVehicle is CombatVehicle combatVehicle)
            {
                _resourcesGrid.SetActive(false);
                _weaponsGrid.SetActive(true);
                
                foreach (var weapon in p_selectedVehicle.VehicleData.Weapons)
                {
                    var newGo = Instantiate(_weaponPrefab, _weaponsGrid.transform);
                    var refs = newGo.GetComponent<WeaponPrefabRefs>();

                    refs.Weapon = weapon.Type;
                    refs.WeaponIcon.sprite = weapon.Icon;
                    refs.WeaponAmmo.text = $"Ammo: {combatVehicle.CurrentWeapon.CurrentAmmo}";

                    _createdWeapons.Add(newGo, refs);
                }

                combatVehicle.OnFireShot += UpdateWeaponTab;
                combatVehicle.OnWeaponSwitch += UpdateWeaponTab;
            }
            else
            {
                _resourcesGrid.SetActive(true);
                _weaponsGrid.SetActive(false);

                foreach (var resource in p_selectedVehicle.Inventory.CurrentResources)
                {
                    var newGo = Instantiate(_resourcesPrefab, _resourcesGrid.transform);
                    var refs = newGo.GetComponent<HudIconRefs>();

                    refs.Icon.sprite = resource.Data.Icon;
                    refs.Text.text = $"{resource.CurrentAmount}";

                    _createdResources.Add(resource.Data, refs);
                }

                p_selectedVehicle.Inventory.OnResourceValueChanged += RefreshResources;
            }
            
            p_selectedVehicle.OnFuelChange += UpdateFuelSlider;

            UpdateWeaponTab();
            UpdateFuelSlider();
        }

        private void UpdateFuelSlider()
        {
            _fuelText.text = $"{_currentVehicle.CurrentFuel}/{_currentVehicle.VehicleData.MaxFuel}";
            _fuelSlider.maxValue = _currentVehicle.VehicleData.MaxFuel;
            _fuelSlider.value = _currentVehicle.CurrentFuel;
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
            foreach (var weapon in _createdWeapons)
            {
                Destroy(weapon.Key.gameObject);
            }

            _createdWeapons.Clear();
            
            foreach (var weapon in _createdResources)
            {
                Destroy(weapon.Value.gameObject);
            }
            
            _createdResources.Clear();
            
            if (_currentVehicle != null)
            {
                _currentVehicle.OnFireShot -= UpdateWeaponTab;
                _currentVehicle.OnWeaponSwitch -= UpdateWeaponTab;
                _currentVehicle.OnFuelChange -= UpdateFuelSlider;
                _currentVehicle.Inventory.OnResourceValueChanged -= RefreshResources;
                _currentVehicle = null;
            }
        }

        private void UpdateWeaponTab()
        {
            if (_currentVehicle is CombatVehicle combatVehicle)
            {
                foreach (var refs in _createdWeapons)
                {
                    var weapon = combatVehicle.Weapons.FirstOrDefault(x => x.Data.Type == refs.Value.Weapon);
                    
                    if (combatVehicle.CurrentWeapon != null && weapon.CurrentAmmo > 0)
                    {
                        refs.Value.Background.color = refs.Value.Weapon == combatVehicle.CurrentWeapon.Data.Type ? Color.cyan : new Color(0, 0, 0, 0);
                        refs.Value.WeaponAmmo.text = $"Ammo: {weapon.CurrentAmmo}";
                    }
                    else
                    {
                        refs.Value.Background.color = Color.red;
                        refs.Value.WeaponAmmo.text = $"Replenish Ammo!";

                    }
                }
            }
        }
        
        private void RefreshResources(ResourceInUnit p_resourceInUnit)
        {
            if (_currentVehicle is CombatVehicle vehicle)
            {
                
            }
            else
            {
                foreach (var resource in _createdResources)
                {
                    if (resource.Key == p_resourceInUnit.Data)
                    {
                        resource.Value.Text.text = _currentVehicle.Inventory.GetResourceAmount(resource.Key.Type).ToString();
                    }
                }
            }
        }
    }
}