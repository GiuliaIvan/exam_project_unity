using System.Collections;
using UnityEngine;

// 🎮 This script makes the enemy patrol between points and chase the player if they get close
public class EnemyAI : MonoBehaviour
{
    // 👮 The enemy can be in two states: Patrolling or Chasing
    private enum EnemyState { Patrolling, Chasing }
    private EnemyState currentState;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints; // Set patrol positions in Unity
    private int currentPointIndex = 0;

    [Header("Chase Settings")]
    [SerializeField] private Transform player;         // Reference to the player
    [SerializeField] private float chaseRange = 4f;    // How close the player needs to be to start chase

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1f; // Wait time between attacks
    [SerializeField] private int damageAmount = 1;
    private float lastAttackTime;

    [Header("Wait Time Between Patrols")]
    [SerializeField] private float waitTime = 1.5f;

    private EnemyPathFinding movement; // Handles enemy movement

    // 🧠 This runs before Start
    private void Awake()
    {
        movement = GetComponent<EnemyPathFinding>(); // Find the movement script
        currentState = EnemyState.Patrolling;        // Start in patrol mode
    }

    private void Start()
    {
        StartCoroutine(StateMachine()); // Begin switching between patrol/chase
    }

    // 🔁 Checks every frame if player is close enough to chase
    private IEnumerator StateMachine()
    {
        while (true)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            // Switch to chase if player is close
            currentState = distance < chaseRange ? EnemyState.Chasing : EnemyState.Patrolling;

            // Run patrol or chase depending on the state
            if (currentState == EnemyState.Patrolling)
                yield return StartCoroutine(Patrol());
            else
                yield return StartCoroutine(Chase());

            yield return null;
        }
    }

    // 🚶 Walks to the next patrol point
    private IEnumerator Patrol()
    {
        Vector2 targetPos = patrolPoints[currentPointIndex].position;
        Vector2 moveDirection = (targetPos - (Vector2)transform.position).normalized;
        movement.MoveTo(moveDirection);

        // Keep walking until close to the patrol point
        while (Vector2.Distance(transform.position, targetPos) > 0.2f && currentState == EnemyState.Patrolling)
        {
            yield return null;
        }

        // Stop and wait
        movement.MoveTo(Vector2.zero);
        yield return new WaitForSeconds(waitTime);

        // Move to next point
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    // 🏃‍♂️ Chases the player
    private IEnumerator Chase()
    {
        while (currentState == EnemyState.Chasing)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement.MoveTo(direction);

            // Try to attack if close enough
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance < 0.8f && Time.time > lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;

                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
            }

            yield return null;
        }
    }

    // 🧱 If we bump into a wall...
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && currentState == EnemyState.Patrolling)
        {
            Debug.Log("Enemy hit a wall. Switching direction.");
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }
}

// 💡 The enemy switches between walking around (Patrolling) and chasing the player (Chasing) based on how close the player is.
// 🧠 StateMachine() runs forever and decides what the enemy should be doing.
// 🧭 In Patrol, it walks to a set point, waits, then moves to the next one.
// 🚨 If the player gets too close, it switches to Chase and follows them.
// 🧱 If the enemy hits a wall while patrolling, it goes to the next patrol point.