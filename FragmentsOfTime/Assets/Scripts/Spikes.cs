using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    private float lastDamageTime;
    [SerializeField] private float damageCooldown = 1.5f;

    private Animator animator;
    private bool canDamage;

    Collider2D spikeCol;

    void Start()
    {
        spikeCol = this.GetComponent<Collider2D>();
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SpikeON()
    {
        spikeCol.enabled = true;
    }

    public void SpikeOFF()
    {
        spikeCol.enabled = false;
    }

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
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Hero hero = other.GetComponent<Hero>();
        if (hero != null && !hero.isDead && Time.time >= lastDamageTime + damageCooldown)
        {
            hero.TakeDamage(damage);
            lastDamageTime = Time.time;
        }

        if (!canDamage) return;
    }

}
