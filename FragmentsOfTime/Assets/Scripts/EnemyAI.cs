using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] Transform player;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float chaseRange = 4f;
    [SerializeField] float attackRange = 0.8f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] int damage = 1;
    [SerializeField] float waitTime = 1f;
    [SerializeField] int maxLives = 3;

    Rigidbody2D rb;
    Vector2 moveDirection;
    int currentPointIndex;
    float lastAttackTime = -Mathf.Infinity;
    float waitCounter;
    int currentLives;
    bool isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentLives = maxLives;
    }

    void Update()
    {
        if (isDead) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange)
        {
            ChaseAndAttack(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    void FixedUpdate() => rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

    void Patrol()
    {
        Vector2 target = patrolPoints[currentPointIndex].position;
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        MoveTo(dir);

        if (Vector2.Distance(transform.position, target) < 0.2f)
        {
            MoveTo(Vector2.zero);
            waitCounter += Time.deltaTime;

            if (waitCounter >= waitTime)
            {
                waitCounter = 0f;
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            }
        }
    }

    void ChaseAndAttack(float distanceToPlayer)
    {
        Vector2 dir = (player.position - transform.position).normalized;
        MoveTo(dir);

        if (distanceToPlayer < attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            player.GetComponent<Hero>()?.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentLives = Mathf.Max(0, currentLives - damage);
        FlashRed();

        if (currentLives <= 0) Die();
    }

    void FlashRed()
    {
        spriteRenderer.color = Color.red;
        Invoke(nameof(ResetColor), 0.2f);
    }

    void ResetColor() => spriteRenderer.color = Color.white;

    void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    void MoveTo(Vector2 dir)
    {
        moveDirection = dir;
        if (dir.x != 0) spriteRenderer.flipX = dir.x < 0;
    }
}