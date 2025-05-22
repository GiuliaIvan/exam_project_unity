using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.Instance.AddLevelCoins(coinValue);
            
            // Play the sound
            if (audioSource != null && audioSource.clip != null)
            {
                // Detach the sound so it continues playing
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position, 2f);
            }

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}
