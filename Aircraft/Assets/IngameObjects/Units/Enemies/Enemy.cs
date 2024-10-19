using System;
using System.Collections.Generic;
using Buildings;
using Objects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Vehicles;

namespace Enemies
{
    public class Enemy: Unit
    {
        [SerializeField] private EnemySO _enemyData;
        [SerializeField] private MonoBehaviour[] _monoComponents;

        private IEnemyMovementComponent[] _movementComponents;
        private IEnemyCombatComponent[] _combatComponents;
        private bool _isAttacking;

        internal Unit CurrentTarget;
        public EnemySO EnemyData => _enemyData;

        public void Initialize(EnemySO p_enemyData)
        {
            _enemyData = p_enemyData;
            UnitData = p_enemyData;

            var movementList = new List<IEnemyMovementComponent>();
            var combatList = new List<IEnemyCombatComponent>();

            for (int i = 0; i < _monoComponents.Length; i++)
            {
                if (_monoComponents[i] is IEnemyMovementComponent movement)
                {
                    movementList.Add(movement);
                }
                else if (_monoComponents[i] is IEnemyCombatComponent attack)
                {
                    combatList.Add(attack);
                }
            }

            _movementComponents = movementList.ToArray();
            _combatComponents = combatList.ToArray();

            base.Initialize(p_enemyData);
        }

        public override void AttackTarget(GameObject p_target)
        {
            Debug.Log($"Attacking {p_target.name}");
            OnUnitAttack?.Invoke(this, p_target.GetComponent<Unit>());
        }

        public void HandleMovement(Transform p_nearestPlayerUnit)
        {
            foreach (var component in _movementComponents)
            {
                if (!_isAttacking)
                {
                    component.PhysicUpdate(p_nearestPlayerUnit, Rigidbody2D, CurrentTarget, _enemyData.Speed, _isAttacking);
                }
            }
        }

        public virtual void HandleSpecialAction()
        {
            foreach (var component in _combatComponents)
            {
                if (!component.IsMain && CurrentTarget != null) // priorytety główne ataki
                    continue;

                CurrentTarget = component.TryToDetectUnit();
                
                if (CurrentTarget != null)
                {
                    component.AttackUpdate(_enemyData.AttackCooldown, _enemyData.AttackDamage, CurrentTarget, out var p_isAttacking);
                    _isAttacking = p_isAttacking;
                }
            }
        }

        public override void ReceiveDamage(int p_damage)
        {
            CurrentHp -= p_damage;
            CanvasInfo.HealthBar.value = CurrentHp;

            float pushbackForce = p_damage;
            Rigidbody2D.AddForce(new Vector2(pushbackForce, 0f), ForceMode2D.Impulse);

            if (CurrentHp <= 0)
            {
                OnUnitDied?.Invoke(this);
            }
        }

        public override void Repair(int p_repaired)
        {
        }

        public override void CheckState()
        {
            // for attacking, states such as sleeping?
        }

        public override void OnPointerClick(PointerEventData p_eventData)
        {
            OnUnitClicked?.Invoke(this, true);
        }

        public override void DestroyHandler()
        {
            Destroy(gameObject);
        }
    }
}