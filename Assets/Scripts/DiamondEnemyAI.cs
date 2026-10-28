using UnityEngine;

public class DiamondEliteAI : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float normalSpeed = 2f;
    public float dashSpeed = 18f;
    public float dashRange = 6f;

    [Header("Zamanlamalar")]
    public float prepTime = 0.6f;
    public float dashDuration = 0.25f;
    public float cooldownTime = 1.5f;

    [Header("Ateş Etme Ayarları")]
    public GameObject enemyBulletPrefab;
    public Transform frontFirePoint; 
    public Transform leftFirePoint;  
    public Transform rightFirePoint; 
    public float bulletForce = 10f;
    public float spreadAngle = 15f;
    public AudioClip shootSound;

    private Transform player;
    private Rigidbody2D rb;
    private enum State { Chasing, Prepping, Dashing, Cooldown }
    private State currentState = State.Chasing;
    private float stateTimer;
    private Vector2 dashDirection;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        if (currentState != State.Dashing)
        {
            Vector2 lookDir = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            rb.rotation = angle; 
        }

        switch (currentState)
        {
            case State.Chasing:
                rb.linearVelocity = (player.position - transform.position).normalized * normalSpeed;
                if (Vector2.Distance(transform.position, player.position) <= dashRange)
                {
                    currentState = State.Prepping;
                    stateTimer = prepTime;
                }
                break;

            case State.Prepping:
                rb.linearVelocity = Vector2.zero;
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0) StartDash();
                break;

            case State.Dashing:
                rb.linearVelocity = dashDirection * dashSpeed;
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0)
                {
                    currentState = State.Cooldown;
                    stateTimer = cooldownTime;
                }
                break;

            case State.Cooldown:
                rb.linearVelocity = Vector2.zero;
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0) currentState = State.Chasing;
                break;
        }
    }

    void StartDash()
    {
        currentState = State.Dashing;
        stateTimer = dashDuration;
        dashDirection = (player.position - transform.position).normalized;

        int bulletsPerPoint = 1 + Mathf.FloorToInt((SpawnManager.enemyPowerLevel - 1) / 2f);

        ShootFan(frontFirePoint, bulletsPerPoint);
        ShootFan(leftFirePoint, bulletsPerPoint);
        ShootFan(rightFirePoint, bulletsPerPoint);

        if (shootSound != null)
            SoundManager.instance.PlaySFX(shootSound, transform.position, 0.7f);
    }

    void ShootFan(Transform point, int count)
    {
        if (point == null || enemyBulletPrefab == null) return;

        for (int i = 0; i < count; i++)
        {
            float angleOffset = (i - (count - 1) / 2f) * spreadAngle;
            Quaternion bulletRotation = point.rotation * Quaternion.Euler(0, 0, angleOffset);

            GameObject bullet = Instantiate(enemyBulletPrefab, point.position, bulletRotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            
            if (bulletRb != null)
            {
                bulletRb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(1);
        }
    }
}