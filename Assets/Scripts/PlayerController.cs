using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum CharacterType { Cheetah, Bunny }
    public CharacterType characterType;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public int maxJumps = 2;

    [Header("Dash (Cheetah only)")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Attack")]
    public GameObject attackPrefab;
    public Transform attackPoint;
    public float attackCooldown = 0.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private int jumpCount;
    private float lastAttackTime = 0f;
    private bool isDashing = false;
    private float lastDashTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpCount = maxJumps;
    }

    void Update()
    {
        UpdateAnimatorStates();

        HandleMovement();
        HandleDirection();
        HandleJump();
        HandleAttack();
    }

    void FixedUpdate()
    {
        if (IsGrounded())
        {
            jumpCount = maxJumps;
        }
    }

    void UpdateAnimatorStates()
    {
        animator.SetBool("isGrounded", IsGrounded());
        float moveInput = Input.GetAxis("Horizontal");
        animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0);
    }

    void HandleMovement()
    {
        if (isDashing) return;

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void HandleDirection()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            attackPoint.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            attackPoint.localScale = new Vector3(-1, 1, 1);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            float jumpMultiplier = (characterType == CharacterType.Bunny) ? 1.5f : 1f;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * jumpMultiplier);
            jumpCount--;

            animator.SetTrigger("Jump");
        }
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.K) && Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Punch");
            lastAttackTime = Time.time;
            StartCoroutine(DelayedAttack());
        }

        if (characterType == CharacterType.Cheetah)
        {
            HandleDash();
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.L) && Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            StartCoroutine(Dash());
            lastDashTime = Time.time;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        animator.SetBool("isDashing", true); // Trigger dash animation

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        float dashDirection = transform.localScale.x > 0 ? 1 : -1;
        rb.velocity = new Vector2(dashDirection * dashSpeed, 0);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
        animator.SetBool("isDashing", false); // Return to idle/run
    }

    private IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(0.1f);

        if (attackPrefab != null && attackPoint != null && attackPoint.childCount == 0)
        {
            GameObject attack = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity, attackPoint);
            float attackOffsetX = 0.2f;
            float direction = transform.localScale.x > 0 ? 1 : -1;
            attack.transform.localPosition = new Vector3(attackOffsetX * direction, 0, 0);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
