using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3Int leftDoor;
    [SerializeField] private Vector3Int rightDoor;

    [SerializeField] private List<DoorTiles> tiles;

    [SerializeField] private float secondsToWait = 0.1f;
    
    public IEnumerator OpenDoor()
    {
        var tilemap = GetComponent<Tilemap>();
        
        foreach (var tile in tiles)
        {
            tilemap.SetTile(leftDoor, tile.leftTile);
            tilemap.SetTile(rightDoor, tile.rightTile);

            yield return new WaitForSeconds(secondsToWait);
        }
    }
}
