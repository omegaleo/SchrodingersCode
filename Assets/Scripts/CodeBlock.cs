using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodeBlock : MonoBehaviour
{
    [Header("Configuration")]
    public CodeBlockType type;

    public string value;

    [Header("UI")] 
    public TMP_Text text;
    
    private void Start()
    {
        SetText();
    }

    public void SetText()
    {
        text.text = value;
    }
}
