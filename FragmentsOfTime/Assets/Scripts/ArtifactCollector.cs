using UnityEngine;

public class ArtifactCollector : MonoBehaviour
{
    public ArtifactCollectionManager collectionManager;
    public ArtifactData artifactToCollect;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collectionManager.MarkCollected(artifactToCollect);
            Debug.Log("Collected: " + artifactToCollect.artifactName);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            collectionManager.ResetCollection();
            Debug.Log("All artifacts reset.");
        }
    }
}
