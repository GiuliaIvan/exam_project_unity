using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData;
    public float pickupRadius = 1f;
    public GameObject tagLetter;
    private bool playerInTrigger = false;

    void Start()
    {
        if (tagLetter != null)
        {
            tagLetter.SetActive(false);
        }
    }

    private void Update()
    {
        // Manual pickup with F key
        if (playerInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            TryManualPickup();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (tagLetter != null)
            {
                tagLetter.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            if (tagLetter != null)
            {
                tagLetter.SetActive(false);
            }
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
