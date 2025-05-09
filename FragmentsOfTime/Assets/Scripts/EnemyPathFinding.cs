using UnityEngine;

// 🧭 This script moves the enemy using physics (Rigidbody2D)
public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;            // 🚀 How fast the enemy moves
    [SerializeField] private SpriteRenderer spriteRenderer;   // 🎨 This controls which way the sprite is facing

    private Rigidbody2D rb;        // 🧱 Unity's 2D physics body
    private Vector2 moveDirection; // 👉 Where the enemy wants to move

    // ✅ Public read-only access for current move direction (used in AI script)
    public Vector2 CurrentDirection => moveDirection;

    private void Awake()
    {
        // 🔌 Get the Rigidbody2D component from the enemy GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 🏃 Every physics update, move the enemy smoothly in the set direction
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    // 🎯 This function is called by the AI script to tell the enemy where to go
    public void MoveTo(Vector2 direction)
    {
        moveDirection = direction;

        // 🔄 Flip the sprite if the direction is left
        if (moveDirection.x != 0)
        {
            spriteRenderer.flipX = moveDirection.x < 0;
        }
    }
}

// 🧠 MoveTo() tells the enemy which direction to go.
// 💪 FixedUpdate() handles the actual movement using Unity physics.
// 🔄 The enemy's sprite flips based on the direction (left or right).
// 🧭 CurrentDirection lets other scripts (like EnemyAI) know which way the enemy is moving.