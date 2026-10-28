using UnityEngine;

public class DashEnemyAI : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float normalSpeed = 3f; 
    public float dashSpeed = 15f; 
    public float dashRange = 5f; 

    [Header("Zamanlamalar (Saniye)")]
    public float prepTime = 0.5f; 
    public float dashDuration = 0.2f; 
    public float cooldownTime = 2f; 

    private Transform player;
    private Rigidbody2D rb;


    private enum State { Chasing, Prepping, Dashing, Cooldown }
    private State currentState = State.Chasing;

    private float stateTimer;
    private Vector2 dashDirection;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        if (currentState != State.Dashing)
        {
            Vector2 lookDir = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }

        switch (currentState)
        {
            case State.Chasing:
            case State.Cooldown:
                Vector2 moveDir = (player.position - transform.position).normalized;
                rb.linearVelocity = moveDir * normalSpeed;

                if (currentState == State.Cooldown)
                {
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0) currentState = State.Chasing;
                }
                else if (Vector2.Distance(transform.position, player.position) <= dashRange)
                {
                    currentState = State.Prepping;
                    stateTimer = prepTime;
                }
                break;

            case State.Prepping:
                rb.linearVelocity = Vector2.zero; 
                stateTimer -= Time.deltaTime;

                if (stateTimer <= 0)
                {
                    currentState = State.Dashing;
                    stateTimer = dashDuration;
                    dashDirection = (player.position - transform.position).normalized;
                }
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