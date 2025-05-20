using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public GameObject goToCampSign; // Assign this in the inspector
    private bool playerInTrigger = false;

    void Start()
    {
        if (goToCampSign != null)
        {
            goToCampSign.SetActive(false); // Hide at start
        }
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Camp");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (goToCampSign != null)
            {
                goToCampSign.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            if (goToCampSign != null)
            {
                goToCampSign.SetActive(false);
            }
        }
    }
}
