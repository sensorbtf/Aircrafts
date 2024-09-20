using Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Objects.Vehicles
{
    public class CombatVehicle : Vehicle
    {
        [Header("Combat Vehicle")]
        [SerializeField] private Transform _rightfirePoint;
        [SerializeField] private Transform _leftfirePoint;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private int _lineRendererResolution = 100;

        [Header("Movement Settings")]
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _dashSpeed = 10f;
        [SerializeField] private float _dashDuration = 0.2f;
        [SerializeField] private GameObject _engineSprite;

        [SerializeField] private float _thrustForce = 3f;
        [SerializeField] private float _maxThrustSpeed = 10f;
        [SerializeField] private float _gravityModifier = 1f;
        [SerializeField] private float _maxHeight = 18f;

        [SerializeField] private float _fallSpeedMultiplier = 2f;
        [SerializeField] private float _fallDamageThreshold = 15f;
        [SerializeField] private float _fallDamageMultiplier = 10f;

        private Weapon _currentWeapon;
        private List<Weapon> _weapons;
        private Camera _mainCamera;
        private Transform _currentFirePoint;
        private bool _isGrounded;
        private bool _canJump;
        private float _dashTime;
        private float _lastFallSpeed = 0;

        public Weapon CurrentWeapon => _currentWeapon;
        public List<Weapon> Weapons => _weapons;

        internal Action OnFireShot;
        internal Action OnWeaponSwitch;

        public override void Initialize(VehicleSO p_vehicleData)
        {
            _mainCamera = Camera.main;

            _weapons = new List<Weapon>(p_vehicleData.Weapons.Length);
            foreach (var weaponData in p_vehicleData.Weapons)
            {
                _weapons.Add(new Weapon(weaponData));
            }

            _currentFirePoint = _rightfirePoint;
            base.Initialize(p_vehicleData);
        }

        public override void Update()
        {
            base.Update();

            foreach (var weapon in _weapons)
            {
                if (weapon.CurrentTimer < weapon.Data.FireRate && weapon.CurrentAmmo > 0)
                {
                    weapon.CurrentTimer += Time.deltaTime;
                }
            }

            RotateEngineSprite();
        }

        public override void SelectedFixedUpdate()
        {
            base.Update();

            if (CurrentFuel <= 0)
                return;

            CheckGrounded();

            if (Input.GetKey(KeyCode.W))
            {
                Jump();
                ApplyThrust();
            }

            if (Input.GetKey(KeyCode.S) && !_isGrounded)
            {
                Rigidbody2D.AddForce(Vector2.down * _fallSpeedMultiplier, ForceMode2D.Force);
            }

            HandleMovement();

            HandleTurretRotationAndFirePoint();
            DrawTrajectory(_currentFirePoint.localPosition, Time.fixedDeltaTime);

            if (_isGrounded && Rigidbody2D.velocity.y <= -_fallDamageThreshold)
            {
                ApplyFallDamage();
            }

            CorrectRotation();
        }

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();

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

        public override void CheckState()
        {
            base.CheckState();

            if (Weapons.Any(x => x.CurrentAmmo < x.Data.MaxAmmo))
            {
                SetNewStateTexts(Actions.Arm);
            }
            else
            {
                ResetStateText(Actions.Arm);
            }
        }

        public override void SelectObject()
        {
            base.SelectObject();
            _lineRenderer.enabled = true;

            CycleThroughWeapon();
        }

        public override void UnSelectObject()
        {
            base.UnSelectObject();
            _lineRenderer.enabled = false;
        }

        private void HandleMovement()
        {
            var moveHorizontal = Input.GetAxis("Horizontal");

            if (moveHorizontal != 0)
            {
                float currentVelocityX = Rigidbody2D.velocity.x;

                if (_isGrounded)
                {
                    currentVelocityX += moveHorizontal * VehicleData.Speed * Time.deltaTime;
                }
                else
                {
                    currentVelocityX += moveHorizontal * (VehicleData.Speed / 2) * Time.deltaTime;
                }

                currentVelocityX = Mathf.Clamp(currentVelocityX, -VehicleData.MaxSpeed, VehicleData.MaxSpeed);

                Rigidbody2D.velocity = new Vector2(currentVelocityX, Rigidbody2D.velocity.y);

                HandleFuelUsage();
            }
        }


        private void CheckGrounded()
        {
            float leftRightRayDistance = 1f;
            float centerRayDistance = 0.6f;

            Vector2 downLeft = new Vector2(-1.4f, -1).normalized;
            Vector2 down = Vector2.down;
            Vector2 downRight = new Vector2(1.4f, -1).normalized;

            RaycastHit2D leftHit = Physics2D.Raycast(transform.position, downLeft, leftRightRayDistance, LayerManager.GroundLayer);
            RaycastHit2D centerHit = Physics2D.Raycast(transform.position, down, centerRayDistance, LayerManager.GroundLayer);
            RaycastHit2D rightHit = Physics2D.Raycast(transform.position, downRight, leftRightRayDistance, LayerManager.GroundLayer);

            RaycastHit2D leftHit2 = Physics2D.Raycast(transform.position, downLeft, leftRightRayDistance, LayerManager.EnemyLayer);
            RaycastHit2D centerHit2 = Physics2D.Raycast(transform.position, down, centerRayDistance, LayerManager.EnemyLayer);
            RaycastHit2D rightHit2 = Physics2D.Raycast(transform.position, downRight, leftRightRayDistance, LayerManager.EnemyLayer);

            _isGrounded = (leftHit.collider != null || centerHit.collider != null || rightHit.collider != null ||
                           leftHit2.collider != null || centerHit2.collider != null || rightHit2.collider != null);
        }

        private void HandleTurretRotationAndFirePoint()
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (mousePosition.x >= transform.position.x)
            {
                _currentFirePoint = _rightfirePoint;
                ObjectRenderer.flipX = false;
            }
            else
            {
                _currentFirePoint = _leftfirePoint;
                ObjectRenderer.flipX = true;
            }

            Vector2 directionToMouse = (mousePosition - _currentFirePoint.position).normalized;
            var angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

            if (ObjectRenderer.flipX)
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

                var hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, LayerManager.GroundLayer);

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

            if (ObjectRenderer.flipX)
            {
                angle = Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg;
            }

            angle = Mathf.Clamp(angle, _currentWeapon.Data.MinFireAngle, _currentWeapon.Data.MaxFireAngle);

            direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            if (ObjectRenderer.flipX)
            {
                direction.x = -direction.x;
            }

            return direction * _currentWeapon.Data.ProjectileSpeed;
        }

        private void RotateEngineSprite()
        {
            Vector2 movementDirection = Rigidbody2D.velocity.normalized;

            float targetAngle;

            if (movementDirection.x == 1 && movementDirection.y != 0)
            {
                targetAngle = -90;
            }
            else if (movementDirection.x == -1 && movementDirection.y != 0)
            {
                targetAngle = 90;
            }
            else if (movementDirection.y == 1 || movementDirection.y == -1)
            {
                targetAngle = 0;
            }
            else
            {
                float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;

                if (movementDirection.x < 0)
                {
                    targetAngle = Mathf.Clamp(angle + 180f, -90f, 90f);
                }
                else
                {
                    targetAngle = Mathf.Clamp(angle, -90f, 90f);
                }
            }

            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            _engineSprite.transform.rotation = Quaternion.Lerp(_engineSprite.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        private void FireProjectile()
        {
            if (_currentWeapon.CurrentAmmo <= 0)
                return;

            var projectile = Instantiate(_projectilePrefab, _currentFirePoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Initialize(GetInitialVelocity(), _currentWeapon.Data.Damage, false);

            ApplyRecoil(GetInitialVelocity());

            _currentWeapon.CurrentAmmo--;
            _currentWeapon.CurrentTimer = 0f;

            OnFireShot?.Invoke();
        }

        private void ApplyRecoil(Vector2 p_projectileVelocity)
        {
            Vector2 recoilDirection = -new Vector2(p_projectileVelocity.normalized.x, 0);

            float recoilForce = _currentWeapon.Data.RecoilForce;

            if (_isGrounded)
            {
                Rigidbody2D.AddForce(recoilDirection * (recoilForce / 2), ForceMode2D.Impulse);
            }
            else
            {
                Rigidbody2D.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse);
            }

            Debug.Log("Recoil applied: " + recoilDirection * recoilForce);
        }


        private void Jump()
        {
            if (_isGrounded)
            {
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, _jumpForce);
                _isGrounded = false;
                _lastFallSpeed = 0;
                HandleFuelUsage(5);
            }
        }

        private void ApplyThrust()
        {
            if (!_isGrounded && Input.GetKey(KeyCode.W) && transform.position.y < _maxHeight)
            {
                float heightRatio = transform.position.y / _maxHeight;
                float scaledThrustForce = Mathf.Lerp(_thrustForce, 0, heightRatio);

                if (Rigidbody2D.velocity.y < _maxThrustSpeed)
                {
                    Rigidbody2D.AddForce(Vector2.up * scaledThrustForce, ForceMode2D.Force);
                    HandleFuelUsage(2);
                }
            }
        }

        private void ApplyFallDamage()
        {
            _lastFallSpeed = Mathf.Abs(Rigidbody2D.velocity.y);

            if (_lastFallSpeed >= _fallDamageThreshold)
            {
                int damage = Mathf.CeilToInt((_lastFallSpeed - _fallDamageThreshold) * _fallDamageMultiplier);
                ReceiveDamage(damage);
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, 0);
            }
        }

        private void OnCollisionEnter2D(Collision2D p_collision)
        {
            if (_isGrounded)
            {
                if (p_collision.gameObject.layer == LayerManager.GroundLayerIndex)
                {
                    _lastFallSpeed = 0;
                }
            }

            if (_lastFallSpeed >= _fallDamageThreshold)
            {
                if (p_collision.gameObject.layer == LayerManager.EnemyLayerIndex)
                {
                    Enemy enemy = p_collision.gameObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.ReceiveDamage((int)_lastFallSpeed);
                    }

                    _lastFallSpeed = 0;
                }
            }
        }

        private void CorrectRotation()
        {
            float currentZRotation = transform.eulerAngles.z;

            if (currentZRotation > 180)
                currentZRotation -= 360;

            float correctionTorque = -currentZRotation * 0.5f;
            Rigidbody2D.AddTorque(correctionTorque);
        }

        private void OnDrawGizmos()
        {
            Vector2 downLeft = new Vector2(-1.4f, -1).normalized;
            Vector2 down = Vector2.down;
            Vector2 downRight = new Vector2(1.4f, -1).normalized;

            float leftRightRayDistance = 1f;
            float centerRayDistance = 0.6f;

            Gizmos.color = Color.green;

            Gizmos.DrawLine(transform.position, transform.position + (Vector3)(downLeft * leftRightRayDistance));
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)(down * centerRayDistance));
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)(downRight * leftRightRayDistance));
        }
    }
}
