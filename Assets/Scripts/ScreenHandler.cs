using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenHandler : MonoBehaviour
{
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject trophies;
    [SerializeField] private GameObject options;

    public static ScreenHandler instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    public void Trophies(bool active = true)
    {
        trophies.SetActive(active);
        titleScreen.SetActive(!active);
    }

    public void Options(bool active = true)
    {
        options.SetActive(active);
        titleScreen.SetActive(!active);
    }

}
