using System;
using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenHandler : MonoBehaviour
{
    [ColoredHeader("Panels", textColor: "#27B391")]
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject trophies;
    [SerializeField] private GameObject options;

    [ColoredHeader("First Selected for each panel", textColor: "#27B391")]
    [SerializeField] private GameObject titleScreenFS;
    [SerializeField] private GameObject trophiesFS;
    [SerializeField] private GameObject optionsFS;
    
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

        if (active)
        {
            EventSystem.current.SetSelectedGameObject(trophiesFS);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(titleScreenFS);
        }
        
        titleScreen.SetActive(!active);
    }

    public void Options(bool active = true)
    {
        options.SetActive(active);
        
        if (active)
        {
            EventSystem.current.SetSelectedGameObject(optionsFS);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(titleScreenFS);
        }
        
        titleScreen.SetActive(!active);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            var selected = titleScreenFS;

            if (options.activeSelf)
            {
                selected = optionsFS;
            }

            if (trophies.activeSelf)
            {
                selected = trophiesFS;
            }
            
            EventSystem.current.SetSelectedGameObject(selected);
        }
    }
}
