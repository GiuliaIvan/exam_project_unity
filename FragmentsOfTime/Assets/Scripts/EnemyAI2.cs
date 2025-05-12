using System.Collections;
using UnityEngine;

public class EnemyAI2 : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTime = 1.5f;

    private int currentPointIndex = 0;
    private EnemyPathFinding enemyPathFinding;
    // private bool isWaiting = false;
    
    private void Awake()
    {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
    }

    private void Start()
    {
        StartCoroutine(PatrolRoutine());
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
}
