using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] Transform player;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float attackRange = 0.8f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] int damage = 1;
    [SerializeField] float waitTime = 1f;
    [SerializeField] int maxLives = 3;

    [SerializeField] LayerMask obstacleMask;

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
        Debug.Log("Enemy Update is running");

        if (player == null)
        {
            Debug.LogWarning("🚨 Player reference missing!");
            return;
        }

        if (player.GetComponent<Hero>().isDead)
        {
            Debug.Log("💤 Player is dead, returning to patrol");
            Patrol();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (CanSeePlayer())
        {
            Debug.Log("✅ Can see player, chase & attack");
            ChaseAndAttack(distanceToPlayer);
        }
        else
        {
            Debug.Log("❌ Cannot see player, patrol instead");
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
        // If extremely close to the player, stop moving to avoid jitter
        if (distanceToPlayer < 0.1f)
        {
            MoveTo(Vector2.zero);
        }
        else
        {
            Vector2 dir = (player.position - transform.position).normalized;
            MoveTo(dir);
        }

        // Attack if in range and cooldown passed
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
        // if (dir.x != 0) spriteRenderer.flipX = dir.x < 0;
        if (Mathf.Abs(dir.x) > 0.05f)  // Avoid flipping rapidly when nearly still
        {
            spriteRenderer.flipX = dir.x < 0;
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 origin = transform.position;
        Vector2 target = player.position;
        Vector2 direction = target - origin;
        float distance = direction.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction.normalized, distance, ~obstacleMask);

        if (hit.collider != null)
        {
            Debug.DrawLine(origin, hit.point, Color.red);
            if (hit.collider.transform == player)
            {
                Debug.Log("👀 Enemy sees player");
                return true;
            }
            else
            {
                Debug.Log("🚧 Enemy vision blocked by: " + hit.collider.name);
            }
        }
        else
        {
            Debug.Log("❓ Nothing hit by raycast");
        }

        return false;
    }

    void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, player.position);
    }
}