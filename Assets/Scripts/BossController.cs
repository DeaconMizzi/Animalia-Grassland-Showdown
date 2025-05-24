using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour, IDamageable
{
    [Header("Boss Health")]
    public int maxHealth = 10;
    [HideInInspector] public int currentHealth;

    [Header("Attack Parameters")]
    public float minAttackInterval = 1f;
    public float maxAttackInterval = 2f;
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
        while (isAlive)
        {
            yield return new WaitForSeconds(Random.Range(minAttackInterval, maxAttackInterval));

            if (chargeGlow != null)
                chargeGlow.SetActive(true);
            yield return new WaitForSeconds(chargeTime);
            if (chargeGlow != null)
                chargeGlow.SetActive(false);

            float actionChance = Random.value;

            if (actionChance < 0.7f)
            {
                PerformAttack();
            }
            else
            {
                MoveBoss();
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
                SwipeAttack();
                break;
            case 1:
                SpikesAttack();
                break;
            case 2:
                StartCoroutine(SpawnWaveAttack());
                break;
        }
    }

    private void SwipeAttack()
    {
        Debug.Log("Swipe Attack!");
        GameObject attack = Instantiate(swipePrefab, attackPoint.position, Quaternion.identity);
        BossAttack bossAttack = attack.GetComponent<BossAttack>();
        if (bossAttack != null) bossAttack.speed = 4f;
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
                float yOffset = +0.5f;
                Vector3 spikePos = new Vector3(checkPos.x, hit.point.y + yOffset, attackPoint.position.z);
                Instantiate(spikesPrefab, spikePos, Quaternion.identity);
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator SpawnWaveAttack()
    {
        Debug.Log("Wave Attack!");
        int waveCount = 4;
        float delayBetweenWaves = 0.2f;
        float waveSpacing = 0.5f;

        // Determine boss's facing direction
        float direction = transform.localScale.x < 0 ? -1f : 1f;

        for (int i = 0; i < waveCount; i++)
        {
            Vector3 waveSpawnPos = attackPoint.position + new Vector3(i * waveSpacing * direction, 0, 0);
            GameObject attack = Instantiate(wavePrefab, waveSpawnPos, Quaternion.identity);
            BossAttack bossAttack = attack.GetComponent<BossAttack>();
            if (bossAttack != null)
            {
                bossAttack.isWave = true;
                bossAttack.waveAmplitude = 1.5f;
                bossAttack.waveFrequency = 2f;
                bossAttack.speed = 3f;
                bossAttack.SetDirection((playerLastPosition - waveSpawnPos).normalized);
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