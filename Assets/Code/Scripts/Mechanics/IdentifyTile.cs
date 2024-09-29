using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class IdentifyTile : MonoBehaviour
{
    public IdentifiedObjectResult identifiedObjectResult = new();

    public class IdentifiedObjectResult
    {
        public TileBase Tile { get; set; }
        public GameObject GameObject { get; set; }
    }

    // Lista de Tilemaps dinâmicos que será atualizada automaticamente
    public List<Tilemap> tileMaps = new List<Tilemap>();

    private void Start()
    {
        // Inicializa e atualiza os Tilemaps automaticamente
        UpdateTilemapsFromAllScenes();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // Atualiza a lista de Tilemaps quando uma nova cena é carregada
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateTilemapsFromScene(scene);
    }

    // Remove os Tilemaps da cena descarregada
    private void OnSceneUnloaded(Scene scene)
    {
        RemoveTilemapsFromScene(scene);
    }

    // Atualiza a lista de Tilemaps com todos os ativos nas cenas carregadas
    private void UpdateTilemapsFromAllScenes()
    {
        tileMaps.Clear();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            UpdateTilemapsFromScene(scene);
        }
    }

    // Adiciona os Tilemaps da cena fornecida
    private void UpdateTilemapsFromScene(Scene scene)
    {
        if (scene.isLoaded)
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
            {
                Tilemap[] sceneTilemaps = rootObject.GetComponentsInChildren<Tilemap>();
                tileMaps.AddRange(sceneTilemaps);
            }
        }
    }

    // Remove os Tilemaps da cena fornecida
    private void RemoveTilemapsFromScene(Scene scene)
    {
        List<Tilemap> tilemapsToRemove = new List<Tilemap>();
        foreach (Tilemap tilemap in tileMaps)
        {
            if (tilemap.gameObject.scene == scene)
            {
                tilemapsToRemove.Add(tilemap);
            }
        }

        foreach (Tilemap tilemap in tilemapsToRemove)
        {
            tileMaps.Remove(tilemap);
        }
    }

    // Método com filtro por tag (com compareTag)
    public IdentifiedObjectResult GetTileOrGameObject(Vector3Int tilePos, string compareTag)
    {
        foreach (Tilemap tileMap in tileMaps)
        {
            // Identifica o tile na posição
            TileBase tileIdentified = tileMap.GetTile(tilePos);

            if (tileIdentified != null && tileMap.gameObject.tag == compareTag)
            {
                return new IdentifiedObjectResult { Tile = tileIdentified };
            }

            // Verifica se há um GameObject com a tag específica na posição
            Vector3 worldPos = tileMap.CellToWorld(tilePos);
            Collider2D[] colliders = Physics2D.OverlapPointAll(worldPos);

            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.tag == compareTag)
                {
                    return new IdentifiedObjectResult { GameObject = collider.gameObject };
                }
            }
        }

        // Retorna null se não encontrar nada
        return null;
    }

    // Método sem filtro de tag
    public IdentifiedObjectResult GetTileOrGameObject(Vector3Int tilePos)
    {
        foreach (Tilemap tileMap in tileMaps)
        {
            // Identifica o tile na posição
            TileBase tileIdentified = tileMap.GetTile(tilePos);

            if (tileIdentified != null)
            {
                return new IdentifiedObjectResult { Tile = tileIdentified };
            }

            // Verifica se há um GameObject na posição
            Vector3 worldPos = tileMap.CellToWorld(tilePos);
            Collider2D[] colliders = Physics2D.OverlapPointAll(worldPos);

            foreach (Collider2D collider in colliders)
            {
                return new IdentifiedObjectResult { GameObject = collider.gameObject };
            }
        }

        // Retorna null se não encontrar nada
        return null;
    }

    // Método para obter apenas o tile
    public IdentifiedObjectResult GetTile(Vector3Int tilePos)
    {
        foreach (Tilemap tileMap in tileMaps)
        {
            // Identifica o tile na posição
            TileBase tileIdentified = tileMap.GetTile(tilePos);

            if (tileIdentified != null)
            {
                return new IdentifiedObjectResult { Tile = tileIdentified };
            }
        }

        // Retorna null se não encontrar nada
        return null;
    }

    // Retorna o sprite correspondente ao TileData.
    public Sprite GetTileSprite(TileBase tile, Vector3Int tilePosition)
    {
        foreach (Tilemap tileMap in tileMaps)
        {
            if (tile is RuleTile ruleTile)
            {
                TileData tileData = new();
                ruleTile.GetTileData(tilePosition, tileMap, ref tileData);
                return tileData.sprite;
            }
        }

        return null;
    }
}
