using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float moveSpeed = 5f;  // Speed for left/right movement
    public float jumpForce = 7f;  // Force applied for jumping
    public int maxJumps = 2;      // Maximum number of jumps allowed

    [Header("Attack Parameters")]
    public GameObject attackPrefab;  // Prefab for the attack (e.g., melee or projectile)
    public Transform attackPoint;    // The point where the attack spawns
    public float attackCooldown = 0.5f;  // Cooldown between attacks

    private Rigidbody2D rb;
    private int jumpCount;
    private float lastAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps;
    }

    void Update()
    {
        // Horizontal Movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
        }

        // Attacking
        if (Input.GetKeyDown(KeyCode.K) && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        jumpCount = maxJumps;
    }

    void PerformAttack()
    {
        if (attackPrefab != null && attackPoint != null)
        {
            // Check if an attack is already active
            if (attackPoint.childCount == 0)
            {
                // Spawn the punch as a child of the attackPoint
                GameObject attack = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity, attackPoint);

                // Optional: Set the local position to ensure it aligns correctly with the player's facing direction
                attack.transform.localPosition = Vector3.right * 0.5f;

                Debug.Log("Punch performed!");
            }
        }
    }
}
