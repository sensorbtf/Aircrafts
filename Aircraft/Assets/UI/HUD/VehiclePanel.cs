using System;
using System.Collections.Generic;
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

                p_selectedVehicle.OnFireShot += UpdateWeaponTab;
            }

            _vehicleHeader.text = p_selectedVehicle.VehicleData.Type.ToString();
            _currentVehicle = p_selectedVehicle;
        }

        public void UpdateWeaponTab()
        {
            if (_currentVehicle is CombatVehicle combatVehicle)
            {
                foreach (var weapon in combatVehicle.VehicleData.Weapons)
                {
                    var newGo = Instantiate(_weaponPrefab, _weaponsGrid.transform);
                    var refs = newGo.GetComponent<WeaponPrefabRefs>();

                    refs.Weapon = weapon.Type;
                    refs.WeaponIcon.sprite = weapon.Icon;
                    refs.WeaponAmmo.text = $"Ammo: {combatVehicle.CurrentWeapon.CurrentAmmo}";

                    _createdWeapons.Add(newGo, refs);
                }
            }
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
                _currentVehicle = null;
            }
        }
    }
}