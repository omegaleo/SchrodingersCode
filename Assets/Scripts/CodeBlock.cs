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
    public bool glitched = false; // Used to determine if a block is glitched and can be used for secrets
    private Vector3 _initialPosition;
    private CodeEval _eval;

    [Header("UI")] 
    public TMP_Text text;

    public Image lockImg;

    private Image _img;

    public CodeBlock()
    {
    }

    public void SetEval(CodeEval eval)
    {
        _eval = eval;
    }

    public void RemoveFromEval()
    {
        if (_eval != null)
        {
            _eval.RemoveBlock(this.gameObject);
            _eval = null;
        }
    }
    
    public CodeBlock(CodeBlockType type, string value)
    {
        this.type = type;
        this.value = value;
    }

    private void Start()
    {
        SetText();
        _img = GetComponent<Image>();
        _initialPosition = transform.localPosition;
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

    public void ToggleGlitched()
    {
        glitched = !glitched;

        if (glitched)
        {
            _img.material = GameManager.instance.glitchedMaterial;
        }
        else
        {
            _img.material = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            transform.localPosition = _initialPosition;
        }
    }
}
