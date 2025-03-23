using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int scoreValue = 100;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float amplitude = 1f; // Для волнообразного движения
    [SerializeField] private float frequency = 1f; // Для волнообразного движения

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float minFireRate = 1f;
    [SerializeField] private float maxFireRate = 3f;
    [SerializeField] private Transform[] gunPoints;

    private int currentHealth;
    private float nextFireTime;
    private Vector3 startPosition;
    private float sinOffset;

    private void Start()
    {
        currentHealth = maxHealth;
        sinOffset = Random.Range(0f, 2f * Mathf.PI); // Случайное смещение для разнообразия движения
        startPosition = transform.position;
        SetNextFireTime();
    }

    private void Update()
    {
        Move();
        HandleShooting();
    }

    private void Move()
    {
        // Базовое движение вниз
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        // Волнообразное движение влево-вправо
        float horizontalOffset = Mathf.Sin((Time.time + sinOffset) * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x + horizontalOffset, transform.position.y, transform.position.z);

        // Уничтожаем врага, если он вышел за пределы экрана
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void HandleShooting()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            SetNextFireTime();
        }
    }

    private void Shoot()
    {
        foreach (Transform gunPoint in gunPoints)
        {
            Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        }
    }

    private void SetNextFireTime()
    {
        nextFireTime = Time.time + Random.Range(minFireRate, maxFireRate);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Добавить очки игроку
        GameManager.Instance.AddScore(scoreValue);
        
        // TODO: Создать эффект взрыва
        
        Destroy(gameObject);
    }
} 