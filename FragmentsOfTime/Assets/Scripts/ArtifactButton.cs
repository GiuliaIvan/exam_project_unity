using UnityEngine;
using UnityEngine.UI;

public class ArtifactButton : MonoBehaviour
{
    public ItemData itemData;
    public ArtifactInfoPanel infoPanel;

    private Image image;
    private Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        RefreshVisual();
        button.onClick.AddListener(OnClick);
    }

    void RefreshVisual()
    {
        if (itemData.collected)
        {
            image.sprite = itemData.buttonActiveSprite;
            button.interactable = true;
        }
        else
        {
            image.sprite = itemData.buttonInactiveSprite;
            button.interactable = false;
        }
    }

    public void OnClick()
    {
        infoPanel.ShowPanel(itemData);
    }

    // Call this later when the artifact gets collected
    public void SetCollected(bool collected)
    {
        itemData.collected = collected;
        RefreshVisual();
    }
}
