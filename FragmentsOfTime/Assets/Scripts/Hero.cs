using UnityEngine;

public class Hero : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Reset movement vector
        movement = Vector2.zero;

        // Get input
        if (Input.GetKey(KeyCode.W))
        {
            movement.y += 1;
            animator.SetInteger("WalkDirection", 2); // Up
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement.y -= 1;
            animator.SetInteger("WalkDirection", 0); // Down
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement.x -= 1;
            animator.SetInteger("WalkDirection", 1); // Left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement.x += 1;
            animator.SetInteger("WalkDirection", 3); // Right
        }

        // Normalize to avoid diagonal speed boost
        movement = movement.normalized;

        // Set isMoving in animator
        bool isMoving = movement != Vector2.zero;
        animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate() // runs at fixed intervals (by default every 0.02 seconds, or 50 times per second), regardless of your frame rate
    {
        // Apply movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
