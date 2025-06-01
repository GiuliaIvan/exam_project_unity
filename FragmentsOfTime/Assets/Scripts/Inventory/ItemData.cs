using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject worldPrefab;
    [TextArea] public string description;
    public Sprite buttonActiveSprite;      // Button image when collected
    public Sprite buttonInactiveSprite;

    public float itemWeight;    // Button image when not collected

    public bool collected;                 // Is the artifact collected?
}
