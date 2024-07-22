using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
         serializedObject.Update();

        // Add a button to generate a new unique ID
        if (GUILayout.Button("Generate Unique ID"))
        {
            Item item = (Item)target;
            item.id = item.GenerateUniqueID();
            EditorUtility.SetDirty(item);
        }

        // Draw custom layout
        EditorGUILayout.BeginHorizontal();

        // Draw Sprite
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("icon"), GUIContent.none, GUILayout.Width(64), GUILayout.Height(64));
        EditorGUILayout.EndVertical();

        // Draw Other Fields
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("id"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemPrefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();


        // Draw remaining fields as default, excluding the ones already drawn and "m_Script"
        DrawPropertiesExcluding(serializedObject, "m_Script", "icon", "id", "itemPrefab", "itemName");

        serializedObject.ApplyModifiedProperties();
}

}