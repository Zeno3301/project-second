using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private bool isEnemyBullet = false;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Движение пули вверх (для игрока) или вниз (для врагов)
        float direction = isEnemyBullet ? -1f : 1f;
        transform.Translate(Vector3.up * speed * direction * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnemyBullet && other.CompareTag("Player"))
        {
            // Логика нанесения урона игроку
            var player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (!isEnemyBullet && other.CompareTag("Enemy"))
        {
            // Логика нанесения урона врагу
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
} 