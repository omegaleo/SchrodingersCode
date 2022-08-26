using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class CodeEval : MonoBehaviour
{
    [SerializeField] private List<BlockTileAssociation> blockTiles;
    [SerializeField] private List<Door> doors;
    [SerializeField] private Tilemap circuitTilemap;
    [SerializeField] private UnityEvent onGlitch;
    
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
                col.GetComponent<CodeBlock>().SetEval(this);
            }

            if (blockTiles.All(x => x.codeBlock != null))
            {
                EvaluateCode();
            }
        }
    }

    private void HandleError()
    {
        
    }

    // public because of tests
    public void EvaluateCode()
    {
        bool isMath = blockTiles.Any(x => x.codeBlock.GetComponent<CodeBlock>().type == CodeBlockType.Operator) && 
                      blockTiles.All(x => x.codeBlock.GetComponent<CodeBlock>().type != CodeBlockType.Condition);

        if (blockTiles.Any(x => x.codeBlock.GetComponent<CodeBlock>().glitched))
        {
            return;
        }
        
        if (IterateExpression(isMath) && doors != null)
        {
            ActivateCircuits();
            foreach (Door door in doors)
            {
                StartCoroutine(door.OpenDoor());
            }
        }
        else if(!isMath)
        {
            HandleError();
        }
    }

    private void ActivateCircuits()
    {
        if (circuitTilemap != null)
        {
            foreach (var pos in circuitTilemap.GetTilePositions())
            {
                circuitTilemap.SetTile(pos, GameManager.instance.activatedCircuitTile);
            }
        }
    }

    private bool IterateExpression(bool isMath)
    {
        string currentValue = "";
        bool expression = false, value = false;

        for (int i = 0; i < blockTiles.Count; i++)
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
                        nextCodeBlock.ToggleGlitched();

                        if (onGlitch != null)
                        {
                            onGlitch.Invoke();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        if (expression)
        {
            value = EvaluateExpression(currentValue);
        }
        
        return value;
    }

    private bool EvaluateExpression(string expression)
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
                case ">=":
                    return argument1 >= argument2;
                case "<":
                    return argument1 < argument2;
                case "<=":
                    return argument1 <= argument2;
                default:
                    return false;
            }
        }
        catch
        {
            return false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("CodeBlock"))
        {
            RemoveBlock(other.gameObject);
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

        foreach (var pos in _tilemap.GetTilePositions())
        {
            if (blockTiles.All(block => block.tilePos != pos))
            {
                blockTiles.Add(new BlockTileAssociation(pos, null));
            }
        }
    }

    public void RemoveBlock(GameObject block)
    {
        if (blockTiles.Any(x => x.codeBlock != null && x.codeBlock.gameObject == block))
        {
            blockTiles.FirstOrDefault(x => x.codeBlock != null && x.codeBlock.gameObject == block)!.codeBlock = null;
        }
    }
}
