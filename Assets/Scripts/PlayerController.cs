using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum CharacterType { Cheetah, Bunny } // Character type enum
    public CharacterType characterType;         // Character type

    [Header("Movement Parameters")]
    public float moveSpeed = 5f;  // Speed for left/right movement
    public float jumpForce = 7f;  // Force applied for jumping
    public int maxJumps = 2;      // Max jumps (for Cheetah and Bunny)

    [Header("Dash Parameters (Cheetah Only)")]
    public float dashSpeed = 15f;     // Dash speed
    public float dashDuration = 0.2f; // Dash duration
    public float dashCooldown = 1f;   // Dash cooldown

    [Header("Attack Parameters")]
    public GameObject attackPrefab;  // Prefab for the attack (e.g., melee or projectile)
    public Transform attackPoint;    // The point where the attack spawns
    public float attackCooldown = 0.5f;  // Cooldown between attacks

    private Rigidbody2D rb;
    private int jumpCount;
    private float lastAttackTime = 0f;
    private bool isDashing = false;
    private float lastDashTime = 0f;

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

        // Flip player based on movement direction
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Facing right
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Facing left
        }

        // Character-specific abilities
        if (characterType == CharacterType.Cheetah)
        {
            HandleDash();
            HandleDoubleJump();
        }
        else if (characterType == CharacterType.Bunny)
        {
            HandleHighJump();
        }

        // Attacking
        if (Input.GetKeyDown(KeyCode.K) && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    private void HandleDash()
    {
        // Dash logic for Cheetah
        if (Input.GetKeyDown(KeyCode.L) && Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            StartCoroutine(Dash());
            lastDashTime = Time.time;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale; // Store original gravity
        rb.gravityScale = 0;                     // Disable gravity during dash

        // Determine dash direction based on player's facing direction
        float dashDirection = transform.localScale.x > 0 ? 1 : -1; // 1 = Right, -1 = Left
        rb.velocity = new Vector2(dashDirection * dashSpeed, 0);   // Dash in that direction

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity; // Restore gravity
        isDashing = false;
    }

    private void HandleDoubleJump()
    {
        // Double jump logic for Cheetah
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
        }
    }

    private void HandleHighJump()
    {
        // High jump logic for Bunny
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.5f); // Higher jump multiplier for Bunny
            jumpCount--;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset jumps for both characters
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
