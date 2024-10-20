﻿using UnityEngine;
using UnityEngine.UI;

namespace Objects.Vehicles
{
    public class UndergroundVehicle : Vehicle
    {
        [SerializeField] private Transform _crushPoint;
        private SpriteRenderer _cellSprite;

        public override void SelectedFixedUpdate()
        {
            base.SelectedFixedUpdate();
            
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

        public override void SelectedUpdate()
        {
            base.SelectedUpdate();
            
            var hitCollider = Physics2D.OverlapCircle(_crushPoint.position, 0.1f); 
            if (hitCollider != null && hitCollider.CompareTag("CrushableCell"))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                        Destroy(hitCollider.gameObject);
                }
                else
                {
                    if (_cellSprite != null)
                    {
                        _cellSprite.color = Color.white;
                        _cellSprite = null;
                    }

                    _cellSprite = hitCollider.gameObject.GetComponent<SpriteRenderer>();
                    _cellSprite.color = Color.red;
                }
            }
            else
            {
                if (_cellSprite != null)
                {
                    _cellSprite.color = Color.white;
                    _cellSprite = null;
                }
            }
        }
    }
}