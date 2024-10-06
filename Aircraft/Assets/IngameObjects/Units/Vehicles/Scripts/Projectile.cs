using Buildings;
using Objects;
using Objects.Vehicles;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _lifeTime = 5f; 
    private int _damage;
    private bool _isFromEnemy;

    public void Initialize(Vector2 p_initialVelocity, int p_damage, bool p_isFromEnemy)
    {
        _damage = p_damage;
        gameObject.GetComponent<Rigidbody2D>().velocity = p_initialVelocity;
        _isFromEnemy = p_isFromEnemy;
        Destroy(gameObject, _lifeTime); 
    }

    private void OnTriggerEnter2D(Collider2D p_collision)
    {
        if (p_collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        
        if (!_isFromEnemy)
        {
            if (p_collision.CompareTag("Enemy"))
            {
                var enemy = p_collision.GetComponentInParent<Unit>();
                if (enemy != null)
                {
                    enemy.ReceiveDamage(_damage);
                }

                Destroy(gameObject);
            } 
        }
        else
        {
            if (p_collision.CompareTag("Vehicle") || p_collision.CompareTag("Building"))
            {
                var unit = p_collision.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.ReceiveDamage(_damage);
                }

                Destroy(gameObject);
            } 
        }
    }
}