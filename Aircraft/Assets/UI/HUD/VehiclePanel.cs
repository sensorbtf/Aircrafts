using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vehicles;

namespace UI.HUD
{
    public class VehiclePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _vehicleHeader;
        [SerializeField] private GameObject _weaponsGrid;
        [SerializeField] private GameObject _weaponPrefab;
        [SerializeField] private Slider _fuelSlider;
        [SerializeField] private TextMeshProUGUI _fuelText;

        private Dictionary<GameObject, WeaponPrefabRefs> _createdWeapons;
        private Vehicle _currentVehicle;
        
        private void Start()
        {
            _createdWeapons = new Dictionary<GameObject, WeaponPrefabRefs>();
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

            if (p_selectedVehicle is CombatVehicle combatVehicle)
            {
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
                p_selectedVehicle.OnFuelChange += UpdateFuelSlider;
            }

            _vehicleHeader.text = p_selectedVehicle.VehicleData.Type.ToString();
            _currentVehicle = p_selectedVehicle;
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
            
            if (_currentVehicle != null)
            {
                _currentVehicle.OnFireShot -= UpdateWeaponTab;
                _currentVehicle.OnWeaponSwitch -= UpdateWeaponTab;
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
    }
}