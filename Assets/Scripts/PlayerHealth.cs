using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;

    void Start()
    {
        currentHealth = maxHealth;

        // Find the HealthUI script in the scene if it's not assigned
        if (healthUI == null)
        {
            healthUI = FindObjectOfType<HealthUI>();
        }

        healthUI.InitializeHearts(maxHealth);
    }

    void Update()
    {
        // Simulate taking damage with the "H" key
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }

        // Simulate healing with the "J" key
        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        healthUI.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthUI.UpdateHearts(currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        Destroy(gameObject);  // Destroy the player GameObject
    }
}