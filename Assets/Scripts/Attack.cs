using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float lifetime = 0.5f;  // How long the punch stays active
    public int damage = 10;       // Damage dealt by the punch

    void Start()
    {
        // Destroy the attack after its lifetime
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for collision with enemies
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit!");
            // Optional: Call a method on the enemy to reduce health
            // collision.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
