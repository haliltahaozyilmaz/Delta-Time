using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType { Health, Freeze }
    
    [Header("Eşya Tipi")]
    public ItemType itemType;

    [Header("Can Ayarları")]
    public int healAmount = 1;

    [Header("Donma Ayarları")]
    public float freezeDuration = 3f;
    public float slowMotionFactor = 0.2f; 

    private SpriteRenderer spriteRenderer;
    private Collider2D coll;
    private bool isCollected = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            ApplyEffect(other.gameObject);
        }
    }

    void ApplyEffect(GameObject player)
    {
        if (itemType == ItemType.Health)
        {
            PlayerHealth pHealth = player.GetComponent<PlayerHealth>();
            if (pHealth != null)
            {
                pHealth.Heal(healAmount); 
            }


            Destroy(gameObject); 
        }
        else if (itemType == ItemType.Freeze)
        {
            spriteRenderer.enabled = false;
            coll.enabled = false;

            StartCoroutine(FreezeTimeRoutine());
        }
    }

    IEnumerator FreezeTimeRoutine()
    {
        Time.timeScale = slowMotionFactor;
        
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 

        yield return new WaitForSecondsRealtime(freezeDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}