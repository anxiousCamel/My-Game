using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IdentifyTileCache
{
    private static IdentifyTile identifyTile;
    public static bool canIdentify;

    public static IdentifyTile GetIdentifyTile()
    {
        if (identifyTile == null)
        {
            GameObject tileObject = GameManager.grid;
            if (tileObject != null)
            {
                identifyTile = tileObject.GetComponent<IdentifyTile>();
            }
            else
            {
                Debug.LogWarning("Tile with tag 'Grid' not found!");
            }
        }
        return identifyTile;
    }
}