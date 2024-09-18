using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScenePreloadManager))]
public class ScenePreloadManagerEditor : Editor
{
    private void OnEnable()
    {
        // Ensures that the target is an instance of ScenePreloadManager
        ScenePreloadManager manager = (ScenePreloadManager)target;
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

        DrawDefaultInspector();
    }
}
