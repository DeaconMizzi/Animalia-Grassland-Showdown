using System.Collections;
using UnityEngine;

public class SkeletonAI : MonoBehaviour, IDamageable
{
    public float moveSpeed = 2f;
    public float patrolRange = 3f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public int maxHealth = 2;

    public Transform attackPoint;
    public LayerMask playerLayer;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 startPosition;
    private bool movingRight = true;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private int currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        currentHealth = maxHealth;

        attackPoint.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isAttacking) return;

        Collider2D player = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (player != null && Time.time >= lastAttackTime + attackCooldown)
        {
            StartCoroutine(Attack());
            return;
        }

        Patrol();
    }

    void Patrol()
    {
        float distance = transform.position.x - startPosition.x;

        if (movingRight && distance > patrolRange)
            Flip();
        else if (!movingRight && distance < -patrolRange)
            Flip();

        if (IsGroundAhead())
        {
            rb.velocity = new Vector2(moveSpeed * (movingRight ? 1 : -1), rb.velocity.y);
            animator.SetBool("isRunning", true);
        }
        else
        {
            Flip();
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack");

        lastAttackTime = Time.time;
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public void EnableAttack()
    {
        attackPoint.gameObject.SetActive(true);
    }

    public void DisableAttack()
    {
        attackPoint.gameObject.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        animator.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // Stop physics effects
        GetComponent<Collider2D>().enabled = false; // Optional: disable body collider
        attackPoint.gameObject.SetActive(false); // Ensure no attack collider active

        yield return new WaitForSeconds(1f); // Adjust to match your death animation length

        Destroy(gameObject);
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(movingRight ? 1 : -1, 1, 1);
    }

    bool IsGroundAhead()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
