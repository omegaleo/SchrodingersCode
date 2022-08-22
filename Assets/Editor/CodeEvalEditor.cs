using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CodeEval), true)]
public class CodeEvalEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var codeEval = (CodeEval) target;
        
        if (GUILayout.Button("Set Tiles"))
        {
            codeEval.SetTiles();
        }
        
        base.OnInspectorGUI();
    }
}