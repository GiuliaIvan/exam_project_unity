using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageCooldown = 1.5f;

    private float lastDamageTime;
    private Animator animator;
    private Collider2D spikeCol;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spikeCol = GetComponent<Collider2D>();
    }

    public void SpikeON()
    {
        spikeCol.enabled = true;
    }

    public void SpikeOFF()
    {
        spikeCol.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Hero hero = other.GetComponent<Hero>();

            if (hero != null && !hero.isDead && Time.time >= lastDamageTime + damageCooldown)
            {
                hero.TakeDamage(damage);
                lastDamageTime = Time.time;

                // Optional pushback effect
                float pushDistance = 0.5f;
                Vector2 pushDirection = (hero.transform.position - transform.position).normalized;
                hero.transform.position += (Vector3)(pushDirection * pushDistance);

                Debug.Log("Player damaged by spikes!");
            }
        }
    }
}
