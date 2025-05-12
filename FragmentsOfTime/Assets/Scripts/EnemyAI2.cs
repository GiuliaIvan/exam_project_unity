using UnityEngine;
using System.Collections;

public class EnemyAI2 : MonoBehaviour
{
    private enum State
    {
        Patrolling,
        Chasing
    }

    private State state;

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float chaseRange = 4f;
    [SerializeField] private Transform player;

    private int currentPointIndex = 0;
    private EnemyPathFinding enemyPathFinding;
    private float waitCounter = 0f;

    private void Awake()
    {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Patrolling;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 👀 Instantly switch state based on distance
        state = distanceToPlayer < chaseRange ? State.Chasing : State.Patrolling;

        if (state == State.Chasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        Vector2 target = patrolPoints[currentPointIndex].position;
        Vector2 dir = (target - (Vector2)transform.position).normalized;

        enemyPathFinding.MoveTo(dir);

        if (Vector2.Distance(transform.position, target) < 0.2f)
        {
            enemyPathFinding.MoveTo(Vector2.zero); // stop
            waitCounter += Time.deltaTime;

            if (waitCounter >= waitTime)
            {
                waitCounter = 0f;
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            }
        }
    }

    private void ChasePlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        enemyPathFinding.MoveTo(directionToPlayer);
    }
}
