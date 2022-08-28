using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class CloseButton : Button, IMouseCaptureEvent
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
        _text = GetComponent<TMP_Text>();
        _text.color = colors.normalColor;
        base.Start();
    }

    public void CloseTrophies()
    {
        _text.color = colors.normalColor;
        SFXManager.instance.PlaySound(SFXType.ButtonClick);
        ScreenHandler.instance.Trophies(false);
    }
    
    public void CloseOptions()
    {
        _text.color = colors.normalColor;
        SFXManager.instance.PlaySound(SFXType.ButtonClick);
        ScreenHandler.instance.Options(false);
    }
}
