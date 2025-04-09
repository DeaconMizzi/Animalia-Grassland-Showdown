using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene transitions

public class BossController : MonoBehaviour
{
    [Header("Boss Health")]
    public int maxHealth = 10;
    [HideInInspector] public int currentHealth;

    [Header("Attack Parameters")]
    public float minAttackInterval = 3f;
    public float maxAttackInterval = 5f;
    public float chargeTime = 0.8f; // Delay before attacks
    public Transform attackPoint;
    public GameObject swipePrefab;
    public GameObject spikesPrefab;
    public GameObject wavePrefab;
    public GameObject chargeGlow; // The visual glow object

    [Header("Boss State")]
    private bool isAlive = true;

    [Header("Animator")]
    private Animator animator;

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

        if (currentHealth <= 0)
        {
            Die();
        }
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

            // 🌟 Charge-up glow before attack
            if (chargeGlow != null)
                chargeGlow.SetActive(true);

            yield return new WaitForSeconds(chargeTime);

            if (chargeGlow != null)
                chargeGlow.SetActive(false);

            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        int attackType = Random.Range(0, 3);

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        switch (attackType)
        {
            case 0:
                SwipeAttack();
                break;
            case 1:
                SpikesAttack();
                break;
            case 2:
                WaveAttack();
                break;
        }
    }

    private void SwipeAttack()
    {
        Debug.Log("Swipe Attack!");
        GameObject attack = Instantiate(swipePrefab, attackPoint.position, Quaternion.identity);
        BossAttack bossAttack = attack.GetComponent<BossAttack>();
        if (bossAttack != null)
        {
            bossAttack.speed = 4f;
        }
    }

    private void SpikesAttack()
    {
        Debug.Log("Spikes Attack!");
        StartCoroutine(SpawnSpikes());
    }

    private IEnumerator SpawnSpikes()
    {
        int spikeCount = 3;
        float delayBetweenSpikes = 0.5f;

        GameObject previousSpike = null;

        for (int i = 0; i < spikeCount; i++)
        {
            Vector3 groundPosition = new Vector3(attackPoint.position.x, -4.6f, attackPoint.position.z);
            GameObject spike = Instantiate(spikesPrefab, groundPosition, Quaternion.identity);

            if (previousSpike != null)
                Destroy(previousSpike);

            previousSpike = spike;

            yield return new WaitForSeconds(delayBetweenSpikes);
        }

        if (previousSpike != null)
            Destroy(previousSpike, 1.5f);
    }

    private void WaveAttack()
    {
        Debug.Log("Wave Attack!");
        GameObject attack = Instantiate(wavePrefab, attackPoint.position, Quaternion.identity);
        BossAttack bossAttack = attack.GetComponent<BossAttack>();
        if (bossAttack != null)
        {
            bossAttack.speed = 2.5f;
            bossAttack.isWave = true;
            bossAttack.waveAmplitude = 1.5f;
            bossAttack.waveFrequency = 2f;
        }
    }
}
