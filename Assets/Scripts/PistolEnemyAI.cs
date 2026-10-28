using UnityEngine;

public class PistolEnemyAI : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float speed = 2.5f;
    public float stopDistance = 6f;

    [Header("Ateş Etme Ayarları")]
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f; 
    public float bulletForce = 12f;
    public float spreadAngle = 10f; 
    private float nextFireTime;

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
        float distance = Vector2.Distance(transform.position, player.position);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        if (distance > stopDistance)
        {
            rb.linearVelocity = direction * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            if (Time.time >= nextFireTime && Time.timeScale > 0.1f)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        if (enemyBulletPrefab == null || firePoint == null) return;

        int bulletCount = 1 + Mathf.FloorToInt((SpawnManager.enemyPowerLevel - 1) / 2.5f);

        for (int i = 0; i < bulletCount; i++)
        {
            float angleOffset = (i - (bulletCount - 1) / 2f) * spreadAngle;
            Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0, 0, angleOffset);

            GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, bulletRotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            
            if (bulletRb != null)
            {
                bulletRb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
            }
        }

        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, firePoint.position, 0.4f);
        }
    }
}