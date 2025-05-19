using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CompositeCollider2D))]
public class ShadowCaster2DFromComposite : MonoBehaviour
{
    void Start()
    {
        if (!Application.isPlaying) return; // Run only in play mode
        GenerateShadowCasters();
    }

    void GenerateShadowCasters()
    {
        CompositeCollider2D composite = GetComponent<CompositeCollider2D>();
        if (composite == null)
        {
            Debug.LogWarning("CompositeCollider2D not found.");
            return;
        }

        // Remove old casters
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        for (int i = 0; i < composite.pathCount; i++)
        {
            int pointsCount = composite.GetPathPointCount(i);
            Vector2[] path = new Vector2[pointsCount];
            composite.GetPath(i, path);

            GameObject caster = new GameObject("ShadowCaster2D");
            caster.transform.SetParent(transform, false);
            caster.layer = LayerMask.NameToLayer("ShadowCasterLayer");

            var shadowCaster = caster.AddComponent<ShadowCaster2D>();
            var poly = caster.AddComponent<PolygonCollider2D>();
            poly.points = path;
            poly.isTrigger = true;

#if UNITY_EDITOR
            shadowCaster.useRendererSilhouette = false;
#endif
        }

        Debug.Log("Shadow casters created.");
    }
}
