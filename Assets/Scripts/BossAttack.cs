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

    public static bool freezeAllAttacks = false; // Add this for cutscene freeze

    void Start()
    {
        Destroy(gameObject, lifetime);
        startPosition = transform.position;
    }

    void Update()
    {
        if (isStationary || freezeAllAttacks) return; // Add freeze check here

        if (isWave)
        {
            float waveY = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
            transform.position = new Vector2(transform.position.x + direction.x * speed * Time.deltaTime, startPosition.y + waveY);
            transform.Rotate(0f, 0f, 180f * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

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
