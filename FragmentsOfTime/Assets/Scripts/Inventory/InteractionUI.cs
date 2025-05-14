using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance;

    public GameObject fIcon;
    public Image loadingCircle;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowIcon() => fIcon.SetActive(true);
    public void HideIcon() => fIcon.SetActive(false);

    public void StartLoading()
    {
        StartCoroutine(AnimateLoading());
    }

    private IEnumerator AnimateLoading()
    {
        loadingCircle.fillAmount = 0;
        loadingCircle.gameObject.SetActive(true);

        float time = 0f;
        float duration = 1.5f;

        while (time < duration)
        {
            time += Time.deltaTime;
            loadingCircle.fillAmount = time / duration;
            yield return null;
        }

        loadingCircle.gameObject.SetActive(false);
    }

    public void ShowFullInventoryMessage()
    {
        // Optional: Add message popup saying inventory is full.
    }
}
