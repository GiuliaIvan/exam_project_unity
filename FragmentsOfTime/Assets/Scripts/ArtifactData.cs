using UnityEngine;

[CreateAssetMenu(fileName = "New Artifact", menuName = "Artifacts/Artifact")]
public class ArtifactData : ScriptableObject
{
    public string artifactName;
    [TextArea] public string description;
    
    public Sprite artifactSprite;          // Image used in info panel
    public Sprite buttonActiveSprite;      // Button image when collected
    public Sprite buttonInactiveSprite;    // Button image when not collected

    public bool collected;                 // Is the artifact collected?
}
