using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float moveSpeed = 5f; // Speed for left/right movement
    public float jumpForce = 7f; // Force applied for jumping
    public int maxJumps = 2;     // Maximum number of jumps allowed

    private Rigidbody2D rb;
    private int jumpCount;       // Tracks the current number of jumps

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Reference Rigidbody2D
        jumpCount = maxJumps;             // Initialize jump count
    }

    void Update()
    {
        // Horizontal Movement
        float moveInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow Keys
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--; // Decrease jump count
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset jump count when touching any collider
        jumpCount = maxJumps;
    }
}
