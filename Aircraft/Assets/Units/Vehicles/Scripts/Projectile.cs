using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _lifeTime = 5f; 
    private int _damage; 

    private void Start()
    {
        Destroy(gameObject, _lifeTime); 
    }

    public void Initialize(Vector2 p_initialVelocity, int p_damage)
    {
        _damage = p_damage;
        gameObject.GetComponent<Rigidbody2D>().velocity = p_initialVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<Enemies.Enemy>();
            if (enemy != null)
            {
                enemy.ReceiveDamage(_damage);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}