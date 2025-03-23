using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float padding = 0.5f; // Отступ от краев экрана

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private Transform[] gunPoints; // Точки, откуда вылетают пули

    private float nextFireTime;
    private Camera mainCamera;
    private float minX, maxX, minY, maxY;

    private void Start()
    {
        mainCamera = Camera.main;
        CalculateMovementBounds();
    }

    private void Update()
    {
        Move();
        HandleShooting();
    }

    private void CalculateMovementBounds()
    {
        Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        minX = -screenBounds.x + padding;
        maxX = screenBounds.x - padding;
        minY = -screenBounds.y + padding;
        maxY = screenBounds.y - padding;
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;

        // Ограничиваем движение в пределах экрана
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;
    }

    private void HandleShooting()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        foreach (Transform gunPoint in gunPoints)
        {
            Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        }
    }
} 