using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAircraftController : MonoBehaviour
{
    [SerializeField] private Aircraft _currentAircraft;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float patrolRadius = 10.0f;
    [SerializeField] private float patrolSpeed = 5.0f;
    [SerializeField] private float patrolHeight = 20.0f;
    [SerializeField] private float groundLevel = 0.0f;
    [SerializeField] private float maxAltitude = 100.0f;
    [SerializeField] private float liftOffSpeed = 50.0f; // Speed required for lift-off
    [SerializeField] private float liftOffAngle = 15.0f; // Angle to gradually pitch up for lift-off
    [SerializeField] private float liftOffSpeedThreshold = 0.1f; // Speed increase threshold for smooth pitch

    private float _currentThrust;
    private float _targetPitchAngle;
    private float _currentPitchAngle;
    private bool _isFlying;
    private Vector2 _patrolCenter;
    private Vector2 _targetPosition;

    public void CustomStart(Aircraft aircraft)
    {
        _currentAircraft = aircraft;

        _patrolCenter = transform.position;
        SetNewTargetPosition();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_rigidbody2D == null || _currentAircraft == null)
            return;

        float step = patrolSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, step);

        if (Vector2.Distance(transform.position, _targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }

        MaintainAltitudeAndThrust();

        Vector2 direction = _targetPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rigidbody2D.rotation = angle;
    }

    private void SetNewTargetPosition()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        _targetPosition = _patrolCenter + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * patrolRadius;
        _targetPosition.y = patrolHeight;
    }

    private void MaintainAltitudeAndThrust()
    {
        if (transform.position.y <= groundLevel + 0.3f)
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
            var targetRotation = Quaternion.Euler(0, 0, _currentPitchAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
        }
        else
        {
            _isFlying = true;
            float rotationAmountZ = 0.0f; // No vertical input for enemy, adjust if needed
            var targetRotation = Quaternion.Euler(0, 0, rotationAmountZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
        }
    }

    public float CurrentSpeed
    {
        get
        {
            float altitude = Mathf.Abs(transform.position.y - groundLevel);
            float speedFactor = Mathf.Clamp01(1 - altitude / maxAltitude);
            return _currentThrust * 0.01f * _currentAircraft.Data.Speed * speedFactor;
        }
    }
}
