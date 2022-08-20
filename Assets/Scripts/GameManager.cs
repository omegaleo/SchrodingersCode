using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Canvas _canvas;

    [Header("UI")] 
    [SerializeField] private GameObject _scanlines;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();

        if (PlayerPrefs.HasKey("ScanLines"))
        {
            var scanLinesActive = PlayerPrefs.GetString("ScanLines");
            if (bool.TryParse(scanLinesActive, out var active))
            {
                _scanlines.SetActive(active);
            }
        }

        if (_canvas.worldCamera == null)
        {
            _canvas.worldCamera = Camera.main;
        }
    }

    public void ToggleScanLines()
    {
        bool active = !_scanlines.activeSelf;
        _scanlines.SetActive(active);
        PlayerPrefs.SetString("ScanLines", active.ToString());
    }
}
