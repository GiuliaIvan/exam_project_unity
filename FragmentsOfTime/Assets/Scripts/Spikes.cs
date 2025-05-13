using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private string damagingStateName = "Spikes"; // Animation state name
    [SerializeField] private int damagingFrameStart = 3; // Start damaging at frame 3
    [SerializeField] private int damagingFrameEnd = 5;   // Stop after frame 5
    [SerializeField] private int totalFrames = 5;        // Total frames in animation
    private float lastDamageTime;
    [SerializeField] private float damageCooldown = 1f;

    private Animator animator;
    private bool canDamage;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName(damagingStateName)) return;

        // Convert normalized time (0 to 1) to current frame
        float normalizedTime = stateInfo.normalizedTime % 1f;
        int currentFrame = Mathf.FloorToInt(normalizedTime * totalFrames) + 1;

        canDamage = currentFrame >= damagingFrameStart && currentFrame <= damagingFrameEnd;

        // Optional: Change collider size based on frame
        // AdjustCollider(currentFrame);
    }

    // private void AdjustCollider(int frame)
    // {
    //     BoxCollider2D col = GetComponent<BoxCollider2D>();
    //     if (col == null) return;

    //     if (frame >= damagingFrameStart)
    //     {
    //         // Spike is up – collider full height
    //         col.offset = new Vector2(0, 0.25f);
    //         col.size = new Vector2(1f, 1f);
    //     }
    //     else
    //     {
    //         // Spike is down – reduce hitbox
    //         col.offset = new Vector2(0, -0.2f);
    //         col.size = new Vector2(1f, 0.3f);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only affect GameObjects tagged "Player"
        if (other.CompareTag("Player"))
        {
            Hero player = other.GetComponent<Hero>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Player hit by spikes!");
            }
        }
        
        if (!canDamage) return;

        Hero hero = other.GetComponent<Hero>();
        if (hero != null && !hero.isDead)
        {
            hero.TakeDamage(damage);
            Debug.Log("Player hurt by spike");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!canDamage) return;

        Hero hero = other.GetComponent<Hero>();
        if (hero != null && !hero.isDead && Time.time >= lastDamageTime + damageCooldown)
        {
            hero.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }

}
