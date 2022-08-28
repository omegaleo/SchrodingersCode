using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandling : MonoBehaviour
{
    [Header("Configuration")] 
    [SerializeField] private OptionsInputType type;

    private void Start()
    {
        switch (type)
        {
            case OptionsInputType.ScanLines:
                GetComponent<Toggle>().isOn = GameManager.instance.ScanLinesOn;
                break;
        }
    }

    public void HandleChange()
    {
        switch (type)
        {
            case OptionsInputType.ScanLines:
                GameManager.instance.ToggleScanLines(GetComponent<Toggle>().isOn);
                break;
        }
    }
}
