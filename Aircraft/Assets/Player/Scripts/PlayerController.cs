using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Aircraft _currentAircraft;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float groundLevel = 0.0f;
    [SerializeField] private float maxAltitude = 100.0f;
    [SerializeField] private float liftOffSpeed = 50.0f; // Speed required for lift-off
    [SerializeField] private float liftOffAngle = 15.0f; // Angle to gradually pitch up for lift-off
    [SerializeField] private float liftOffSpeedThreshold = 0.1f; // Speed increase threshold for smooth pitch

    private float _currentThrust;
    private float _targetPitchAngle;
    private float _currentPitchAngle;
    private bool _isFlying;

    public float CurrentThrust => _currentThrust;
    public float CurrentAircraftFuel => _currentAircraft.CurrentFuel;

    public float CurrentSpeed
    {
        get
        {
            float altitude = Mathf.Abs(transform.position.y - groundLevel);
            float speedFactor = Mathf.Clamp01(1 - altitude / maxAltitude);
            return _currentThrust * 0.01f * _currentAircraft.Data.Speed * speedFactor;
        }
    }

    public void CustomStart(AircraftSO p_firstAircraft)
    {
        _currentAircraft = new Aircraft(p_firstAircraft);
        _currentThrust = 0;
        _isFlying = false;
    }

    void Update()
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.D))
        {
            _currentThrust += 5;
            if (_currentThrust > 100)
            {
                _currentThrust = 100;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _currentThrust -= 5;
            if (_currentThrust < 0)
            {
                _currentThrust = 0;
            }
        }

        if (_isFlying && transform.position.y <= groundLevel + 0.25f)
        {
            Debug.LogError("DESTROYED");
        }
    }

    private void HandleMovement()
    {
        if (_rigidbody2D == null || _currentAircraft == null)
            return;

        float rotationInput = Input.GetAxis("Vertical"); // W and S keys

        Vector2 thrustDirection = transform.right * CurrentSpeed;
        _rigidbody2D.velocity = thrustDirection;

        if (!_isFlying && transform.position.y <= groundLevel + 0.3f)
        {
            if (CurrentSpeed >= liftOffSpeed)
            {
                _targetPitchAngle = liftOffAngle;
            }
            else
            {
                _targetPitchAngle = 0;
            }

            _currentPitchAngle = Mathf.Lerp(_currentPitchAngle, _targetPitchAngle, liftOffSpeedThreshold * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, _currentPitchAngle);

            float rotationAmountZ = -rotationInput * _currentAircraft.Data.Maneuverability * Time.deltaTime;
            transform.Rotate(0, 0, rotationAmountZ);
        }
        else
        {
            _isFlying = true;
            var rotationAmount = -rotationInput * _currentAircraft.Data.Maneuverability * Time.deltaTime;
            transform.Rotate(0, 0, rotationAmount);
        }
    }
}