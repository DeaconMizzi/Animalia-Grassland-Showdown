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
        var damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        // Optional: if this should only damage one target
        Destroy(gameObject);
    }
}
