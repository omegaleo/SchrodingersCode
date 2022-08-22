using System;
using UnityEngine;

public enum CodeBlockType
{
    Int,
    Condition,
    Blank,
    Operator
}

public enum LookingDirection
{
    Up,
    Down,
    Right,
    Left
}

[Serializable]
public class DirectionPlacementAnchors
{
    public LookingDirection direction;
    public Vector2 offset;
}