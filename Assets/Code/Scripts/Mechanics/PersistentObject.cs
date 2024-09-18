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

        // Adiciona este objeto à lista estática se ainda não estiver lá
        if (!existingObjects.Contains(gameObject.name))
        {
            existingObjects.Add(gameObject.name);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Se o objeto já existe, destrua este duplicado
            Debug.LogWarning("Object already exists: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    // Atualiza a lista que é exibida no Inspector
    private void UpdateInspectorList()
    {
        debugExistingObjects.Clear();
        debugExistingObjects.AddRange(existingObjects);
    }
}
