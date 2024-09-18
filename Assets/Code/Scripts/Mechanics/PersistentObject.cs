using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    private static List<string> existingObjects = new List<string>();

    // Esta lista será mostrada no Inspector como uma cópia da lista estática
    [SerializeField] private List<string> debugExistingObjects = new List<string>();

    private void Awake()
    {
        // Atualiza a lista no Inspector para exibição
        UpdateInspectorList();
    }

    // Atualiza a lista que é exibida no Inspector
    private void UpdateInspectorList()
    {
        debugExistingObjects.Clear();
        debugExistingObjects.AddRange(existingObjects);
    }
}
