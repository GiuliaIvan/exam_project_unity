using UnityEngine;

public class CampManager : MonoBehaviour
{
    public GameObject menuOverlay;

    public void ShowMenu()
    {
        menuOverlay.SetActive(true);
    }

    public void HideMenu()
    {
        menuOverlay.SetActive(false);
    }

}
