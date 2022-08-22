using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CodeEval : MonoBehaviour
{
    [SerializeField] private List<BlockTileAssociation> blockTiles;

    private Tilemap _tilemap;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("CodeBlock"))
        {
            Vector3Int lPos = _tilemap.WorldToCell(col.transform.position);
            var blockTile = blockTiles.FirstOrDefault(x => x.tilePos == lPos);
            
            if (blockTile != null)
            {
                blockTile.codeBlock = col.transform;
                blockTile.CenterBlockOnTile(_tilemap);
            }
        }
    }

    private void Start()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    public void SetTiles()
    {
        if (_tilemap == null)
        {
            _tilemap = GetComponent<Tilemap>();
        }
        
        foreach (var pos in _tilemap.cellBounds.allPositionsWithin)
        {
            if (blockTiles.All(block => block.tilePos != pos))
            {
                blockTiles.Add(new BlockTileAssociation(pos, null));
            }
        }
    }
}
