using UnityEngine;
using UnityEngine.UI;

public class ArtifactButton : MonoBehaviour
{
    public ArtifactData artifactData;
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
        if (artifactData.collected)
        {
            image.sprite = artifactData.buttonActiveSprite;
            button.interactable = true;
        }
        else
        {
            image.sprite = artifactData.buttonInactiveSprite;
            button.interactable = false;
        }
    }

    public void OnClick()
    {
        infoPanel.ShowPanel(artifactData);
    }

    // Call this later when the artifact gets collected
    public void SetCollected(bool collected)
    {
        artifactData.collected = collected;
        RefreshVisual();
    }
}
