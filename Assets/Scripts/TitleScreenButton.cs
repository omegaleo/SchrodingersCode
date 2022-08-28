using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class TitleScreenButton : Button, IMouseCaptureEvent
{
    private TMP_Text _text;

    private void OnMouseOver()
    {
        _text.color = colors.highlightedColor;
    }

    private void OnMouseExit()
    {
        _text.color = colors.normalColor;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        _text.color = colors.selectedColor;
        base.OnSelect(eventData);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        _text.color = colors.normalColor;
        base.OnDeselect(eventData);
    }

    protected override void Start()
    {
        _text = transform.GetChild(0).GetComponent<TMP_Text>();
        _text.color = colors.normalColor;
        base.Start();
    }

    public void StartGame()
    {
        SFXManager.instance.PlaySound(SFXType.ButtonClick);
        SceneManager.LoadScene(1);
    }

    public void Trophies()
    {
        SFXManager.instance.PlaySound(SFXType.ButtonClick);
        ScreenHandler.instance.Trophies();
    }

    public void Options()
    {
        SFXManager.instance.PlaySound(SFXType.ButtonClick);
        ScreenHandler.instance.Options();
    }

    public void ExitGame()
    {
        SFXManager.instance.PlaySound(SFXType.ButtonClick);
        Application.Quit();
    }
}
