using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public ItemData[] slots = new ItemData[9];
    private int selectedSlot = 0;

    private List<ItemData> collectedArtifactsThisRun = new List<ItemData>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InventoryUIManager.Instance.HighlightSlot(selectedSlot);
    }

    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                InventoryUIManager.Instance.UpdateUI();

                // Add to temporary collected list
                if (!collectedArtifactsThisRun.Contains(item))
                {
                    collectedArtifactsThisRun.Add(item);
                }

                return true;
            }
        }

        Debug.Log("Inventory is full!");
        return false;
    }



    public void SelectSlot(int index)
    {
        selectedSlot = index;
        InventoryUIManager.Instance.HighlightSlot(selectedSlot);
    }

    public void DropSelectedItem(Vector3 dropPosition)
    {
        if (slots[selectedSlot] != null)
        {
            // Instantiate the item prefab at drop position
            GameObject droppedItem = Instantiate(slots[selectedSlot].worldPrefab, dropPosition, Quaternion.identity);

            // Add PickupItem component if it doesn't exist
            PickupItem pickup = droppedItem.GetComponent<PickupItem>();
            if (pickup == null)
            {
                pickup = droppedItem.AddComponent<PickupItem>();
            }

            // Assign the item data
            pickup.itemData = slots[selectedSlot];

            // Remove from inventory
            slots[selectedSlot] = null;

            // Update UI
            InventoryUIManager.Instance.UpdateUI();
        }
    }

    public ItemData GetSelectedItem()
    {
        return slots[selectedSlot];
    }

    public void OnLevelCompleted(bool playerSurvived)
    {
        if (playerSurvived)
        {
            foreach (var item in collectedArtifactsThisRun)
            {
                if (item is ItemData itemData)
                {
                    // Mark as collected in ArtifactCollectionManager
                    ArtifactCollectionManager collectionManager = FindObjectOfType<ArtifactCollectionManager>();
                    if (collectionManager != null)
                    {
                        collectionManager.MarkCollected(itemData);
                    }
                }
            }
        }

        // Clear collected list either way
        collectedArtifactsThisRun.Clear();
    }
    
    public float GetTotalWeight()
    {
        float weight = 0f;
        foreach (var item in slots)
        {
            if (item != null)
                weight += item.itemWeight;
        }
        return weight;
    }



}