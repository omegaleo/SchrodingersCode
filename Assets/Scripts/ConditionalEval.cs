using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConditionalEval : MonoBehaviour
{
    [SerializeField] private CodeEval condition1;
    [SerializeField] private CodeEval condition2;
    [SerializeField] private CodeBlock conditionalBlock;
    [SerializeField] private List<Door> doors;
    [SerializeField] private Tilemap circuitTilemap;
    
    public bool evalRunning = false;
    
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
    
    private void DeActivateCircuits()
    {
        if (circuitTilemap != null)
        {
            foreach (var pos in circuitTilemap.GetTilePositions())
            {
                circuitTilemap.SetTile(pos, GameManager.instance.deActivatedCircuitTile);
            }
        }
    }
    
    private void Update()
    {
        if (condition1.canEvaluate && condition2.canEvaluate && !evalRunning)
        {
            if (Evaluate())
            {
                ActivateCircuits();
                SFXManager.instance.PlaySound(SFXType.DoorOpen);
                doors.ForEach(door => door.OpenDoor());
            }
            else
            {
                DeActivateCircuits();
                SFXManager.instance.PlaySound(SFXType.DoorOpen);
                doors.ForEach(door => door.CloseDoor());
            }
        }

        evalRunning = condition1.canEvaluate && condition2.canEvaluate;
    }

    public bool Evaluate()
    {
        try
        {
            var conditionValue1 = condition1.IterateExpression(false);
            var conditionValue2 = condition2.IterateExpression(false);
            
            switch(conditionalBlock.value)
            {
                case "AND":
                    return conditionValue1 && conditionValue2;
                case "OR":
                    return conditionValue1 || conditionValue2;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return false;
    }
}
