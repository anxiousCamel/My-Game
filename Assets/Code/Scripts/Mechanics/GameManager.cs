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

    // Variáveis estáticas
    public static List<GameObject> Confinners = new List<GameObject>();
    public static List<Tilemap> Tilemaps = new List<Tilemap>();  // Lista de Tilemaps
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
        Vector3 playerPosition = player.transform.position;

        // Verifica o melhor Confiner com o qual o player está colidindo

        GameObject bestConfiner = GetConfinerContainingPlayer(playerPosition);
        if (bestConfiner != null)
        {
            player.GetComponent<PlayerData_Mechanics>().GameManager.confiner = bestConfiner;


            Scene confinerScene = bestConfiner.scene;

            // Procura o tilemap na cena do confiner
            GameObject currentTilemap = GetTilemapInConfinerScene(confinerScene);

            if (currentTilemap != null)
            {
                player.GetComponent<PlayerData_Mechanics>().GameManager.Tilemap = currentTilemap;
            }
        }

    }

    IEnumerator ExecuteRoutine()
    {
        yield return null; // Espera um frame para garantir que todos os objetos estejam inicializados

        player = AssignObject(player, playerObject, TagPlayer);
        grid = AssignObject(grid, gridObject, TagGrid);
        hotBar = AssignObject(hotBar, hotBarObject, TagHotBar);
        inventoryController = AssignObject(inventoryController, inventoryControllerObject, TagInventoryController);

        playerObject = player;
        gridObject = grid;
        hotBarObject = hotBar;
        inventoryControllerObject = inventoryController;

        // Inicializa os componentes e referencias necessários
        player.GetComponent<PlayerData_Mechanics>().Tile = grid.GetComponent<IdentifyTile>();
        player.GetComponent<PlayerData_Mechanics>().hotbarController = hotBar.GetComponent<HotbarController>();
        hotBar.GetComponent<HotbarController>().PlayerMechanics = player.GetComponent<PlayerData_Mechanics>();
        inventoryController.GetComponent<InventoryController>().input = player.GetComponent<PlayerData_Input>();

        player.GetComponentInChildren<PreviewCheck>().IdentifyTile = grid.GetComponent<IdentifyTile>();
        player.GetComponent<PlayerUseItem>().inventoryController = inventoryController.GetComponent<InventoryController>();
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

    // Função para detectar o melhor Confiner baseado no PolygonCollider2D
    public GameObject GetConfinerContainingPlayer(Vector3 playerPosition)
    {
        foreach (GameObject confiner in Confinners)
        {
            PolygonCollider2D polygonCollider = confiner.GetComponent<PolygonCollider2D>();
            if (polygonCollider != null && polygonCollider.OverlapPoint(playerPosition))
            {
                return confiner; // Retorna o confiner que contém o player
            }
        }
        return null; // Retorna null se nenhum confiner envolver o player
    }

    // Função para encontrar o Tilemap na cena do Confiner
    public GameObject GetTilemapInConfinerScene(Scene confinerScene)
    {
        // Obtém os objetos raiz da cena do confiner
        GameObject[] rootObjects = confinerScene.GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            // Procura o objeto com a tag "EditorOnly"
            if (rootObject.CompareTag("EditorOnly"))
            {
                // Dentro do objeto com a tag "EditorOnly", procurar o objeto com a tag "Downloadable"
                foreach (Transform child in rootObject.transform)
                {
                    if (child.CompareTag("Downloadable"))
                    {
                        Tilemap tilemap = child.GetComponent<Tilemap>();
                        if (tilemap != null)
                        {
                            return child.gameObject; // Retorna o objeto com o Tilemap
                        }
                    }
                }
            }
        }

        return null; // Retorna null se nenhum Tilemap for encontrado
    }

    // Método chamado quando uma nova cena é carregada
    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AddConfinnersToList(scene);
    }

    // Adiciona Confinners da cena atual à lista
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
    }

    // Método para adicionar Tilemaps da cena à lista
    public static void AddTilemapsToList(Scene scene)
    {
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject rootObject in rootObjects)
        {
            // Encontra todos os Tilemaps na cena
            Tilemap[] tilemaps = rootObject.GetComponentsInChildren<Tilemap>();
            foreach (Tilemap tilemap in tilemaps)
            {
                Tilemaps.Add(tilemap); // Adiciona cada Tilemap à lista
            }
        }
    }
}
