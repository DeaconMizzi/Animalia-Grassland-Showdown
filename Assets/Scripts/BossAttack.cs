using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float lifetime = 1.5f; // Lifetime of the attack
    public int damage = 1;        // Damage dealt by the attack
    public float speed = 5f;      // Movement speed (used for Swipe and Wave)
    public bool isWave = false;   // Determines if the attack is a wave
    public float waveAmplitude = 1f; // Wave motion amplitude
    public float waveFrequency = 2f; // Wave motion frequency

    private Vector2 direction;
    private Vector2 startPosition;

    void Start()
    {
        // Destroy the attack after its lifetime
        Destroy(gameObject, lifetime);

        // For Swipe and Wave, calculate the direction toward the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
        }
        else
        {
            direction = Vector2.right; // Default to moving right
        }

        // Store starting position for wave motion
        startPosition = transform.position;
    }

    void Update()
    {
        // Handle movement logic
        if (isWave)
        {
            float waveY = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
            transform.position = new Vector2(transform.position.x + direction.x * speed * Time.deltaTime, startPosition.y + waveY);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the attack hits the player
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Apply damage to the player
                Debug.Log("Player hit by attack! Damage: " + damage);
            }

            // Destroy the attack after it hits the player
            Destroy(gameObject);
        }
    }
}
