using System.Collections.Generic;
using UnityEngine;

namespace Vehicles
{
    public class CombatVehicle : Vehicle
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private LineRenderer _lineRenderer;

        [SerializeField] private int _lineRendererResolution = 100;

        private Weapon _currentWeapon;
        private List<Weapon> _weapons;
        private Camera _mainCamera;
        private LayerMask _groundLayerMask;

        // TODO: Mechanika paliwa i refuellingu (pojazdy lub baza)
        // TODO: Narrator, PoI oraz misje

        public Weapon CurrentWeapon => _currentWeapon;
        
        public override void Initialize(VehicleSO p_vehicleData)
        {
            _mainCamera = Camera.main;
            _groundLayerMask = LayerMask.GetMask("Ground");
            
            _weapons = new List<Weapon>(p_vehicleData.Weapons.Length);
            foreach (var weaponData in p_vehicleData.Weapons)
            {
                _weapons.Add(new Weapon(weaponData));
            }

            _currentWeapon = _weapons[0];
            base.Initialize(p_vehicleData);
        }
        
        public override void HandleMovement()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");

            Vector2 direction = Vector2.zero;

            if (moveHorizontal > 0)
            {
                direction = Vector2.right;
                UnitRenderer.flipX = false;
            }
            else if (moveHorizontal < 0)
            {
                direction = Vector2.left;
                UnitRenderer.flipX = true;
            }

            if (direction != Vector2.zero)
            {
                Vector2 newPosition = (Vector2)transform.position + direction * VehicleData.Speed * Time.deltaTime;
                Rigidbody2D.MovePosition(newPosition);
            }
            
            DrawTrajectory(_firePoint.localPosition, GetInitialVelocity(), Time.fixedDeltaTime);
        }

        public override void HandleSpecialAction()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireProjectile();
            }
        }

        private Vector2 GetInitialVelocity()
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            
            Vector2 direction = (mousePosition - _firePoint.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, _currentWeapon.Data.MinFireAngle, _currentWeapon.Data.MaxFireAngle);
            direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            return  direction * _currentWeapon.Data.ProjectileSpeed;
        }

        private void DrawTrajectory(Vector2 p_startPosition, Vector2 p_initialVelocity, float p_timeStep)
        {
            _lineRenderer.positionCount = _lineRendererResolution;
            Vector3[] positions = new Vector3[_lineRendererResolution];
            positions[0] = p_startPosition;

            for (int i = 1; i < positions.Length; i++)
            {
                var time = i * p_timeStep;
                positions[i] = p_startPosition + p_initialVelocity * time + 0.5f * Physics2D.gravity * time * time;

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

        private void FireProjectile()
        {
            var projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Initialize(GetInitialVelocity(), _currentWeapon.Data.Damage);
            OnFireShot?.Invoke();
            _currentWeapon.CurrentAmmo--;
        }
    }
}
