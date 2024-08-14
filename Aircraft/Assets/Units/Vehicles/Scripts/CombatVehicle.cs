using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vehicles
{
    public class CombatVehicle : Vehicle
    {
        [Header("Combat Vehicle")] [SerializeField]
        private Transform _rightfirePoint;

        [SerializeField] private Transform _leftfirePoint;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private int _lineRendererResolution = 100;

        private Weapon _currentWeapon;
        private List<Weapon> _weapons;
        private Camera _mainCamera;
        private LayerMask _groundLayerMask;
        private Transform _currentFirePoint;

        // TODO: Mechanika paliwa i refuellingu (pojazdy lub baza)
        // TODO: Narrator, PoI oraz misje

        public Weapon CurrentWeapon => _currentWeapon;
        public List<Weapon> Weapons => _weapons;

        public override void Initialize(VehicleSO p_vehicleData)
        {
            _mainCamera = Camera.main;
            _groundLayerMask = LayerMask.GetMask("Ground");

            _weapons = new List<Weapon>(p_vehicleData.Weapons.Length);
            foreach (var weaponData in p_vehicleData.Weapons)
            {
                _weapons.Add(new Weapon(weaponData));
            }

            _currentFirePoint = _rightfirePoint;
            base.Initialize(p_vehicleData);
        }

        private void Update()
        {
            foreach (var weapon in _weapons)
            {
                if (weapon.CurrentTimer < weapon.Data.FireRate && weapon.CurrentAmmo > 0)
                {
                    weapon.CurrentTimer += Time.deltaTime;
                }
            }
        }

        public override void HandleMovement()
        {
            if (CurrentFuel <= 0)
                return;
            
            var moveHorizontal = Input.GetAxis("Horizontal");
            var direction = Vector2.zero;

            if (moveHorizontal != 0)
            {
                direction = moveHorizontal > 0 ? Vector2.right : Vector2.left;
            }

            if (direction != Vector2.zero)
            {
                Vector2 newPosition = (Vector2)transform.position + direction * VehicleData.Speed * Time.deltaTime;
                Rigidbody2D.MovePosition(newPosition);
                HandleFuelUsage();
            }

            HandleTurretRotationAndFirePoint();
            DrawTrajectory(_currentFirePoint.localPosition, Time.fixedDeltaTime);
        }
        
        public override void HandleSpecialAction()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (_currentWeapon.CurrentTimer >= _currentWeapon.Data.FireRate)
                {
                    FireProjectile();
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                CycleThroughWeapon();
            }
        }
        
        public override void SelectUnit()
        {
            base.SelectUnit();
            _lineRenderer.enabled = true;

            CycleThroughWeapon();
        }

        public override void UnSelectUnit()
        {
            base.UnSelectUnit();
            _lineRenderer.enabled = false;
        }

        private void HandleTurretRotationAndFirePoint() // tutaj dostosowywać kąt działa
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (mousePosition.x >= transform.position.x)
            {
                _currentFirePoint = _rightfirePoint;
                UnitRenderer.flipX = false;
            }
            else
            {
                _currentFirePoint = _leftfirePoint;
                UnitRenderer.flipX = true;
            }

            Vector2 directionToMouse = (mousePosition - _currentFirePoint.position).normalized;
            var angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

            if (UnitRenderer.flipX)
            {
                angle = 180f - angle;
            }

            angle = Mathf.Clamp(angle, _currentWeapon.Data.MinFireAngle, _currentWeapon.Data.MaxFireAngle);

            _currentFirePoint.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void CycleThroughWeapon()
        {
            if (_currentWeapon == null)
            {
                _currentWeapon = _weapons.FirstOrDefault(x => x.CurrentAmmo > 0);
            }
            else
            {
                int currentIndex = _weapons.IndexOf(_currentWeapon);
                currentIndex++;

                if (currentIndex >= _weapons.Count)
                {
                    currentIndex = 0;
                }

                _currentWeapon = _weapons[currentIndex];
            }

            OnWeaponSwitch?.Invoke();
        }

        private void DrawTrajectory(Vector2 p_startPosition, float p_timeStep)
        {
            _lineRenderer.positionCount = _lineRendererResolution;
            var positions = new Vector3[_lineRendererResolution];
            positions[0] = p_startPosition;

            for (int i = 1; i < positions.Length; i++)
            {
                var time = i * p_timeStep;
                positions[i] = p_startPosition + GetInitialVelocity() * time + Physics2D.gravity * (0.5f * time * time);

                Vector2 rayOrigin = transform.TransformPoint(positions[i - 1]);
                Vector2 rayDirection = positions[i] - positions[i - 1];
                var rayDistance = Vector2.Distance(positions[i - 1], positions[i]);

                var hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, _groundLayerMask);

                if (hit.collider != null && hit.collider.CompareTag("Ground"))
                {
                    positions[i] = transform.InverseTransformPoint(hit.point);
                    _lineRenderer.positionCount = i + 1;
                    break;
                }
            }

            _lineRenderer.SetPositions(positions);
        }

        private Vector2 GetInitialVelocity()
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector2 direction = (mousePosition - _currentFirePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (UnitRenderer.flipX)
            {
                angle = Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg;
            }

            angle = Mathf.Clamp(angle, _currentWeapon.Data.MinFireAngle, _currentWeapon.Data.MaxFireAngle);

            direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            if (UnitRenderer.flipX)
            {
                direction.x = -direction.x;
            }

            return direction * _currentWeapon.Data.ProjectileSpeed;
        }

        private void FireProjectile()
        {
            if (_currentWeapon.CurrentAmmo <= 0)
                return;

            var projectile = Instantiate(_projectilePrefab, _currentFirePoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Initialize(GetInitialVelocity(), _currentWeapon.Data.Damage);

            _currentWeapon.CurrentAmmo--;
            _currentWeapon.CurrentTimer = 0f;

            OnFireShot?.Invoke();
        }
    }
}