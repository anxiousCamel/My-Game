using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IdentifyTile : MonoBehaviour
{
    public IdentifiedObjectResult identifiedObjectResult = new();

    public class IdentifiedObjectResult
    {
        public TileBase Tile { get; set; }
        public GameObject GameObject { get; set; }
    }

    public List<Tilemap> tileMaps = new List<Tilemap>();

    // Retorna o TileData correspondente a posição.
    public IdentifiedObjectResult GetTileOrGameObject(Vector3Int tilePos, string compareTag)
    {
        foreach (Tilemap tileMap in tileMaps)
        {
            // Tenta identificar o tile na posição especificada
            TileBase tileIdentified = tileMap.GetTile(tilePos);

            // Se encontrou o tile e a tag do gameObject do tileMap é igual a compareTag
            if (tileIdentified != null && tileMap.gameObject.tag == compareTag)
            {
                return new IdentifiedObjectResult { Tile = tileIdentified };
            }

            // Se não encontrou o tile ou a tag não corresponde, procura por um GameObject na mesma posição
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

        // Retorna null se não encontrou um tile nem um GameObject com a tag especificada
        return null;
    }

    public IdentifiedObjectResult GetTileOrGameObject(Vector3Int tilePos)
    {
        foreach (Tilemap tileMap in tileMaps)
        {
            // Tenta identificar o tile na posição especificada
            TileBase tileIdentified = tileMap.GetTile(tilePos);

            if (tileIdentified != null)
            {
                return new IdentifiedObjectResult { Tile = tileIdentified };
            }

            // Se não encontrou o tile, procura por um GameObject na mesma posição
            Vector3 worldPos = tileMap.CellToWorld(tilePos);
            Collider2D[] colliders = Physics2D.OverlapPointAll(worldPos);

            foreach (Collider2D collider in colliders)
            {
                return new IdentifiedObjectResult { GameObject = collider.gameObject };
            }
        }

        // Retorna null se não encontrou um tile nem um GameObject
        return null;
    }

    public IdentifiedObjectResult GetTile(Vector3Int tilePos)
    {
        foreach (Tilemap tileMap in tileMaps)
        {
            // Tenta identificar o tile na posição especificada
            TileBase tileIdentified = tileMap.GetTile(tilePos);

            if (tileIdentified != null)
            {
                return new IdentifiedObjectResult { Tile = tileIdentified };
            }
        }

        // Retorna null se não encontrou um tile
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
