using UnityEngine;
using UnityEditor;

public class PreviewSpriteAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(PreviewSpriteAttribute))]
public class PreviewSpriteDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Draw the sprite preview
        if (property.objectReferenceValue != null)
        {
            Sprite sprite = (Sprite)property.objectReferenceValue;
            Rect spriteRect = new Rect(position.x, position.y, 64, 64);
            EditorGUI.ObjectField(spriteRect, property.objectReferenceValue, typeof(Sprite), false);
        }
        else
        {
            EditorGUI.PropertyField(position, property, GUIContent.none);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 64;
    }
}
#endif
