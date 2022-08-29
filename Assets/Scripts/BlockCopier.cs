using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockCopier : CodeEval
{
    public override void EvaluateCode()
    {
        var codeBlocks = blockTiles.Select(x => x.codeBlock.GetComponent<CodeBlock>());

        var blank = codeBlocks.FirstOrDefault(x => x.type == CodeBlockType.Blank);
        var value = codeBlocks.FirstOrDefault(x => x.type != CodeBlockType.Blank);

        if (blank != null && value != null)
        {
            ActivateCircuits();
            SFXManager.instance.PlaySound(SFXType.ButtonClick);
            blank.glitched = value.glitched;
            blank.type = value.type;
            blank.SetText(value.value);
        }
        else
        {
            DeActivateCircuits();
        }
    }
}
