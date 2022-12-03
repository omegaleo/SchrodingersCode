using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Runtime.Models;
using TMPro;
using UnityEngine;

public class VersionLabel  : InstancedBehavior<VersionLabel>
{
    [SerializeField] private Color color;
    
    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        string version = Application.version;

        if (GameManager.instance.IsBeta)
        {
            version += "-beta";
        }
        
        if (GameManager.instance.IsDemo)
        {
            version += " Demo";
        }

        /*if (GameManager.instance.IsEarlyAccess && !GameManager.instance.IsDemo)
        {
            version += " Early Access";
        }*/
        
        GetComponent<TMP_Text>().text = version;
    }
}
