using UnityEngine;
using UnityEngine.EventSystems; 

public class PlayerController : MonoBehaviour
{
    [Header("Temel Ayarlar (Taban Değerler)")]
    public float baseMoveSpeed = 5f;
    public float baseFireRate = 0.5f;
    public float baseSlowTime = 0.05f;
    public float timeTransitionSpeed = 10f;

    [Header("Mobil Kontroller")]
    public Joystick movementJoystick; 

    [Header("Ateş Etme")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public float multiShotSpread = 10f;

    [Header("Ses Efektleri")]
    public AudioClip shootSound;

    private float currentMoveSpeed;
    private float currentFireRate;
    private float currentSlowTime;
    private float nextFireTime = 0f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 mousePosition;
    private Camera cam;

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
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        UpdateStatsFromUpgrades();
        Time.timeScale = currentSlowTime; 
    }

    void UpdateStatsFromUpgrades()
    {
        int speedLevel = PlayerPrefs.GetInt("Upgrade_MoveSpeed", 0);
        currentMoveSpeed = baseMoveSpeed + (speedLevel * 0.75f);

        int fireRateLevel = PlayerPrefs.GetInt("Upgrade_FireRate", 0);
        currentFireRate = baseFireRate - (fireRateLevel * 0.05f);
        currentFireRate = Mathf.Max(currentFireRate, 0.1f);

        int timeLevel = PlayerPrefs.GetInt("Upgrade_TimeSlow", 0);
        currentSlowTime = baseSlowTime / (1f + (timeLevel * 0.5f)); 
    }

    void Update()
    {
        if (movementJoystick != null && movementJoystick.Direction.sqrMagnitude > 0.01f)
        {
            moveInput = movementJoystick.Direction;
        }
        else
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
        }

        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        
        if (Time.timeScale == 0f) return;
        
        bool isShooting = Input.GetMouseButton(0); 

        bool isPointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

        if (isShooting && !isPointerOverUI && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + currentFireRate; 
        }

        if (moveInput.sqrMagnitude > 0.01f || (isShooting && !isPointerOverUI))
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.unscaledDeltaTime * timeTransitionSpeed);
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, currentSlowTime, Time.unscaledDeltaTime * timeTransitionSpeed);
        }
        
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * currentMoveSpeed;

        Vector2 lookDir = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        int multiShotLevel = PlayerPrefs.GetInt("Upgrade_MultiShot", 0);
        int bulletCount = 1 + multiShotLevel; 

        int sizeLevel = PlayerPrefs.GetInt("Upgrade_BulletSize", 0);
        float bulletScaleMultiplier = 1f + (sizeLevel * 0.2f); 

        for (int i = 0; i < bulletCount; i++)
        {
            float offset = (i - (bulletCount - 1) / 2f) * multiShotSpread;
            Quaternion spawnRotation = firePoint.rotation * Quaternion.Euler(0, 0, offset);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, spawnRotation);
            bullet.transform.localScale *= bulletScaleMultiplier;

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.AddForce(bullet.transform.right * fireForce, ForceMode2D.Impulse);
            }
        }

        if (shootSound != null) SoundManager.instance.PlaySFX(shootSound, firePoint.position, 0.7f);
    }
}