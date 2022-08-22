using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeBlock : MonoBehaviour
{
    [Header("Configuration")]
    public CodeBlockType type;

    public string value;

    public bool locked = false;
    
    [Header("UI")] 
    public TMP_Text text;
    public Image lockImg;
    
    private void Start()
    {
        SetText();
    }

    public void SetText()
    {
        var lockedText = (locked) ? " (Locked)" : "";
        text.text = value;
        gameObject.name = value + lockedText;
        lockImg.gameObject.SetActive(locked);
    }

    public void SetText(string value)
    {
        this.value = value;
        SetText();
    }
}
