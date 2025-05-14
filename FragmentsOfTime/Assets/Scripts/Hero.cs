using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hero : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;
    public Animator animator;

    [Header("Health")]
    public int maxLives = 5;
    private int currentLives;
    public bool isDead = false;

    [Header("Combat")]
    public float shootCooldown = 0.5f;
    private bool canShoot = true;
    private bool isShooting = false;
    public GameObject arrowPrefab;

    // [Header("Game Over")]
    // public GameObject gameOverScreen; // Assign in inspector

    void Start()
    {
        // Debug.Log("Player start");
        // Debug.Log("isDead: " + isDead);
        // Debug.Log("rb.simulated: " + rb.simulated);
        currentLives = maxLives;
        isDead = false; // force reset
        rb.simulated = true;

    }

    void Update()
    {
        if (isDead) return;

        ProcessInputs();
        Animate();

        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            Shoot();
        }
    }

    void FixedUpdate() // runs at fixed intervals (by default every 0.02 seconds, or 50 times per second), regardless of your frame rate
    {
        if (isDead) return;
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if ((moveX == 0 && moveY == 0) && moveDirection.x != 0 || moveDirection.y != 0)
        {
            lastMoveDirection = moveDirection;
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        if (isShooting)
        {
            rb.linearVelocity = Vector2.zero; // Force stop if shooting
            return;
        }

        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentLives -= damage;
        currentLives = Mathf.Max(0, currentLives); // Ensure doesn't go below 0

        // Simple red flash (one frame)
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.2f); // Resets after 0.1 seconds

        if (currentLives <= 0)
        {
            Die();
        }
    }

    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void Die()
    {
        isDead = true;

        //Trigger death animation
        animator.SetTrigger("Die");

        //Stop movement
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false; // Disables physics collisions

        // Game over handling (call your game manager)
        // GameManager.Instance.PlayerDied();
    }

    void Shoot()
    {
        if (!canShoot || isShooting) return;

        isShooting = true;
        canShoot = false;
        animator.SetTrigger("Shoot");

        // Immediately stop movement
        rb.linearVelocity = Vector2.zero;

        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetDirection(lastMoveDirection);
    }

    public void ResetShoot()
    {
        isShooting = false;
        canShoot = true;
    }

    void Animate()
    {
        animator.SetFloat("AnimMoveX", moveDirection.x);
        animator.SetFloat("AnimMoveY", moveDirection.y);
        animator.SetFloat("AnimMoveMagnitude", moveDirection.magnitude);

        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            lastMoveDirection = moveDirection;
            animator.SetFloat("AnimLastMoveX", lastMoveDirection.x);
            animator.SetFloat("AnimLastMoveY", lastMoveDirection.y);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Trap"))
        {
            TakeDamage(1);

            //pushback (no physics force)
            float pushDistance = 0.5f;
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            transform.position += (Vector3)(pushDirection * pushDistance);
        }
    }

}
