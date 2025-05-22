using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public GameObject goToCampSign;
    private bool playerInTrigger = false;
    public AudioClip clickSound;

    void Start()
    {
        if (goToCampSign != null)
        {
            goToCampSign.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(PlaySoundAndLoadScene());
        }
    }

    private IEnumerator PlaySoundAndLoadScene()
    {
        AudioSource.PlayClipAtPoint(clickSound, transform.position);
        yield return new WaitForSeconds(clickSound.length); // Wait for sound to finish
        GameManager.Instance.LevelComplete();
        SceneManager.LoadScene("Camp");
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
