using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Süre Ayarları")]
    public float lifeTime = 3f; 
    public float wallStickTime = 0.5f; 

    private Rigidbody2D rb;
    private Collider2D coll;
    private bool isStuck = false;

    private int damage = 1; 
    private int bounceCount;
    private Vector2 lastVelocity; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        Destroy(gameObject, lifeTime); 
    }

    void OnEnable()
    {
        UpgradeManager.OnUpgradesChanged += UpdateStatsFromUpgrades;
    }

    void OnDisable()
    {
        UpgradeManager.OnUpgradesChanged -= UpdateStatsFromUpgrades;
    }

    void Start()
    {
        UpdateStatsFromUpgrades();
    }

    void UpdateStatsFromUpgrades()
    {
        bounceCount = PlayerPrefs.GetInt("Upgrade_Bounce", 0);
    }

    void Update()
    {
        if (!isStuck)
        {
            lastVelocity = rb.linearVelocity;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStuck) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                isStuck = true;
                enemyHealth.TakeDamage(damage, gameObject); 
                StickToTarget(collision.transform);
                return;
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            if (bounceCount > 0)
            {
                bounceCount--;
                
                var speed = lastVelocity.magnitude;
                var direction = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

                rb.linearVelocity = direction * Mathf.Max(speed, 10f); 
                
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                isStuck = true;
                StickToTarget(null);
                Destroy(gameObject, wallStickTime);
            }
        }
    }

    void StickToTarget(Transform parent)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        coll.enabled = false;

        if (parent != null)
        {
            transform.SetParent(parent);
        }
    }
}