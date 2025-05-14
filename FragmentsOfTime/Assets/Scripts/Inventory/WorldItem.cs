using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemData itemData;

    public void Initialize(ItemData data)
    {
        itemData = data;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Dropped_" + itemData.itemName;
    }

    private void Start()
    {
        // Optional: fallback for manually placed pickups (if not dropped)
        if (itemData != null)
        {
            GetComponent<SpriteRenderer>().sprite = itemData.icon;
        }
    }
}
