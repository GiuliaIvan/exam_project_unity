using System.Collections;
using UnityEngine;

//------- This script controls the enemy’s behavior, like When & Where to move -------//

public class EnemyAI : MonoBehaviour
{
    // has one state: Roaming (which means walking around randomly
    private enum State
    {
        Roaming
    }

    private State state;

    [SerializeField] private Transform[] patrolPoints; // Set these in Unity to define where the enemy patrols
    [SerializeField] private float waitTime = 1.5f;

    private int currentPointIndex = 0;
    private EnemyPathFinding enemyPathFinding;
    private bool isWaiting = false;

    // Create a "box" or area where the enemy is allowed to move
    [SerializeField] private Vector2 roamAreaMin = new Vector2(-5f, -5f);
    [SerializeField] private Vector2 roamAreaMax = new Vector2(5f, 5f);


    // This happens when the scene loads
    private void Awake()
    {
        // It finds the EnemyPathFinding script attached to the enemy.
        // It sets the enemy's mode to Roaming.
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        // state = State.Roaming;
    }

    private void Start()
    {
        // When the game starts, it launches a Coroutine that runs the RoamingRoutine over time
        StartCoroutine(PatrolRoutine());
    }

    private IEnumerator RoamingRoutine()
    {
        // While the enemy is in Roaming mode
        while (state == State.Roaming)
        {
            // Pick a random direction to move
            // Tell the enemy to move that way
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathFinding.MoveTo(roamPosition);
            // Wait for 2 seconds & Repeat
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            Vector2 target = patrolPoints[currentPointIndex].position;
            enemyPathFinding.MoveTo((target - (Vector2)transform.position).normalized);

            // Wait until enemy is close to the point
            while (Vector2.Distance(transform.position, target) > 0.2f)
            {
                yield return null;
            }

            // Stop movement and wait
            enemyPathFinding.MoveTo(Vector2.zero);
            yield return new WaitForSeconds(waitTime);

            // Go to the next point
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    // Picks a random direction within the allowed zone on the X and Y axes
    private Vector2 GetRoamingPosition()
    {
        // return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        float x = Random.Range(roamAreaMin.x, roamAreaMax.x);
        float y = Random.Range(roamAreaMin.y, roamAreaMax.y);
        Vector2 targetPosition = new Vector2(x, y);

        // Direction from enemy to target
        return (targetPosition - (Vector2)transform.position).normalized;
        // .normalized makes sure the direction isn’t too big — it just gives a consistent direction to move
    }
}
