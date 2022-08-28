using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrophyDisplay : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private string identifier;

    [SerializeField] private string displayName;

    [SerializeField] private Color disabledColor;

    [SerializeField] private Sprite hiddenSprite;
    [SerializeField] private Sprite showSprite;
    
    [Header("UI")] 
    [SerializeField] private TMP_Text text;

    [SerializeField] private Image img;

    public bool IsClaimed()
    {
        if (PlayerPrefs.HasKey(identifier))
        {
            var claimedString = PlayerPrefs.GetString(identifier);
            if (bool.TryParse(claimedString, out var claimed))
            {
                return claimed;
            }
        }

        return false;
    }
    
    private void Start()
    {
        if (IsClaimed())
        {
            text.text = displayName;
            img.color = new Color(1f, 1f, 1f, 1f);
            img.sprite = showSprite;
        }
        else
        {
            text.text = "???";
            img.color = disabledColor;
            img.sprite = hiddenSprite;
        }
    }
}
