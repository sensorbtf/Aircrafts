using UnityEngine;

namespace Vehicles
{
    public class UndergroundVehicle : Vehicle
    {
        [SerializeField] private Transform _crushPoint;

        void Update()
        {
            HandleMovement();
            HandleSpecialAction();
        }

        public override void HandleMovement()
        {
            // Get the input for movement
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Determine the direction based on the input
            Vector2 direction = Vector2.zero;

            if (moveHorizontal > 0)
            {
                direction = Vector2.right; // East
                transform.rotation = Quaternion.Euler(0, 0, 0); // Rotate to face East
            }
            else if (moveHorizontal < 0)
            {
                direction = Vector2.left; // West
                transform.rotation = Quaternion.Euler(0, 0, 180); // Rotate to face West
            }
            else if (moveVertical > 0)
            {
                direction = Vector2.up; // North
                transform.rotation = Quaternion.Euler(0, 0, 90); // Rotate to face North
            }
            else if (moveVertical < 0)
            {
                direction = Vector2.down; // South
                transform.rotation = Quaternion.Euler(0, 0, 270); // Rotate to face South
            }

            // Move the vehicle in the determined direction
            transform.Translate(direction * VehicleData.Speed * Time.deltaTime, Space.World);
        }
        
        public override void HandleSpecialAction()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Collider2D hitCollider = Physics2D.OverlapCircle(_crushPoint.position, 0.1f); // Reduced radius for precision
                if (hitCollider != null)
                {
                    if (hitCollider.CompareTag("CrushableCell"))
                    {
                        Destroy(hitCollider.gameObject);
                    }
                }
            }
        }
    }
}