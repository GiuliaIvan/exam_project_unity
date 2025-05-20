using UnityEngine;
using System.Collections.Generic;

public class ArtifactCollectionManager : MonoBehaviour
{
    public List<ArtifactButton> artifactButtons;

    // Called when a new artifact is collected
    public void MarkCollected(ItemData data)
    {
        data.collected = true;

        foreach (ArtifactButton button in artifactButtons)
        {
            if (button.itemData == data)
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
            button.itemData.collected = false;
        }
    }

    public List<ItemData> GetCollectedArtifacts()
    {
        List<ItemData> collected = new List<ItemData>();
        foreach (ArtifactButton button in artifactButtons)
        {
            if (button.itemData.collected)
            {
                collected.Add(button.itemData);
            }
        }
        return collected;
    }

}
