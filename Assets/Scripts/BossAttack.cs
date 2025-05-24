using System.Collections;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float lifetime = 3f;
    public int damage = 1;
    public float speed = 5f;
    public bool isWave = false;
    public float waveAmplitude = 1f;
    public float waveFrequency = 2f;
    public bool isStationary = false;

    private Vector2 direction;
    private Vector2 startPosition;

    void Start()
    {
        Destroy(gameObject, lifetime);

        // ❌ REMOVE THIS - we don't need to reset direction
        // if (!isStationary)
        // {
        //     GameObject player = GameObject.FindGameObjectWithTag("Player");
        //     direction = (player != null) ? (player.transform.position - transform.position).normalized : Vector2.right;
        //     startPosition = transform.position;
        // }

        startPosition = transform.position; // Keep this to allow wave bouncing
    }

    void Update()
    {
        if (isStationary) return;

        if (isWave)
        {
            // Move in wave pattern + optional spin
            float waveY = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
            transform.position = new Vector2(transform.position.x + direction.x * speed * Time.deltaTime, startPosition.y + waveY);

            // Spin effect for wave
            transform.Rotate(0f, 0f, 180f * Time.deltaTime); 
        }
        else
        {
            // Move straight without spin
            transform.Translate(direction * speed * Time.deltaTime);
        }

        // Ground bounce (optional)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground", "Walls"));
        if (hit.collider != null)
        {
            waveAmplitude = -waveAmplitude;
        }
}

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        startPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player hit by attack! Damage: " + damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Destroy(gameObject);
        }
    }
}