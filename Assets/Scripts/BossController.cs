using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene transitions

public class BossController : MonoBehaviour
{
    [Header("Boss Health")]
    public int maxHealth = 10; // Boss dies after 10 hits
    [HideInInspector] public int currentHealth;

    [Header("Attack Parameters")]
    public float minAttackInterval = 3f; // Minimum time between attacks
    public float maxAttackInterval = 5f; // Maximum time between attacks
    public Transform attackPoint; // The point from where attacks originate
    public GameObject swipePrefab; // Swipe attack prefab
    public GameObject spikesPrefab; // Spikes attack prefab
    public GameObject wavePrefab; // Wave attack prefab

    [Header("Boss State")]
    private bool isAlive = true;

    [Header("Animator")]
    private Animator animator; // Reference to Animator

    void Start()
    {
        animator = GetComponent<Animator>(); // Ensure the Animator is attached
        currentHealth = maxHealth; // Initialize boss health
        StartCoroutine(AttackCycle()); // Start attack pattern
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
        StopAllCoroutines(); // Stop attack cycles
        // Add death animation or effect here
        Destroy(gameObject, 1f); // Destroy the boss object after 1 second
        SceneManager.LoadScene("Win"); // Replace "Win" with the exact name of your Win scene

    }

    private IEnumerator AttackCycle()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(Random.Range(minAttackInterval, maxAttackInterval));
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        int attackType = Random.Range(0, 3); // Randomly choose an attack

        // Trigger the attack animation
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
        // Ensure the attack prefab has the BossAttack script
        BossAttack bossAttack = attack.GetComponent<BossAttack>();
        if (bossAttack != null)
        {
            bossAttack.speed = 8f; // Fast straight movement
        }
    }

    private void SpikesAttack()
    {
        Debug.Log("Spikes Attack!");
        StartCoroutine(SpawnSpikes());
    }

    private IEnumerator SpawnSpikes()
    {
        int spikeCount = 3; // Number of spikes to spawn
        float delayBetweenSpikes = 0.5f; // Delay between spikes

        GameObject previousSpike = null;

        for (int i = 0; i < spikeCount; i++)
        {
            // Spawn a spike
            GameObject spike = Instantiate(spikesPrefab, attackPoint.position, Quaternion.identity);

            // Destroy the previous spike
            if (previousSpike != null)
            {
                Destroy(previousSpike);
            }

            previousSpike = spike;

            yield return new WaitForSeconds(delayBetweenSpikes);
        }

        // Destroy the final spike
        if (previousSpike != null)
        {
            Destroy(previousSpike, 1.5f);
        }
    }

    private void WaveAttack()
    {
        Debug.Log("Wave Attack!");
        GameObject attack = Instantiate(wavePrefab, attackPoint.position, Quaternion.identity);
        // Ensure the attack prefab has the BossAttack script
        BossAttack bossAttack = attack.GetComponent<BossAttack>();
        if (bossAttack != null)
        {
            bossAttack.speed = 4f;        // Slow wave movement
            bossAttack.isWave = true;    // Enable wave motion
            bossAttack.waveAmplitude = 1.5f; // Up and down height
            bossAttack.waveFrequency = 2f;  // Wave speed
        }
    }
}
