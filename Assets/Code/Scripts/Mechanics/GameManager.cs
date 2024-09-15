using System.Collections;
using Inventory;
using Inventory.UI;
using UnityEngine;

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



    // Variáveis estáticas
    public static GameObject player;
    public static GameObject grid;
    public static GameObject hotBar;
    public static GameObject inventoryController;


    private void Start()
    {
        StartCoroutine(ExecuteRoutine());
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
    }

    /// <summary>
    /// Método genérico para atribuir um GameObject estático.
    /// Primeiro, verifica se o objeto estático já foi atribuído.
    /// Se não, tenta usar o objeto atribuído no Inspector. 
    /// Caso o objeto do Inspector seja nulo, ele busca o objeto pela tag.
    /// </summary>
    /// <param name="staticObject">O GameObject estático que será atribuído, como player ou canvas.</param>
    /// <param name="inspectorObject">O GameObject configurado manualmente no Inspector.</param>
    /// <param name="tag">A tag associada ao objeto a ser encontrado caso o objeto do Inspector seja nulo.</param>
    /// <returns>O GameObject final a ser usado, seja ele do Inspector ou encontrado pela tag.</returns>
    public static GameObject AssignObject(GameObject staticObject, GameObject inspectorObject, string tag)
    {
        if (staticObject == null)
        {
            return inspectorObject != null ? inspectorObject : GetObject(tag);
        }

        return staticObject;
    }

    /// <summary>
    /// Busca um GameObject na cena com base na tag fornecida.
    /// Utiliza o método GameObject.FindWithTag para localizar o objeto.
    /// </summary>
    /// <param name="tag">A tag do GameObject que será buscado.</param>
    /// <returns>O GameObject encontrado com a tag correspondente, ou null se nenhum objeto com a tag for encontrado.</returns>
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
}
