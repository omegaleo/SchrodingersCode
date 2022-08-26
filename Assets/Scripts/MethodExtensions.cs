using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class MethodExtensions
{
    public static IEnumerable<Vector3Int> GetTilePositions(this Tilemap tilemap)
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(pos) != null)
            {
                yield return pos;
            }
        }
    }
}