using UnityEngine;

public class HexagonEnemyAI : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float speed = 1.5f; 
    public float spinSpeed = 30f; 

    [Header("Ateş Etme Ayarları")]
    public GameObject enemyBulletPrefab;
    public float baseFireRate = 1.5f; 
    public float bulletForce = 8f; 
    private float nextFireTime;

    [Header("Mermi Çıkış Mesafesi")]
    public float spawnRadius = 0.8f; 

    [Header("Ses Efektleri")]
    public AudioClip shootSound; 

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        rb.rotation += spinSpeed * Time.fixedDeltaTime;

        if (Time.time >= nextFireTime && Time.timeScale > 0.1f)
        {
            ShootHexagon();
            
            float currentFireRate = baseFireRate / Mathf.Max(1f, SpawnManager.enemyPowerLevel * 0.1f);
            nextFireTime = Time.time + currentFireRate;
        }
    }

    void ShootHexagon()
    {
        if (enemyBulletPrefab == null) return;

        int baseBulletCount = 6;
        int bonusBullets = Mathf.FloorToInt(SpawnManager.enemyPowerLevel - 1f);
        int totalBulletCount = baseBulletCount + bonusBullets;

        float angleStep = 360f / totalBulletCount;

        for (int i = 0; i < totalBulletCount; i++)
        {
            float angle = rb.rotation + (i * angleStep);
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

            Vector3 spawnDir = bulletRotation * Vector3.up;
            Vector3 spawnPos = transform.position + (spawnDir * spawnRadius);

            GameObject bullet = Instantiate(enemyBulletPrefab, spawnPos, bulletRotation);
            
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
            }
        }

        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, transform.position, 0.6f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); 
            }
        }
    }
}