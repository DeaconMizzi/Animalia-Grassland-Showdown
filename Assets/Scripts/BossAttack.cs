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

        if (!isStationary)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            direction = (player != null) ? (player.transform.position - transform.position).normalized : Vector2.right;
            startPosition = transform.position;
        }
    }
    void Update()
    {
        if (isStationary) return;

        // Wave movement
        if (isWave)
        {
            float waveY = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
            transform.position = new Vector2(transform.position.x + direction.x * speed * Time.deltaTime, startPosition.y + waveY);
        }
        else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        // Raycast forward to check for bounce
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.1f, LayerMask.GetMask("Ground", "Walls"));
        if (hit.collider != null)
        {
            // Reflect the direction on X axis (bounce)
            direction = Vector2.Reflect(direction, hit.normal);
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
    }
}