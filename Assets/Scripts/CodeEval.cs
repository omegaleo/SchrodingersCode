using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CodeEval : MonoBehaviour
{
    [SerializeField] private List<BlockTileAssociation> blockTiles;
    [SerializeField] private Door door;
    
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

            if (blockTiles.All(x => x.codeBlock != null))
            {
                EvaluateCode();
            }
        }
    }

    private void EvaluateCode()
    {
        string currentValue = "";
        bool isMath = blockTiles.Any(x => x.codeBlock.GetComponent<CodeBlock>().type == CodeBlockType.Operator) && 
                      blockTiles.All(x => x.codeBlock.GetComponent<CodeBlock>().type != CodeBlockType.Condition);

        bool expression = false;
        
        for(int i = 0; i < blockTiles.Count; i++)
        {
            var block = blockTiles[i];
            var codeBlock = block.codeBlock.GetComponent<CodeBlock>();

            try
            {
                if (codeBlock.value != "=")
                {
                    if (codeBlock.type != CodeBlockType.Condition)
                    {
                        currentValue += codeBlock.value;
                    }
                    else
                    {
                        currentValue += $" {codeBlock.value} ";
                        expression = true;
                    }
                }
                else
                {
                    var nextBlock = blockTiles[i + 1];
                    var nextCodeBlock = nextBlock.codeBlock.GetComponent<CodeBlock>();

                    if (nextCodeBlock.type == CodeBlockType.Blank)
                    {
                        if (isMath)
                        {
                            nextCodeBlock.value = new DataTable().Compute(currentValue, null).ToString();
                        }
                        else
                        {
                            nextCodeBlock.value = EvaluateExpression(currentValue).ToString();
                        }
                        nextCodeBlock.SetText();
                        break;
                    }
                    else
                    {
                        // Error
                        break;
                    }
                }

                if (expression)
                {
                    bool value = EvaluateExpression(currentValue);

                    if (value && door != null)
                    {
                        StartCoroutine(door.OpenDoor());
                    }
                }
                else
                {
                    // Error
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    public bool EvaluateExpression(String expression)
    {
        var parts = expression.Split();

        if (parts.Length != 3)
            throw new Exception("Invalid Input String");
        try
        {
            var argument1 = Int32.Parse(parts[0]);
            var @operator = parts[1];
            var argument2 = Int32.Parse(parts[2]);

            switch(@operator)
            {
                case "==":
                    return argument1 == argument2;
                case ">":
                    return argument1 > argument2;
                default:
                    throw new Exception("Invalid Input String");
            }
        }
        catch
        {
            throw new Exception("Invalid Input String");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("CodeBlock"))
        {
            Vector3Int lPos = _tilemap.WorldToCell(other.transform.position);
            var blockTile = blockTiles.FirstOrDefault(x => x.tilePos == lPos);

            if (blockTile != null)
            {
                blockTile.codeBlock = null;
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
                if (_tilemap.GetTile(pos) != null)
                {
                    blockTiles.Add(new BlockTileAssociation(pos, null));
                }
            }
        }
    }
}
