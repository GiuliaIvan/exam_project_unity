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
    public int maxLives = 3;
    private int currentLives;
    private bool isDead = false;

    [Header("Combat")]
    public float shootCooldown = 0.5f;
    private bool canShoot = true;

    // [Header("Game Over")]
    // public GameObject gameOverScreen; // Assign in inspector

   void Start()
    {
        currentLives = maxLives;
    }

    void Update()
    {
        if (isDead) return;

        ProcessInputs();
        Animate();

        // Test death trigger (remove this in final game)
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            TakeDamage(1);
        }

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

        if((moveX == 0 && moveY == 0 ) && moveDirection.x != 0 || moveDirection.y !=0)
        {
            lastMoveDirection = moveDirection;
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
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
            canShoot = false;
            animator.SetTrigger("Shoot");
            Invoke(nameof(ResetShoot), shootCooldown);
        }

        void ResetShoot()
        {
            canShoot = true;
        }

    void Animate()
    {
        animator.SetFloat("AnimMoveX", moveDirection.x);
        animator.SetFloat("AnimMoveY", moveDirection.y);
        animator.SetFloat("AnimMoveMagnitude", moveDirection.magnitude);
        animator.SetFloat("AnimLastMoveX", lastMoveDirection.x);
        animator.SetFloat("AnimLastMoveY", lastMoveDirection.y);
    }

}
