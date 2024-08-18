using System;
using Buildings;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vehicles;

namespace Enemies
{
    public abstract class Enemy : Unit, IPointerClickHandler
    {
        public EnemySO EnemyData { get; private set; }
        public Transform AttackPoint;

        private Transform _currentTarget;
        private float _attackCooldown;

        public void Initialize(EnemySO p_enemyData)
        {
            EnemyData = p_enemyData;
            UnitData = p_enemyData;
    
            base.Initialize(p_enemyData);
        }

        public virtual void HandleMovement(Transform p_playerBase)
        {
            if (_currentTarget == null)
            {
                Vector2 direction = (p_playerBase.position - transform.position).normalized;
                MoveTowards(direction);
            }
            else
            {
                StopMovement();
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

        private void RotateTowards(Vector3 p_position)
        {
            var direction = (Vector2)p_position - (Vector2)AttackPoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        private void HandleAttackCooldown()
        {
            _attackCooldown -= Time.deltaTime;

            if (_attackCooldown <= 0f && _currentTarget != null)
            {
                AttackTarget(_currentTarget.gameObject);
                _attackCooldown = EnemyData.AttackCooldown;
            }
        }

        public override void AttackTarget(GameObject p_target)
        {
            Debug.Log($"Attacking {p_target.name}");
            OnUnitAttack?.Invoke(this, p_target.GetComponent<Unit>());
        }

        public virtual void HandleSpecialAction()
        {
            var hitCollider = Physics2D.OverlapCircle(AttackPoint.position, 0.1f);
            if (hitCollider != null && (hitCollider.gameObject.CompareTag("Building") ||
                                        hitCollider.gameObject.CompareTag("Vehicle")))
            {
                _currentTarget = hitCollider.transform;
            }
            else
            {
                _currentTarget = null;
            }
        }

        public override void ReceiveDamage(int p_damage)
        {
            CurrentHp -= p_damage;
            CanvasInfo.HealthBar.value = CurrentHp;

            if (CurrentHp <= 0)
            {
                OnUnitDied?.Invoke(this);
            }
        }
        
        public override void CheckState()
        {
           // for attacking, states such as sleeping?
        }

        public void OnPointerClick(PointerEventData p_eventData)
        {
            OnUnitClicked?.Invoke(this, true);
        }
        
        public override void DestroyHandler()
        {
            Destroy(gameObject);
        }
    }
}