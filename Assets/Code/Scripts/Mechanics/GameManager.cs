using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerObject;
    [ReadOnly] public string TagPlayer = "Player";

    [Header("Grid")]
    public GameObject gridObject;
    [ReadOnly] public string TagGrid = "Grid";

    [Header("HotBar")]
    public GameObject hotBarObject;
    [ReadOnly] public string TagHotBar = "HotBar";

    [Header("Inventory Controller")]
    public GameObject inventoryControllerObject;
    [ReadOnly] public string TagInventoryController = "Canvas";

    [Header("All Scenes")]
    [SerializeField] private List<GameObject> inspectorConfinners;
    [SerializeField] private List<GameObject> inspectorTilemaps;

    // Variáveis estáticas
    public static List<GameObject> Confinners = new List<GameObject>();
    public static List<GameObject> Tilemaps = new List<GameObject>();
    public static GameObject player;
    public static GameObject grid;
    public static GameObject hotBar;
    public static GameObject inventoryController;

    private void Start()
    {
        // Registra o método OnSceneLoaded como ouvinte do evento sceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(ExecuteRoutine());
    }

    private void OnDestroy()
    {
        // Remove o método OnSceneLoaded do evento sceneLoaded ao destruir o objeto
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        inspectorConfinners = new List<GameObject>(Confinners);
        inspectorTilemaps = new List<GameObject>(Tilemaps);
    }

    IEnumerator ExecuteRoutine()
    {
        // Espera um frame para garantir que todos os objetos estejam inicializados
        yield return null;

        player = AssignObject(player, playerObject, TagPlayer);
        grid = AssignObject(grid, gridObject, TagGrid);
        hotBar = AssignObject(hotBar, hotBarObject, TagHotBar);
        inventoryController = AssignObject(inventoryController, inventoryControllerObject, TagInventoryController);

        playerObject = player;
        gridObject = grid;
        hotBarObject = hotBar;
        inventoryControllerObject = inventoryController;

        player.GetComponent<PlayerData_Mechanics>().Tile = grid.GetComponent<IdentifyTile>();
        player.GetComponent<PlayerData_Mechanics>().hotbarController = hotBar.GetComponent<HotbarController>();
        hotBar.GetComponent<HotbarController>().PlayerMechanics = player.GetComponent<PlayerData_Mechanics>();
        inventoryController.GetComponent<InventoryController>().input = player.GetComponent<PlayerData_Input>();

        player.GetComponentInChildren<PreviewCheck>().IdentifyTile = grid.GetComponent<IdentifyTile>();
        player.GetComponent<PlayerUseItem>().inventoryController = inventoryController.GetComponent<InventoryController>();

        var gridTilemap = FindChildWithTagAndLayer(grid, "Downloadable", LayerMask.NameToLayer("Ground"));
        player.GetComponent<PlayerData_Mechanics>().ToPlace.Tilemap = gridTilemap;

        CenaryObjects.identifyTile = grid.GetComponent<IdentifyTile>();
    }

    public static GameObject AssignObject(GameObject staticObject, GameObject inspectorObject, string tag)
    {
        if (staticObject == null)
        {
            return inspectorObject != null ? inspectorObject : GetObject(tag);
        }

        return staticObject;
    }

    public static GameObject GetObject(string tag)
    {
        return GameObject.FindWithTag(tag);
    }

    private GameObject FindChildWithTagAndLayer(GameObject parent, string tag, int layer)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag) && child.gameObject.layer == layer)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    // Método chamado quando a cena é carregada
    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AddConfinnersToList(scene);
        AddTilemapsToList(scene);
    }

    public static void AddConfinnersToList(Scene scene)
    {
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
        {
            if (rootObject.CompareTag("Confiner"))
            {
                Confinners.Add(rootObject);
            }
        }

        Debug.Log("Confinners encontrados na cena " + scene.name + ": " + Confinners.Count);
    }
public static void AddTilemapsToList(Scene scene)
{
    GameObject[] rootObjects = scene.GetRootGameObjects(); // Pega todos os GameObjects da cena especificada
    
    foreach (GameObject rootObject in rootObjects)
    {
        // Primeiro, buscamos objetos com a tag "EditorOnly"
        if (rootObject.CompareTag("EditorOnly"))
        {
            // Agora, buscamos dentro do objeto com a tag "EditorOnly" por filhos com a tag "Downloadable"
            AddTilemapsFromEditorOnly(rootObject);
        }
    }

    Debug.Log("Tilemaps encontrados na cena " + scene.name + ": " + Tilemaps.Count);
}

// Método para buscar Tilemaps dentro de objetos com a tag "EditorOnly"
private static void AddTilemapsFromEditorOnly(GameObject editorOnlyObject)
{
    // Percorre todos os filhos do objeto "EditorOnly"
    foreach (Transform child in editorOnlyObject.transform)
    {
        // Verifica se o filho tem a tag "Downloadable"
        if (child.CompareTag("Downloadable"))
        {
            Tilemap tilemap = child.GetComponent<Tilemap>();
            if (tilemap != null)
            {
                Tilemaps.Add(child.gameObject); // Adiciona o GameObject à lista se o Tilemap for encontrado
            }
        }
    }
}
}
