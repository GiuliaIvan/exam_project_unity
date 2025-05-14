using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    public GameObject inventoryPanel;
    public Image[] slotImages;
    public Sprite emptySlotSprite;
    public Color highlightColor;
    public Color defaultColor;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            ItemData item = InventorySystem.Instance.slots[i];
            slotImages[i].sprite = item ? item.icon : emptySlotSprite;
        }
    }

    public void HighlightSlot(int index)
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].color = i == index ? highlightColor : defaultColor;
        }
    }
}
