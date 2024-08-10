using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _lifeTime = 5f; 

    private void Start()
    {
        Destroy(gameObject, _lifeTime); 
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