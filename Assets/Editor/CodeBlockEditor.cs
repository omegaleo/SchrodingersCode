using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(CodeBlock))]
public class CodeBlockEditor : Editor
{
    private List<string> conditions = new List<string>()
    {
        ">=",
        ">",
        "<",
        "<=",
        "AND",
        "OR",
        "NOT",
    };

    private List<string> operators = new List<string>()
    {
        "+",
        "-",
        "*",
        ":",
        "=",
    };

    public override void OnInspectorGUI()
    {
        var block = (CodeBlock) target;

        EditorGUILayout.LabelField("Configuration");
        block.type = (CodeBlockType)EditorGUILayout.EnumPopup("Type", block.type);
        int selection = 0;
        switch (block.type)
        {
            case CodeBlockType.Condition:
                selection = EditorGUILayout.Popup("Value", conditions.FindIndex(x => x == block.value), conditions.ToArray());
                block.value = conditions[selection];
                break;
            case CodeBlockType.Operator:
                selection = EditorGUILayout.Popup("Value", operators.FindIndex(x => x == block.value), operators.ToArray());
                block.value = operators[selection];
                break;
            case CodeBlockType.Int:
                int.TryParse(block.value, out int value);
                block.value = EditorGUILayout.IntField("Value", value).ToString();
                break;
            default:
                block.value = EditorGUILayout.TextField("Value", block.value);
                break;
        }
        block.locked = EditorGUILayout.Toggle("Locked", block.locked);
        
        EditorGUILayout.LabelField("UI");
        block.text = (TMP_Text)EditorGUILayout.ObjectField("TMP_Text", block.text, typeof(TMP_Text));
        block.lockImg = (Image)EditorGUILayout.ObjectField("Lock Image", block.lockImg, typeof(Image));

        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(block);
        }
        
        EditorGUILayout.LabelField("Test Methods");
        if (GUILayout.Button("Set Text"))
        {
            block.SetText();
        }
        if (GUILayout.Button("Toggle Glitched"))
        {
            block.ToggleGlitched();
        }
    }
}