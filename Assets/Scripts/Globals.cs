using System;
using UnityEngine;
using UnityEngine.Tilemaps;

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

public enum EvalCheck
{
    CheckForTrue,
    CheckForBug,
    CheckForFalse
}

public enum OptionsInputType
{
    ScanLines,
    Music,
    SFX
}

public enum SFXType
{
    ButtonClick,
    Glitch,
    Pickup,
    Drop,
    Trophy,
    DoorOpen,
    Error
}

[Serializable]
public class DirectionPlacementAnchors
{
    public LookingDirection direction;
    public Vector2 offset;
}

[Serializable]
public class DoorTiles
{
    public TileBase leftTile;
    public TileBase rightTile;
}

[Serializable]
public class BlockTileAssociation
{
    public Vector3Int tilePos;
    public Transform codeBlock;

    public BlockTileAssociation()
    {
    }

    public BlockTileAssociation(Vector3Int pos, Transform codeBlock)
    {
        tilePos = pos;
        this.codeBlock = codeBlock;
    }
    
    public void CenterBlockOnTile(Tilemap tilemap)
    {
        if (Camera.main != null)
        {
            codeBlock.position = tilemap.CellToWorld(new Vector3Int(tilePos.x, tilePos.y, 0)) + new Vector3(0.5f, 0.5f, 0);
        }
    }
}

[Serializable]
public class AudioClipAssociation
{
    public AudioClip clip;
    public SFXType type;
}