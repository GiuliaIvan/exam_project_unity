using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f;
        StartCoroutine(PlayClickAndLoad(sceneName));
    }

    private IEnumerator PlayClickAndLoad(string sceneName)
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(sceneName);
    }


    private IEnumerator DelayedLoad(string sceneName)
    {
        yield return new WaitForSeconds(0.4f); // Wait for sound to play
        SceneManager.LoadScene(sceneName);
    }
}
