using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float lifetime = 0.5f;  // How long the attack stays active
    public int damage = 1;        // Damage dealt by the attack (adjust as needed)

    void Start()
    {
        // Destroy the attack after its lifetime
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for collision with the boss
        if (collision.CompareTag("Boss"))
        {
            Debug.Log("Boss hit!");
            // Reduce boss health
            BossController boss = collision.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
        }
    }
}
