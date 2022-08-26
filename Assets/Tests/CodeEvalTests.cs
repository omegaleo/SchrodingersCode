using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CodeEvalTests
{
    private static readonly object[] _sourceLists = 
    {
        new object[] {new List<CodeBlock>
        {
            new CodeBlock(type: CodeBlockType.Int, value: "2"),
            new CodeBlock(type: CodeBlockType.Condition, value: "<"),
            new CodeBlock(type: CodeBlockType.Int, value: "1")
        }},   //case 1
    };

    [Test]
    [TestCaseSource(nameof(_sourceLists))]
    public void CodeEvalTestsSimplePasses(List<CodeBlock> codeBlocks)
    {
        CodeEval eval = new CodeEval();
    }
}
