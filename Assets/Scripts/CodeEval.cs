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
    [SerializeField] protected List<BlockTileAssociation> blockTiles;
    [SerializeField] protected List<Door> doors;
    [SerializeField] protected Tilemap circuitTilemap;
    [SerializeField] protected UnityEvent onGlitch;
    
    private Tilemap _tilemap;

    public bool canEvaluate;
    
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("CodeBlock")) return;

        Vector3Int lPos = _tilemap.WorldToCell(col.transform.position);
        
        if (col.gameObject.GetComponent<CodeBlock>().locked)
        {
            var blockTile = blockTiles.FirstOrDefault(x => x.tilePos == lPos);

            if (blockTile != null)
            {
                blockTile.codeBlock = col.transform;
                blockTile.CenterBlockOnTile(_tilemap);
                col.GetComponent<CodeBlock>().SetEval(this);
            }
        }
        else
        {
            var blockTile = blockTiles.FirstOrDefault(x => x.tilePos == lPos && (x.codeBlock != col.transform && x.codeBlock == null));

            if (blockTile == null)
            {
                blockTile = blockTiles.FirstOrDefault(x => x.codeBlock == null);
            }
        
            if (blockTile != null)
            {
                blockTile.codeBlock = col.transform;
                blockTile.CenterBlockOnTile(_tilemap);
                col.GetComponent<CodeBlock>().SetEval(this);
            }
            else
            {
                PlayerManager.instance.HoldBlock(col.gameObject);
            }

            if (blockTiles.All(x => x.codeBlock != null))
            {
                EvaluateCode();
                canEvaluate = true;
            }
        }
    }

    private void HandleError()
    {
        SFXManager.instance.PlaySound(SFXType.Error);
    }

    // public because of tests
    public virtual void EvaluateCode()
    {
        var isMath = blockTiles.Any(x => x.codeBlock.GetComponent<CodeBlock>().type == CodeBlockType.Operator) && 
                     blockTiles.All(x => x.codeBlock.GetComponent<CodeBlock>().type != CodeBlockType.Condition);

        // Disabled to prevent soft locks
        /*if (blockTiles.Any(x => x.codeBlock.GetComponent<CodeBlock>().glitched))
        {
            return;
        }*/
        
        if (IterateExpression(isMath))
        {
            ActivateCircuits();

            if (doors == null || !doors.Any()) return;
            
            SFXManager.instance.PlaySound(SFXType.DoorOpen);
            doors.ForEach((door) => StartCoroutine(door.OpenDoor()));
        }
        else if(!isMath)
        {
            HandleError();
        }
    }

    protected void ActivateCircuits()
    {
        if (circuitTilemap == null) return;
        
        foreach (var pos in circuitTilemap.GetTilePositions())
        {
            circuitTilemap.SetTile(pos, GameManager.instance.activatedCircuitTile);
        }
    }
    
    protected void DeActivateCircuits()
    {
        if (circuitTilemap == null) return;
        
        foreach (var pos in circuitTilemap.GetTilePositions())
        {
            circuitTilemap.SetTile(pos, GameManager.instance.deActivatedCircuitTile);
        }
    }

    public bool IterateExpression(bool isMath)
    {
        var currentValue = "";
        bool expression = false, value = false;

        for (var i = 0; i < blockTiles.Count; i++)
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
                    
                    if (SetValue(isMath, nextCodeBlock, currentValue))
                    {
                        break;
                    }
                    else
                    {
                        if (i - 1 < 0)
                        {
                            ToggleGlitchedBlock(nextCodeBlock);
                        }
                        else
                        {
                            var previousBlock = blockTiles[i - 1];
                            var previousCodeBlock = previousBlock.codeBlock.GetComponent<CodeBlock>();

                            for (var j = i + 1; j < blockTiles.Count; j++)
                            {
                                var subBlock = blockTiles[j];
                                var subCodeBlock = subBlock.codeBlock.GetComponent<CodeBlock>();
                                currentValue += subCodeBlock.value;
                            }

                            if (SetValue(isMath, previousCodeBlock, currentValue))
                            {
                                break;
                            }
                            else
                            {
                                ToggleGlitchedBlock(codeBlock);
                            }
                        }
                    }
                }
            }
            catch (ConditionalException e)
            {
                ToggleGlitchedBlock();
            }
            catch (Exception e)
            {
                HandleError();
            }
        }

        try
        {
            if (expression)
            {
                value = EvaluateExpression(currentValue);
            }
        }
        catch (ConditionalException e)
        {
            ToggleGlitchedBlock();
        }
        catch (Exception e)
        {
            HandleError();
        }
        
        
        return value;
    }

    private bool SetValue(bool isMath, CodeBlock codeBlock, string value)
    {
        if (codeBlock.type == CodeBlockType.Blank)
        {
            if (isMath)
            {
                codeBlock.value = new DataTable().Compute(value, null).ToString();
                codeBlock.type = CodeBlockType.Int;
            }
            else
            {
                codeBlock.value = EvaluateExpression(value).ToString();
                codeBlock.type = CodeBlockType.Boolean;
            }

            codeBlock.SetText();
            return true;
        }

        return false;
    }

    private void ToggleGlitchedBlock(CodeBlock block = null)
    {
        var glitchedBlock = block ?? blockTiles
            .Select(x => x.codeBlock.GetComponent<CodeBlock>())
            .FirstOrDefault(x => x.type == CodeBlockType.Int);
        
        if (glitchedBlock == null) return;
        
        glitchedBlock.ToggleGlitched();
        SFXManager.instance.PlaySound(SFXType.Glitch);

        onGlitch?.Invoke();
    }

    private bool EvaluateExpression(string expression)
    {
        var parts = expression.Split();

        if (parts.Length != 3)
            throw new Exception("Invalid Input String");
        
        if (string.IsNullOrEmpty(parts[0]) ||
            string.IsNullOrWhiteSpace(parts[0])|| 
            string.IsNullOrEmpty(parts[2]) ||
            string.IsNullOrWhiteSpace(parts[2]))
        {
            throw new ConditionalException("Error: Invalid block in conditions");
        }
            
        var argument1 = Int32.Parse(parts[0]);
        var @operator = parts[1];
        var argument2 = Int32.Parse(parts[2]);

        return @operator switch
        {
            "==" => argument1 == argument2,
            ">" => argument1 > argument2,
            ">=" => argument1 >= argument2,
            "<" => argument1 < argument2,
            "<=" => argument1 <= argument2,
            _ => false
        };
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("CodeBlock")) return;
        
        RemoveBlock(other.gameObject);
        canEvaluate = false;
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
        if (!blockTiles.Any(x => x.codeBlock != null && x.codeBlock.gameObject == block)) return;
        
        blockTiles.FirstOrDefault(x => x.codeBlock != null && x.codeBlock.gameObject == block)!.codeBlock = null;

        DeActivateCircuits();

        if (doors == null || !doors.Any()) return;
        
        SFXManager.instance.PlaySound(SFXType.DoorOpen);
        doors.ForEach((door) => StartCoroutine(door.CloseDoor()));
    }
}
