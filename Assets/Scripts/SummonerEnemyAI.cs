using UnityEngine;

public class SummonerEnemyAI : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float baseSpeed = 2f;
    public float keepDistance = 6f;

    [Header("Çağırma (Summon) Ayarları")]
    public GameObject[] enemyPrefabs; 
    public float baseSummonCooldown = 4f; 
    public float spawnRadius = 1.5f;  
    private float summonTimer;

    [Header("Ses ve Efektler")]
    public GameObject summonEffectPrefab; 
    public AudioClip summonSound;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        
        rb = GetComponent<Rigidbody2D>();
        summonTimer = baseSummonCooldown; 
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        float currentSpeed = baseSpeed + (SpawnManager.enemyPowerLevel * 0.1f);

        if (distance < keepDistance - 1f)
        {
            rb.linearVelocity = -directionToPlayer * currentSpeed; 
        }
        else if (distance > keepDistance + 1f)
        {
            rb.linearVelocity = directionToPlayer * currentSpeed; 
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (Time.timeScale > 0.1f)
        {
            summonTimer -= Time.deltaTime;
            if (summonTimer <= 0)
            {
                SummonLogic();

                float cooldownReduction = (SpawnManager.enemyPowerLevel - 1) * 0.1f;
                float currentCooldown = Mathf.Max(1.5f, baseSummonCooldown - cooldownReduction);
                
                summonTimer = currentCooldown;
            }
        }
    }

    void SummonLogic()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        int summonCount = 1 + Mathf.FloorToInt((SpawnManager.enemyPowerLevel - 1) / 4f);
        summonCount = Mathf.Min(summonCount, 3); 

        for (int i = 0; i < summonCount; i++)
        {
            SummonRandomEnemy();
        }
    }

    void SummonRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyToSpawn = enemyPrefabs[randomIndex];

        Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = transform.position + (Vector3)randomOffset;

        if (summonEffectPrefab != null)
        {
            Instantiate(summonEffectPrefab, spawnPos, Quaternion.identity);
        }
        
        if (summonSound != null)
        {
            AudioSource.PlayClipAtPoint(summonSound, transform.position, 0.8f);
        }

        Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);
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