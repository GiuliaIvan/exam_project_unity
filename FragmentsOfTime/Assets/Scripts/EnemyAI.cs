using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Patrolling,
        Chasing
    }

    [Header("References")]
    [SerializeField] private Transform[] patrolPoints;  // Set patrol points in Unity
    [SerializeField] private Transform player;          // Assign player transform
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private int currentPointIndex = 0;

    [Header("Combat")]
    [SerializeField] private float chaseRange = 4f;
    [SerializeField] private float attackRange = 0.8f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int damageAmount = 1;
    private float lastAttackTime = -Mathf.Infinity;

    [Header("Patrol")]
    [SerializeField] private float waitTime = 1f;
    private float waitCounter = 0f;

    [Header("Health")]
    [SerializeField] private int maxLives = 3;
    private int currentLives;
    private bool isDead = false;

    private State state;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentLives = maxLives;
        state = State.Patrolling;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Switch between chasing and patrolling
        state = distanceToPlayer < chaseRange ? State.Chasing : State.Patrolling;

        if (state == State.Chasing)
        {
            ChaseAndAttack(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    private void FixedUpdate()
    {
        // Move enemy smoothly using physics
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    private void Patrol()
    {
        Vector2 target = patrolPoints[currentPointIndex].position;
        Vector2 dir = (target - (Vector2)transform.position).normalized;

        MoveTo(dir);

        if (Vector2.Distance(transform.position, target) < 0.2f)
        {
            MoveTo(Vector2.zero); // stop moving
            waitCounter += Time.deltaTime;

            if (waitCounter >= waitTime)
            {
                waitCounter = 0f;
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            }
        }
    }

    private void ChaseAndAttack(float distanceToPlayer)
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        MoveTo(directionToPlayer);

        // Attack if close and cooldown passed
        if (distanceToPlayer < attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            Hero hero = player.GetComponent<Hero>();
            if (hero != null)
            {
                hero.TakeDamage(damageAmount);
                Debug.Log("Enemy attacked the player!");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentLives -= damage;
        currentLives = Mathf.Max(0, currentLives); // Ensure doesn't go below 0
        Debug.Log("Skeleton took damage! Lives left: " + currentLives);

        // Simple red flash (one frame)
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.2f); // Resets after 0.1 seconds

        if (currentLives <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Skeleton died! 💀");
        Destroy(gameObject);
    }

    private void MoveTo(Vector2 dir)
    {
        moveDirection = dir;

        // Flip sprite based on direction
        if (dir.x != 0)
        {
            spriteRenderer.flipX = dir.x < 0;
        }
    }
}
