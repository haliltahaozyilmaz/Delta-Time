using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Süre Ayarları")]
    public float lifeTime = 3f; 
    public float wallStickTime = 0.5f;

    private Rigidbody2D rb;
    private Collider2D coll;
    private bool isStuck = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        
        Destroy(gameObject, lifeTime); 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStuck) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                isStuck = true;

                playerHealth.TakeDamage(1, gameObject); 

                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
                coll.enabled = false;

                transform.SetParent(collision.transform);

                return;
            }
        }

        isStuck = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        coll.enabled = false;
        
        Destroy(gameObject, wallStickTime);
    }
}