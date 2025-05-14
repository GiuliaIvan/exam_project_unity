using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;
    public float pickupRadius = 1f;
    public bool autoPickup = false; // Set to true if you want items to auto-pickup on touch
    
    private void Update()
    {
        // Manual pickup with F key
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryManualPickup();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Auto pickup on touch (if enabled)
        if (autoPickup && other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    private void TryManualPickup()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Pickup();
                break;
            }
        }
    }

    public void Pickup()
    {
        if (InventorySystem.Instance.AddItem(itemData))
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}