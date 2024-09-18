using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScenePreloadManager))]
public class ScenePreloadManagerEditor : Editor
{
    private void OnEnable()
    {
        // Register a callback to repaint the inspector when changes are detected
        EditorApplication.update += RepaintInspector;
    }

    private void OnDisable()
    {
        // Unregister the callback when the editor is disabled
        EditorApplication.update -= RepaintInspector;
    }

    public override void OnInspectorGUI()
    {
        ScenePreloadManager manager = (ScenePreloadManager)target;

        // Display preloaded scenes
        EditorGUILayout.LabelField("Preloaded Scenes", EditorStyles.boldLabel);
        foreach (var kvp in manager.GetPreloadedScenes())
        {
            EditorGUILayout.LabelField(kvp.Key.ToString(), kvp.Value.progress.ToString("F2"));
        }

        // Display activated scenes
        EditorGUILayout.LabelField("Activated Scenes", EditorStyles.boldLabel);
        foreach (var kvp in manager.GetActivatedScenes())
        {
            EditorGUILayout.LabelField(kvp.Key.ToString(), kvp.Value ? "Activated" : "Not Activated");
        }

        DrawDefaultInspector(); // Draw default inspector elements as well
    }

    private void RepaintInspector()
    {
        // Force the inspector to repaint
        if (target != null)
        {
            Repaint();
        }
    }
}
