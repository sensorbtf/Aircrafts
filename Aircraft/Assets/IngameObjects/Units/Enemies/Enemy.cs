using System;
using Buildings;
using Objects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vehicles;

namespace Enemies
{
    public abstract class Enemy: Unit
    {
        [SerializeField] private EnemySO _enemyData;

        public EnemySO EnemyData => _enemyData;
        public Transform AttackPoint;

        public void Initialize(EnemySO p_enemyData)
        {
            _enemyData = p_enemyData;
            UnitData = p_enemyData;
    
            base.Initialize(p_enemyData);
        }       

        public override void AttackTarget(GameObject p_target)
        {
            Debug.Log($"Attacking {p_target.name}");
            OnUnitAttack?.Invoke(this, p_target.GetComponent<Unit>());
        }

        public virtual void HandleMovement(Transform p_playerBase)
        {

        }

        public virtual void HandleSpecialAction()
        {

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
            Inventory.CreateItemsOnDestroy(transform);

            Destroy(gameObject);
        }
    }
}