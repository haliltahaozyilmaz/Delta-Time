using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyTier
    {
        public string tierName;
        public GameObject[] prefabs;
        public float unlockTime;
    }

    [Header("Düşman Grupları")]
    public EnemyTier[] tiers;

    [Header("Zorluk Ayarları")]
    public float spawnRadius = 12f;
    public float initialSpawnRate = 2.5f;
    public float minSpawnRate = 0.4f;
    public float difficultyScaling = 0.02f;

    [Header("Global Güçlenme (Infinite Scaling)")]
    public static float enemyPowerLevel = 1f;
    public float powerIncreaseRate = 0.05f;

    [Header("Harita Sınırları (50x50)")]
    public float minX = -24.5f; 
    public float maxX = 24.5f; 
    public float minY = -24.5f; 
    public float maxY = 24.5f; 

    private float timer = 0f;
    private float nextSpawnTime;
    private Transform player;
    private List<GameObject> activePool = new List<GameObject>();
    private int currentTierIndex = -1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        nextSpawnTime = Time.time + initialSpawnRate;
        enemyPowerLevel = 1f;
    }

    void Update()
    {
        if (player == null || Time.timeScale < 0.1f) return;

        timer += Time.deltaTime;
        enemyPowerLevel += powerIncreaseRate * Time.deltaTime;

        CheckTiers();

        float currentRate = Mathf.Max(minSpawnRate, initialSpawnRate - (timer * difficultyScaling));
        
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + currentRate;
        }
    }

    void CheckTiers()
    {
        for (int i = 0; i < tiers.Length; i++)
        {
            if (timer >= tiers[i].unlockTime && currentTierIndex < i)
            {
                currentTierIndex = i;
                foreach (GameObject p in tiers[i].prefabs)
                {
                    activePool.Add(p);
                }
                Debug.Log("YENİ DÜŞMANLAR KATILDI: " + tiers[i].tierName);
            }
        }
    }

    void SpawnEnemy()
    {
        if (activePool.Count == 0) return;

        GameObject prefab = activePool[Random.Range(0, activePool.Count)];
        
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnRadius;

        spawnPos.x = Mathf.Clamp(spawnPos.x, minX, maxX);
        spawnPos.y = Mathf.Clamp(spawnPos.y, minY, maxY);

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0);
        Gizmos.DrawWireCube(center, size);
    }
}