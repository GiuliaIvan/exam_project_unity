using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArtifactInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image artifactImage;
    public Button closeButton;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HidePanel);
        }
    }

   public void ShowPanel(ArtifactData data)
{
    titleText.text = data.artifactName;
    descriptionText.text = data.description;
    artifactImage.sprite = data.artifactSprite;

    gameObject.SetActive(true);
}


    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
