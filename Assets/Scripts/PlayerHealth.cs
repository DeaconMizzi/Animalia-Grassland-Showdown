using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;

    [Header("Damage Overlay")]
    public string damageOverlayName = "DamageOverlay";
    public float overlayDuration = 0.2f;
    public float fadeSpeed = 5f;

    [Header("Invincibility")]
    public float invincibilityDuration = 2f;
    public float flashInterval = 0.1f;
    private bool isInvincible = false;

    private GameObject damageOverlay;
    private Image overlayImage;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthUI == null)
        {
            healthUI = FindObjectOfType<HealthUI>();
        }

        healthUI.InitializeHearts(maxHealth);

        damageOverlay = GameObject.Find(damageOverlayName);
        if (damageOverlay != null)
        {
            overlayImage = damageOverlay.GetComponent<Image>();
            damageOverlay.SetActive(false);
        }
        else
        {
            Debug.LogError($"Damage overlay with name '{damageOverlayName}' not found!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Player SpriteRenderer is missing!");
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H)) TakeDamage(1);
        if (Input.GetKeyDown(KeyCode.J)) Heal(1);
#endif
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        healthUI.UpdateHearts(currentHealth);

        if (overlayImage != null)
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
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        healthUI.UpdateHearts(currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player Died!");
        gameObject.SetActive(false); // Instead of Destroy
        SceneManager.LoadScene("Lose");
    }

    private IEnumerator FlashDamageOverlay()
    {
        damageOverlay.SetActive(true);
        overlayImage.color = new Color(1, 0, 0, 0.5f);

        float elapsedTime = 0f;
        float startAlpha = overlayImage.color.a;

        while (elapsedTime < overlayDuration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, 0, elapsedTime / overlayDuration);
            overlayImage.color = new Color(1, 0, 0, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        overlayImage.color = new Color(1, 0, 0, 0);
        damageOverlay.SetActive(false);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}
