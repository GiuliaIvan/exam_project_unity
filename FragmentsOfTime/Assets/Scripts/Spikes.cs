using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damage = 1;

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
    }
}
