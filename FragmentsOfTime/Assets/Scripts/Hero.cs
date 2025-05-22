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

    [Header("Light")]
    public GameObject playerLight;
    public float rotationSpeed = 5f;

    [Header("Hiding")]
    public bool isHiding = false;
    public float hideRange = 1f;

    [Header("Audio")]
    public AudioClip inventorySound;
    public AudioClip damageSound;
    public AudioClip deadSound;
    public AudioClip bowSound;


    void Start()
    {
        currentLives = maxLives;
        Debug.Log("Hero starting with lives: " + currentLives); // ✅ add this
        GameManager.Instance.StartGame(this);
    }

    void Update()
    {
        if (isDead) return;

        ProcessInputs();
        Animate();
        RotateLight();

        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            TryHide();
        }

        if (isHiding) return; // 🧊 Freeze all actions while hiding

    }

    void FixedUpdate()
    {
        if (isDead || isHiding) return;
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if ((moveX == 0 && moveY == 0) && (moveDirection.x != 0 || moveDirection.y != 0))
        {
            lastMoveDirection = moveDirection;
        }

        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.E))
            InventoryUIManager.Instance.ToggleInventory();

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                InventorySystem.Instance.SelectSlot(i);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            InventorySystem.Instance.DropSelectedItem(transform.position);

            if (inventorySound != null)
                AudioSource.PlayClipAtPoint(inventorySound, transform.position);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inventorySound != null)
                AudioSource.PlayClipAtPoint(inventorySound, transform.position);

            TryPickupItem();
        }

    }

    private void TryPickupItem()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D collider in colliders)
        {
            PickupItem pickup = collider.GetComponent<PickupItem>();
            if (pickup != null)
            {
                pickup.Pickup();

                break;
            }
        }

    }

    void Move()
    {
        if (isShooting)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentLives -= damage;
        currentLives = Mathf.Max(0, currentLives);

        LifeManager.Instance.UpdateLives(currentLives);

        if (damageSound != null)
            AudioSource.PlayClipAtPoint(damageSound, transform.position);

        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.2f);

        if (currentLives <= 0)
        {
            if (deadSound != null)
                AudioSource.PlayClipAtPoint(deadSound, transform.position);

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

        animator.SetTrigger("Die");

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        GameManager.Instance.PlayerDied();
    }

    void Shoot()
    {
        if (!canShoot || isShooting) return;

        isShooting = true;
        canShoot = false;
        animator.SetTrigger("Shoot");

        rb.linearVelocity = Vector2.zero;

        if (bowSound != null)
            AudioSource.PlayClipAtPoint(bowSound, transform.position);

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

        if (moveDirection != Vector2.zero)
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

            float pushDistance = 0.5f;
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            transform.position += (Vector3)(pushDirection * pushDistance);
        }
    }

    void RotateLight()
    {
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            playerLight.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            //playerLight.transform.rotation = Quaternion.Lerp(playerLight.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void ResetLives()
    {
        currentLives = maxLives;
        isDead = false;
        rb.simulated = true;
        LifeManager.Instance.UpdateLives(currentLives);
    }

    void TryHide()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (isHiding)
        {
            isHiding = false;
            sr.sortingOrder = 3; // Reset to normal
            playerLight.SetActive(true); // Turn the light back on
            Debug.Log("🧍 Player is no longer hiding.");
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hideRange);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Barrel"))
            {
                isHiding = true;
                moveDirection = Vector2.zero;
                rb.linearVelocity = Vector2.zero;

                sr.sortingOrder = 1; // Appear behind barrel
                playerLight.SetActive(false); // Turn the light off while hiding
                Debug.Log("🫣 Player is now hiding behind the barrel.");
                return;
            }
        }

        Debug.Log("❌ No barrel nearby to hide behind.");
    }
}
