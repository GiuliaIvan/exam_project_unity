using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    public void ShowPanel(ItemData data)
    {
        titleText.text = data.itemName;
        descriptionText.text = data.description;
        artifactImage.sprite = data.icon;

        gameObject.SetActive(true);
    }


    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
