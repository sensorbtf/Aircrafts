using Buildings;
using Objects;
using Objects.Vehicles;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _lifeTime = 5f; 
    private int _damage;
    private bool _isFromEnemy;

    private void Start()
    {
        _isFromEnemy = false;
        Destroy(gameObject, _lifeTime); 
    }

    public void Initialize(Vector2 p_initialVelocity, int p_damage, bool p_isFromEnemy)
    {
        _damage = p_damage;
        gameObject.GetComponent<Rigidbody2D>().velocity = p_initialVelocity;
        _isFromEnemy = p_isFromEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        
        if (!_isFromEnemy)
        {
            if (collision.CompareTag("Enemy"))
            {
                var enemy = collision.GetComponent<Unit>();
                if (enemy != null)
                {
                    enemy.ReceiveDamage(_damage);
                }

                Destroy(gameObject);
            } 
        }
        else
        {
            if (collision.CompareTag("Vehicle") || collision.CompareTag("Building"))
            {
                var unit = collision.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.ReceiveDamage(_damage);
                }

                Destroy(gameObject);
            } 
        }
    }
}