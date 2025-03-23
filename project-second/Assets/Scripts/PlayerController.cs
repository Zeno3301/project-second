using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    public Transform[] gunPoints;
    public float bulletSpeed = 10f;
    public float bulletSize = 0.3f;

    private float nextFireTime;
    private Camera mainCamera;
    private float minX, maxX, minY, maxY;
    private Rigidbody2D rb;

    private void Start()
    {
        // Получаем или добавляем Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        // Настраиваем физику
        rb.gravityScale = 0f;
        rb.linearDamping = 5f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        // Настраиваем границы движения
        mainCamera = Camera.main;
        CalculateScreenBounds();

        // Убеждаемся, что у объекта правильные настройки
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 1; // Поверх фона
        }
    }

    private void Update()
    {
        // Обработка стрельбы
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void FixedUpdate()
    {
        // Обработка движения в FixedUpdate для более стабильной физики
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveX, moveY);
        
        // Нормализуем движение по диагонали
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Применяем движение
        rb.linearVelocity = movement * moveSpeed;

        // Ограничиваем позицию
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;
    }

    private void CalculateScreenBounds()
    {
        // Получаем границы экрана в мировых координатах
        Vector2 screenBounds = mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z)
        );

        // Получаем размеры спрайта для точного ограничения
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float spriteWidth = 0;
        float spriteHeight = 0;
        
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            spriteWidth = spriteRenderer.bounds.extents.x;
            spriteHeight = spriteRenderer.bounds.extents.y;
        }
        else
        {
            spriteWidth = transform.localScale.x / 2;
            spriteHeight = transform.localScale.y / 2;
        }

        // Устанавливаем границы с учетом размеров спрайта
        minX = -screenBounds.x + spriteWidth;
        maxX = screenBounds.x - spriteWidth;
        minY = -screenBounds.y + spriteHeight;
        maxY = screenBounds.y - spriteHeight;
    }

    private void Shoot()
    {
        if (gunPoints != null && gunPoints.Length > 0 && bulletPrefab != null)
        {
            foreach (Transform gunPoint in gunPoints)
            {
                // Создаем пулю
                GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
                
                // Настраиваем размер пули
                bullet.transform.localScale = new Vector3(bulletSize, bulletSize, 1f);
                
                // Получаем или добавляем Rigidbody2D к пуле
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                if (bulletRb == null)
                {
                    bulletRb = bullet.AddComponent<Rigidbody2D>();
                    bulletRb.gravityScale = 0f;
                    bulletRb.bodyType = RigidbodyType2D.Kinematic;
                    bulletRb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                
                // Устанавливаем скорость пули
                bulletRb.linearVelocity = Vector2.up * bulletSpeed;
                
                // Уничтожаем пулю через 3 секунды
                Destroy(bullet, 3f);
            }
        }
    }
} 