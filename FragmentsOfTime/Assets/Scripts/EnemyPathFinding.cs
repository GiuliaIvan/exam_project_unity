using UnityEngine;

//------- This script actually moves the enemy using physics -------//

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;  //Controls how fast the enemy moves
    [SerializeField] private SpriteRenderer spriteRenderer; // Assign in Inspector

    private Rigidbody2D rb;  // rb = Unity's 2D physics component
    private Vector2 moveDir;   //Direction the enemy wants to go
    public Vector2 CurrentDirection => moveDir;

    private void Awake()
    {
        // Connects to the enemy’s Rigidbody so Unity can move it with physics
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Moves the enemy every physics update by:
        // Taking its current position,
        // Adding the movement direction multiplied by speed and time
        // So it moves smoothly in the direction set
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
        //  This is called by the first script to tell the enemy where to move
        moveDir = targetPosition;

        // Face direction
        if (moveDir.x != 0)
        {
            spriteRenderer.flipX = moveDir.x < 0;
        }
    }
}
