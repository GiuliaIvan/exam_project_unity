using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject tagLetter;
    private bool playerInTrigger = false;
    void Start()
    {
        if (tagLetter != null)
        {
            tagLetter.SetActive(false);
        }
    }

  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (tagLetter != null)
            {
                tagLetter.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            if (tagLetter != null)
            {
                tagLetter.SetActive(false);
            }
        }
    }
}
