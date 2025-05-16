using UnityEngine;
using System.Collections.Generic;

public class ArtifactCollectionManager : MonoBehaviour
{
    public List<ArtifactButton> artifactButtons;

    // Called when a new artifact is collected
    public void MarkCollected(ArtifactData data)
    {
        data.collected = true;

        foreach (ArtifactButton button in artifactButtons)
        {
            if (button.artifactData == data)
            {
                button.SetCollected(true);
            }
        }
    }

    // Optional: resets all artifacts to uncollected
    public void ResetCollection()
    {
        foreach (ArtifactButton button in artifactButtons)
        {
            button.SetCollected(false);
            button.artifactData.collected = false;
        }
    }

    public List<ArtifactData> GetCollectedArtifacts()
    {
        List<ArtifactData> collected = new List<ArtifactData>();
        foreach (ArtifactButton button in artifactButtons)
        {
            if (button.artifactData.collected)
            {
                collected.Add(button.artifactData);
            }
        }
        return collected;
    }

}
