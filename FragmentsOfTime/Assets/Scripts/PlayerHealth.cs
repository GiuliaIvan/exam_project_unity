using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage! HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player DIED!");
            // Add death logic here
        }
    }
}
