using UnityEngine;
using TMPro;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;

    public TMP_Text lifeText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateLives(int lives)
    {
        if (lifeText != null)
        {
            lifeText.text = " " + lives;
        }
    }
}
