using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PreviewCheck : MonoBehaviour
{
    [Header("Components References")]
    public SpriteRenderer sprite;

    [Header("Scripts References")]
    public IdentifyTile IdentifyTile;
    [ReadOnly] public PlayerData_Collider Collider;
    [ReadOnly] public PlayerData_Mechanics Mechanics;

    [Header("MovePreview")]
    public Transform player;
    public Vector3 offset = new Vector3(1f, 0f, 0f);

    [Header("Colors")]
    public Color available;
    public Color unavailable;


    private float gridSize = 1f;
    private Vector3 previousPlayerPosition;

    void Start()
    {
        previousPlayerPosition = player.position;
        Collider = GetComponentInParent<PlayerData_Collider>();
        sprite = GetComponentInParent<SpriteRenderer>();
        Mechanics = GetComponentInParent<PlayerData_Mechanics>();
    }

    void Update()
    {
        MovePreview();
        ChangeColor();
        Flip();
    }

    private void Flip()
    {
        if (player.rotation.y == 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        else if (player.rotation.y == -1)
        {
            transform.localRotation = Quaternion.Euler(0, -180, 0);
        }
    }

    private void ChangeColor()
    {
        if (Mechanics.Carry.tileSprite != null && !Mechanics.Carry.downloadToGetItem && !Mechanics.Target.canMoveTarget)
        {
            sprite.enabled = true;
            sprite.color = !CheckTile() && CheckTileBelow() ? available : unavailable;
            Mechanics.ToPlace.canPlace = !CheckTile() && CheckTileBelow() ? true : false;
        }
        else
        {
            sprite.enabled = false;
        }
    }

    void MovePreview()
    {
        Vector3 playerPosition = player.position;

        // Calcula o offset baseado na rotação do jogador
        float offsetX = (player.rotation.eulerAngles.y == 0f) ? offset.x : -offset.x + -1;

        // Calcula a posição na grade em unidades do Unity, adicionando o deslocamento
        float gridX = Mathf.Round((playerPosition.x + offsetX) / gridSize) * gridSize;
        float gridY = Mathf.Round((playerPosition.y + offset.y) / gridSize) * gridSize;

        // Define a nova posição do objeto filho na grade, mantendo a posição Z atual
        transform.position = new Vector3(gridX, gridY, transform.position.z);
    }


    bool CheckTile()
    {
        if (IdentifyTile.tileMaps.Count > 0)
        {
            foreach (Tilemap tileMap in IdentifyTile.tileMaps)
            {
                // Converte a posição do transform para uma posição de tilemap usando cada tilemap na lista
                Vector3Int tilePosition = tileMap.WorldToCell(transform.position);

                // Verifica se o tile ou GameObject está presente na posição
                IdentifyTile.IdentifiedObjectResult result = IdentifyTile.GetTileOrGameObject(tilePosition);

                if (result != null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    bool CheckTileBelow()
    {
        if (IdentifyTile.tileMaps.Count > 0)
        {
            foreach (Tilemap tileMap in IdentifyTile.tileMaps)
            {
                // Converte a posição do transform para uma posição de tilemap usando cada tilemap na lista
                Vector3Int tilePosition = tileMap.WorldToCell(transform.position);

                // Ajusta a posição para verificar o tile abaixo
                Vector3Int tileBelowPosition = new Vector3Int(tilePosition.x, tilePosition.y - 1, tilePosition.z);

                // Verifica se o tile ou GameObject está presente na posição abaixo
                IdentifyTile.IdentifiedObjectResult result = IdentifyTile.GetTileOrGameObject(tileBelowPosition);

                if (result != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
}


