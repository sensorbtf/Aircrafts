﻿using UnityEngine;
using UnityEngine.UI;

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
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

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

            if (direction != Vector2.zero)
            {
                Vector2 newPosition = (Vector2)transform.position + direction * VehicleData.Speed * Time.deltaTime;
                Rigidbody2D.MovePosition(newPosition);
            }
        }

        private SpriteRenderer renderer = null;

        public override void HandleSpecialAction()
        {
            var hitCollider = Physics2D.OverlapCircle(_crushPoint.position, 0.1f); // Reduced radius for precision
            if (hitCollider != null && hitCollider.CompareTag("CrushableCell"))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                        Destroy(hitCollider.gameObject);
                }
                else
                {
                    if (renderer != null)
                    {
                        renderer.color = Color.white;
                        renderer = null;
                    }

                    renderer = hitCollider.gameObject.GetComponent<SpriteRenderer>();
                    renderer.color = Color.red;
                }
            }
            else
            {
                if (renderer != null)
                {
                    renderer.color = Color.white;
                    renderer = null;
                }
            }
        }
    }
}