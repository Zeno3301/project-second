using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 2f;
    
    public UnityEvent onDeath;
    public UnityEvent<int> onHealthChanged;

    private int currentHealth;
    private bool isInvincible;
    private float invincibilityTimer;

    private void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }

    private void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        onHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartInvincibility();
        }
    }

    private void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    private void Die()
    {
        onDeath?.Invoke();
        gameObject.SetActive(false);
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
        gameObject.SetActive(true);
    }
} 