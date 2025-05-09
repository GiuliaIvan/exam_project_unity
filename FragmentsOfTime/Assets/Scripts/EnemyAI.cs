using System.Collections;
using UnityEngine;

//------- This script controls the enemy’s behavior, like When & Where to move -------//

public class EnemyAI : MonoBehaviour
{
    // has one state: Roaming (which means walking around randomly
    private enum State
    {
        Roaming,
        Patrolling,
        Chasing
    }

    private State state;

    [SerializeField] private Transform[] patrolPoints; // Set these in Unity to define where the enemy patrols
    [SerializeField] private float waitTime = 1.5f;
    [SerializeField] private float chaseRange = 4f;
    [SerializeField] private Transform player; // Assign the player in Inspector

    private int currentPointIndex = 0;
    private EnemyPathFinding enemyPathFinding;
    //private bool isWaiting = false;

    // Create a "box" or area where the enemy is allowed to move
    [SerializeField] private Vector2 roamAreaMin = new Vector2(-5f, -5f);
    [SerializeField] private Vector2 roamAreaMax = new Vector2(5f, 5f);

    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int damageAmount = 1;
    private float lastAttackTime;



    // This happens when the scene loads
    private void Awake()
    {
        // It finds the EnemyPathFinding script attached to the enemy.
        // It sets the enemy's mode to Roaming.
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        // state = State.Roaming;
        state = State.Patrolling;
    }

    private void Start()
    {
        // When the game starts, it launches a Coroutine that runs the RoamingRoutine over time
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer < chaseRange)
            {
                state = State.Chasing;
            }
            else
            {
                state = State.Patrolling;
            }

            switch (state)
            {
                case State.Patrolling:
                    yield return StartCoroutine(PatrolRoutine());
                    break;

                case State.Chasing:
                    yield return StartCoroutine(ChaseRoutine());
                    break;
            }

            yield return null;
        }
    }



    private IEnumerator PatrolRoutine()
    {
        Vector2 target = patrolPoints[currentPointIndex].position;
        enemyPathFinding.MoveTo((target - (Vector2)transform.position).normalized);

        while (Vector2.Distance(transform.position, target) > 0.2f && state == State.Patrolling)
        {
            yield return null;
        }

        enemyPathFinding.MoveTo(Vector2.zero);
        yield return new WaitForSeconds(waitTime);

        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    // Picks a random direction within the allowed zone on the X and Y axes


    private IEnumerator ChaseRoutine()
    {
        while (state == State.Chasing)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            enemyPathFinding.MoveTo(directionToPlayer);

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

    private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Wall"))
    {
        Debug.Log("Hit wall!");

        // If patrolling, go to the next point
        if (state == State.Patrolling)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }

        // Flip direction if roaming
        if (state == State.Roaming)
        {
            enemyPathFinding.MoveTo(-enemyPathFinding.CurrentDirection);
        }
    }
}

}
