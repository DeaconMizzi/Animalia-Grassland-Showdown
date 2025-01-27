using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;

    [Header("Damage Overlay")]
    public string damageOverlayName = "DamageOverlay"; // Name of the overlay GameObject in the hierarchy
    public float overlayDuration = 0.2f;  // Duration of the red screen flash
    public float fadeSpeed = 5f;  // Speed of fade-out for the overlay

    [Header("Invincibility")]
    public float invincibilityDuration = 2f;  // Duration of invincibility after taking damage
    public float flashInterval = 0.1f;        // Interval for flashing
    private bool isInvincible = false;

    private GameObject damageOverlay; // Reference to the overlay GameObject
    private Image overlayImage;
    private SpriteRenderer spriteRenderer;    // Reference to the player's sprite renderer

    void Start()
    {
        currentHealth = maxHealth;

        // Find the HealthUI script in the scene if it's not assigned
        if (healthUI == null)
        {
            healthUI = FindObjectOfType<HealthUI>();
        }

        healthUI.InitializeHearts(maxHealth);

        // Automatically find the damage overlay by its name
        damageOverlay = GameObject.Find(damageOverlayName);
        if (damageOverlay != null)
        {
            overlayImage = damageOverlay.GetComponent<Image>();
            damageOverlay.SetActive(false); // Ensure it's inactive at the start
        }
        else
        {
            Debug.LogError($"Damage overlay with name '{damageOverlayName}' not found!");
        }

        // Get the player's SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Player SpriteRenderer is not assigned or missing!");
        }
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
        if (isInvincible) return; // Ignore damage if invincible

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        healthUI.UpdateHearts(currentHealth);

        if (damageOverlay != null)
        {
            StartCoroutine(FlashDamageOverlay());
        }

        StartCoroutine(InvincibilityCoroutine());

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
        SceneManager.LoadScene("Lose");
    }

    private IEnumerator FlashDamageOverlay()
    {
        if (overlayImage == null)
        {
            Debug.LogError("Overlay Image is missing!");
            yield break;
        }

        damageOverlay.SetActive(true);

        // Make the overlay fully visible
        overlayImage.color = new Color(1, 0, 0, 0.5f);

        // Wait for the overlay duration
        yield return new WaitForSeconds(overlayDuration);

        // Gradually fade out the overlay
        while (overlayImage.color.a > 0)
        {
            Color newColor = overlayImage.color;
            newColor.a -= Time.deltaTime * fadeSpeed;
            overlayImage.color = newColor;
            yield return null;
        }

        damageOverlay.SetActive(false);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            // Toggle the sprite renderer's visibility
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }

        // Ensure the sprite is visible after invincibility ends
        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}
