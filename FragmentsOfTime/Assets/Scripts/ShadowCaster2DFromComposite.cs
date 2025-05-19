using UnityEngine;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[RequireComponent(typeof(CompositeCollider2D))]
public class ShadowCaster2DFromComposite : MonoBehaviour
{
    void Start()
    {
        if (!Application.isPlaying) return;
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

        // Remove existing shadow casters
        foreach (Transform child in transform)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(child.gameObject);
            else
                Destroy(child.gameObject);
#else
            Destroy(child.gameObject);
#endif
        }

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
            // Makes shadow show up immediately in editor mode
            shadowCaster.useRendererSilhouette = false;
            SerializedObject so = new SerializedObject(shadowCaster);
            so.FindProperty("m_UseRendererSilhouette").boolValue = false;
            so.FindProperty("m_ShapePath").ClearArray();
            for (int j = 0; j < path.Length; j++)
            {
                so.FindProperty("m_ShapePath").InsertArrayElementAtIndex(j);
                so.FindProperty("m_ShapePath").GetArrayElementAtIndex(j).vector3Value = path[j];
            }
            so.ApplyModifiedProperties();
#endif
        }

        Debug.Log("Shadow casters created.");
    }
}
