using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level1_BlockPlacement_Glitch : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap wallTilemap;
    
    [SerializeField] private List<Vector2Int> groundTilePlacePositions = new List<Vector2Int>();
    [SerializeField] private List<Vector2Int> wallTilePlacePositions = new List<Vector2Int>();
    [SerializeField] private List<Vector2Int> wallTileDestroyPositions = new List<Vector2Int>();
    
    public void Execute()
    {
        foreach (var tile in groundTilePlacePositions)
        {
            groundTilemap.SetTile(new Vector3Int(tile.x, tile.y), GameManager.instance.groundTile);
        }
        
        foreach (var tile in wallTilePlacePositions)
        {
            wallTilemap.SetTile(new Vector3Int(tile.x, tile.y), GameManager.instance.wallTile);
        }
        
        foreach (var tile in wallTileDestroyPositions)
        {
            wallTilemap.SetTile(new Vector3Int(tile.x, tile.y), null);
        }
    }
}
