using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, IPointerClickHandler
    {
        public EnemySO EnemyData { get; private set; }
        public Rigidbody2D Rigidbody2D { get; private set; }
        public Transform AttackPoint;  // The point from which the attack range is calculated


        internal Action<Enemy> OnEnemyClicked;

        private Transform _currentTarget;  // To keep track of the current target
        private float _attackCooldown;  // Timer to manage the attack interval

        public void Initialize(EnemySO p_enemyData)
        {
            EnemyData = p_enemyData;
            Rigidbody2D = GetComponent<Rigidbody2D>();
            // Rigidbody2D.drag = EnemyData.Drag;  // Adjust drag to control sliding
            // Rigidbody2D.angularDrag = EnemyData.AngularDrag;  // Control rotational drag
        }

        public virtual void HandleMovement(Transform p_playerBase)
        {
            if (_currentTarget == null)
            {
                // Move towards the player base
                Vector2 direction = (p_playerBase.position - transform.position).normalized;
                MoveTowards(direction);
            }
            else
            {
                // Stop moving when attacking
                StopMovement();

                // Rotate towards the current target
                RotateTowards();

                // Handle attack if in range
                HandleAttackCooldown();
            }
        }

        private void MoveTowards(Vector2 direction)
        {
            Rigidbody2D.AddForce(direction * EnemyData.Speed, ForceMode2D.Force);
        }

        private void StopMovement()
        {
            // Stop the enemy's movement by setting the velocity to zero
            Rigidbody2D.velocity = Vector2.zero;
        }

        private void RotateTowards()
        {
            var direction = (Vector2)_currentTarget.position - (Vector2)AttackPoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        private void HandleAttackCooldown()
        {
            _attackCooldown -= Time.deltaTime;

            if (_attackCooldown <= 0f && _currentTarget != null)
            {
                HandleAttack(_currentTarget.gameObject);
                _attackCooldown = EnemyData.AttackCooldown;
            }
        }

        public virtual void HandleAttack(GameObject target)
        {
            Debug.Log($"Attacking {target.name}");
            // Implement specific attack logic here, like reducing target health
        }

        public virtual void HandleSpecialAction()
        {
            var hitCollider = Physics2D.OverlapCircle(AttackPoint.position, 0.1f); 
            if (hitCollider != null && hitCollider.gameObject.CompareTag("Building") || 
                hitCollider.gameObject.CompareTag("Vehicle"))
            {
                _currentTarget = hitCollider.transform;
            }
            else
            {
                _currentTarget = null;
            }
        }

        public void OnPointerClick(PointerEventData p_eventData)
        {
            OnEnemyClicked?.Invoke(this);
        }
    }
}
