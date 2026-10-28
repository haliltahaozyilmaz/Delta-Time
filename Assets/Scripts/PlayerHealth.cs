using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int baseMaxHealth = 1;
    private int currentMaxHealth;
    private int currentHealth;

    [Header("Hasar Ayarları")]
    public float baseInvincibilityTime = 1f;
    private float currentInvincibilityTime;
    private bool isInvincible = false;

    [Header("Efektler")]
    public GameObject hitEffectPrefab; 
    public GameObject deathEffectPrefab; 

    [Header("Ses Efektleri")]
    public AudioClip hitSound; 
    public AudioClip deathSound; 
    public AudioClip dodgeSound;

    private List<GameObject> stuckBullets = new List<GameObject>();
    private bool isDead = false;

    void Start()
    {
        UpdateStatsFromUpgrades();
        
        currentHealth = currentMaxHealth;
    }

    void UpdateStatsFromUpgrades()
    {
        if (UpgradeManager.instance != null)
        {
            int healthLevel = UpgradeManager.instance.GetUpgradeLevel("MaxHealth");
            currentMaxHealth = baseMaxHealth + healthLevel;

            int iFrameLevel = UpgradeManager.instance.GetUpgradeLevel("IFrameDuration");
            currentInvincibilityTime = baseInvincibilityTime + (iFrameLevel * 0.2f);
        }
        else
        {
            currentMaxHealth = baseMaxHealth;
            currentInvincibilityTime = baseInvincibilityTime;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        ProcessDamage(damageAmount, null);
    }

    public void TakeDamage(int damageAmount, GameObject bulletObj)
    {
        ProcessDamage(damageAmount, bulletObj);
    }

    private void ProcessDamage(int damageAmount, GameObject bulletObj)
    {
        if (isDead || isInvincible) return;

        if (UpgradeManager.instance != null)
        {
            int dodgeLevel = UpgradeManager.instance.GetUpgradeLevel("DodgeChance");
            float dodgeProbability = dodgeLevel * 5f; 

            if (Random.Range(0f, 100f) < dodgeProbability)
            {
                Debug.Log("Sıyrıldın! (Dodge)");
                if (dodgeSound != null) SoundManager.instance.PlaySFX(dodgeSound, transform.position, 0.7f);
                return; 
            }
        }

        currentHealth -= damageAmount;
        Debug.Log("Oyuncu Hasar Aldı! Kalan Can: " + currentHealth);

        Vector3 effectPos = (bulletObj != null) ? bulletObj.transform.position : transform.position;
        
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, effectPos, Quaternion.identity);
        }

        if (hitSound != null)
        {
            SoundManager.instance.PlaySFX(hitSound, transform.position, 0.7f);
        }

        if (bulletObj != null)
        {
            if (!stuckBullets.Contains(bulletObj))
            {
                stuckBullets.Add(bulletObj);
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = new Color(1, 1, 1, 0.1f);
        
        yield return new WaitForSeconds(currentInvincibilityTime);
        
        if (sr != null) sr.color = new Color(1, 1, 1, 1f); 
        isInvincible = false;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        if (deathSound != null)
        {
            SoundManager.instance.PlaySFX(deathSound, transform.position, 0.7f);
        }

        foreach (GameObject bullet in stuckBullets)
        {
            if (bullet != null) Destroy(bullet);
        }
        stuckBullets.Clear();

        GetComponent<SpriteRenderer>().enabled = false;

        Time.timeScale = 0f;

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.FinalizeRun();
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.ShowGameOverPanel();
        }

        Debug.Log("OYUN BİTTİ!");
    }
    public void Heal(int amount)
{
    currentHealth += amount;
    if (currentHealth > currentMaxHealth) currentHealth = currentMaxHealth; 
    
}
}