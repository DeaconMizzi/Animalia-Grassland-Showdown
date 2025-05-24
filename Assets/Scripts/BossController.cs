using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour, IDamageable
{
    [Header("Boss Health")]
    public int maxHealth = 10;
    [HideInInspector] public int currentHealth;

    [Header("Attack Parameters")]
    public float minAttackInterval = 0.5f;
    public float maxAttackInterval = 1f;
    public float chargeTime = 0.8f;
    public Transform attackPoint;
    public GameObject swipePrefab;
    public GameObject spikesPrefab;
    public GameObject wavePrefab;
    public GameObject chargeGlow;
    public float hopDistance = 3f;

    [Header("Environment")]
    public LayerMask groundLayer;

    private bool isAlive = true;
    private Animator animator;
    private Vector3 playerLastPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        StartCoroutine(AttackCycle());
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;
        currentHealth -= damage;
        Debug.Log("Boss hit! Current Health: " + currentHealth);
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Boss Defeated!");
        isAlive = false;
        StopAllCoroutines();
        Destroy(gameObject, 1f);
        SceneManager.LoadScene("Win");
    }

    private IEnumerator AttackCycle()
    {
        float moveCooldown = 2f; // Boss moves every 2 seconds
        float moveTimer = moveCooldown;

        while (isAlive)
        {
            float attackInterval = Random.Range(minAttackInterval, maxAttackInterval);
            yield return new WaitForSeconds(attackInterval);

            PerformAttack(); // Attack almost all the time

            moveTimer -= attackInterval;

            if (moveTimer <= 0f)
            {
                MoveBoss();
                moveTimer = moveCooldown; // Reset the move timer
            }
        }
    }

    private void PerformAttack()
    {
        int attackType = Random.Range(0, 3);

        if (animator != null)
            animator.ResetTrigger("Attack");

        if (animator != null)
            animator.SetTrigger("Attack");

        // Capture player position for wave targeting
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerLastPosition = player.transform.position;

        switch (attackType)
        {
            case 0:
                StartCoroutine(SwipeBurstAttack());
                break;
            case 1:
                SpikesAttack();
                break;
            case 2:
                StartCoroutine(SpawnWaveAttack());
                break;
        }
    }

    private IEnumerator SwipeBurstAttack()
    {
        Debug.Log("Swipe Burst Attack!");

        int swipeCount = Random.Range(3, 6); // Number of swipes per burst
        float delayBetweenSwipes = 0.2f;

        for (int i = 0; i < swipeCount; i++)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) yield break;

            Vector2 direction = (player.transform.position - attackPoint.position).normalized;

            GameObject attack = Instantiate(swipePrefab, attackPoint.position, Quaternion.identity);
            BossAttack bossAttack = attack.GetComponent<BossAttack>();
            if (bossAttack != null)
            {
                bossAttack.isWave = false;
                bossAttack.isStationary = false;
                bossAttack.SetDirection(direction);
                bossAttack.speed = 6f; // Adjust speed as needed
            }

            yield return new WaitForSeconds(delayBetweenSwipes);
        }
    }

    private void SpikesAttack()
    {
        Debug.Log("Spikes Attack!");
        StartCoroutine(SpawnSpikes());
    }

    private IEnumerator SpawnSpikes()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null) yield break;

        Vector3 playerGroundPos = new Vector3(playerObj.transform.position.x, 0, 0);
        RaycastHit2D baseGroundHit = Physics2D.Raycast(playerObj.transform.position, Vector2.down, 20f, groundLayer);
        if (baseGroundHit.collider != null)
            playerGroundPos.y = baseGroundHit.point.y;
        else
            yield break;

        float step = 1.5f;
        float initialDistance = 8f;
        float delay = 0.3f;

        float direction = transform.localScale.x > 0 ? 1f : -1f;
        float totalDistance = Mathf.Abs(playerGroundPos.x - transform.position.x);
        int spikeCount = Mathf.CeilToInt((initialDistance + totalDistance) / step);

        for (int i = 0; i < spikeCount; i++)
        {
            float xOffset = direction * (initialDistance - i * step);
            Vector3 checkPos = new Vector3(playerGroundPos.x + xOffset, playerGroundPos.y + 1f, 0);

            RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, 2f, groundLayer);
            if (hit.collider != null)
            {
                float yOffset = +0.4f;
                Vector3 spikePos = new Vector3(checkPos.x, hit.point.y + yOffset, attackPoint.position.z);
                Instantiate(spikesPrefab, spikePos, Quaternion.identity);
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator SpawnWaveAttack()
    {
        Debug.Log("Wave Attack!");

        int minWaves = 2;
        int maxWaves = 5;
        int waveCount = Random.Range(minWaves, maxWaves + 1);
        float delayBetweenWaves = Random.Range(0.7f, 1.0f);

        float facingDirection = transform.localScale.x < 0 ? 1f : -1f; // Correct for your setup!
        Vector2 waveDirection = new Vector2(facingDirection, 0f);

        for (int i = 0; i < waveCount; i++)
        {
            GameObject attack = Instantiate(wavePrefab, attackPoint.position, Quaternion.identity);
            BossAttack bossAttack = attack.GetComponent<BossAttack>();
            if (bossAttack != null)
            {
                bossAttack.isWave = true;
                bossAttack.waveAmplitude = Random.Range(0.8f, 2.0f);
                bossAttack.waveFrequency = Random.Range(1.5f, 3.0f);
                bossAttack.speed = 3f;
                bossAttack.SetDirection(waveDirection);
            }

            yield return new WaitForSeconds(delayBetweenWaves);
        }
    }

    private void MoveBoss()
    {
        float direction = Random.value < 0.5f ? -1f : 1f;

        transform.localScale = new Vector3(direction > 0 ? -1f : 1f, 1f, 1f);

        if (animator != null)
            animator.SetTrigger("Dash");

        Vector3 newPos = transform.position + new Vector3(hopDistance * direction, 0, 0);
        transform.position = Vector3.Lerp(transform.position, newPos, 0.5f);

        StartCoroutine(ResetFacingAfterDash());
    }

    private IEnumerator ResetFacingAfterDash()
    {
        yield return new WaitForSeconds(0.3f);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}