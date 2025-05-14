using UnityEngine;
using System.Collections;

public class ItemPickupHandler : MonoBehaviour
{
    private WorldItem nearbyItem;

    private void Update()
    {
        if (nearbyItem != null && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(PickupSequence(nearbyItem));
        }
    }

    private IEnumerator PickupSequence(WorldItem item)
    {
        InteractionUI.Instance.StartLoading(); // Show circle
        yield return new WaitForSeconds(1.5f);

        if (InventorySystem.Instance.AddItem(item.itemData))
        {
            Destroy(item.gameObject); // Remove world item
            InteractionUI.Instance.HideIcon();
            nearbyItem = null;
        }
        else
        {
            InteractionUI.Instance.ShowFullInventoryMessage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            nearbyItem = other.GetComponent<WorldItem>();
            if (nearbyItem != null)
            {
                InteractionUI.Instance.ShowIcon(); // Show F icon
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            if (nearbyItem != null && other.gameObject == nearbyItem.gameObject)
            {
                InteractionUI.Instance.HideIcon();
                nearbyItem = null;
            }
        }
    }
}
