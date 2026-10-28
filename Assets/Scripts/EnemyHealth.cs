using UnityEngine;
using System.Collections.Generic; 

public class EnemyHealth : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int maxHealth = 2;
    private int currentHealth;

    [Header("Efekt Ayarları")]
    public GameObject hitEffectPrefab; 
    public GameObject deathEffectPrefab; 

    [Header("Ses Efektleri")]
    public AudioClip hitSound; 
    public AudioClip deathSound;

    [Header("Puan Değeri")]
    public int scoreValue = 10;

    [Header("Market & Eşya Düşürme")]
    public GameObject[] dropableItems;
    [Range(0, 100)] public float baseDropChance = 5f; 

    private List<GameObject> stuckBullets = new List<GameObject>();
    private bool isQuitting = false; 

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount, GameObject bullet)
    {
        currentHealth -= damageAmount;

        if (hitEffectPrefab != null && bullet != null)
        {
            Instantiate(hitEffectPrefab, bullet.transform.position, Quaternion.identity);
        }

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position, 0.7f);
        }

        if (bullet != null && !stuckBullets.Contains(bullet))
        {
            stuckBullets.Add(bullet);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f);
        }

        foreach (GameObject bullet in stuckBullets)
        {
            if (bullet != null) Destroy(bullet);
        }
        stuckBullets.Clear(); 
    
        if (ScoreManager.instance != null)
        {
            int multiplierLevel = 0;
            if (UpgradeManager.instance != null)
            {
                multiplierLevel = UpgradeManager.instance.GetUpgradeLevel("MoneyMultiplier");
            }

            int finalScore = Mathf.RoundToInt(scoreValue * (1f + (multiplierLevel * 0.2f)));
            ScoreManager.instance.AddScore(finalScore);
        }

        TryDropItem();

        Destroy(gameObject);
    }

    void TryDropItem()
    {
        if (dropableItems == null || dropableItems.Length == 0) return;

        float luckBonus = 0;
        if (UpgradeManager.instance != null)
        {
            luckBonus = UpgradeManager.instance.GetUpgradeLevel("Luck") * 2f;
        }

        float finalDropChance = baseDropChance + luckBonus;
        float randomRoll = Random.Range(0f, 100f);

        if (randomRoll < finalDropChance)
        {
            int randomIndex = Random.Range(0, dropableItems.Length);
            Instantiate(dropableItems[randomIndex], transform.position, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        if (isQuitting) return;
        foreach (GameObject bullet in stuckBullets)
        {
            if (bullet != null) Destroy(bullet);
        }
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }
}