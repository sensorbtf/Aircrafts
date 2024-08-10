using UnityEngine;

namespace Vehicles
{
    public class CombatVehicle : Vehicle
    {
        [SerializeField] private Transform _firePoint; 
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private LineRenderer _lineRenderer; 

        [SerializeField] private float _projectileSpeed = 10f; 
        [SerializeField] private int _lineRendererResolution = 100; 

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
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
            UpdateLineRenderer(); // Continuously update the line renderer
        }

        private void UpdateLineRenderer()
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; 

            Vector2 direction = (mousePosition - _firePoint.position).normalized;
            Vector2 initialVelocity = direction * _projectileSpeed;

            DrawTrajectory(_firePoint.localPosition, initialVelocity, Time.fixedDeltaTime);
        }

        private void DrawTrajectory(Vector2 startPosition, Vector2 initialVelocity, float timeStep)
        {
            _lineRenderer.positionCount = _lineRendererResolution;
            Vector3[] positions = new Vector3[_lineRendererResolution];
            positions[0] = startPosition;

            for (int i = 1; i < positions.Length; i++)
            {
                float time = i * timeStep;
                positions[i] = startPosition + initialVelocity * time + 0.5f * Physics2D.gravity * time * time;
                if (Physics2D.OverlapPoint(positions[i]))
                {
                    _lineRenderer.positionCount = i + 1;
                    break;
                }
            }

            _lineRenderer.SetPositions(positions);
        }

        public override void HandleSpecialAction()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireProjectile();
            }
        }

        private void FireProjectile()
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure it's on the same plane as the game objects

            Vector2 direction = (mousePosition - _firePoint.position).normalized;

            GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * _projectileSpeed;
        }
    }
}
