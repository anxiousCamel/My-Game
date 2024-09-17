using UnityEngine;
using UnityEditor;
using System.IO;

public class SceneEnumGenerator : MonoBehaviour
{
    [MenuItem("Tools/Generate Scene Enum")]
    public static void GenerateSceneEnum()
    {
        string enumName = "Scenes";
        string filePath = "Assets/Code/Plugins/" + enumName + ".cs";

        // Certifique-se de que o diretório existe
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine("public enum " + enumName);
            sw.WriteLine("{");

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                string scenePath = EditorBuildSettings.scenes[i].path;
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                sw.WriteLine("    " + sceneName + " = " + i + ",");
            }

            sw.WriteLine("}");
        }

        AssetDatabase.Refresh();
        Debug.Log("Scene enum generated at " + filePath);
    }
}
