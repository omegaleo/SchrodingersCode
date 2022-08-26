using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private Canvas _canvas;

    [Header("UI")] 
    [SerializeField] private GameObject _scanlines;

    [Header("Configuration")] 
    public Material glitchedMaterial;

    public TileBase activatedCircuitTile;
    public TileBase wallTile;
    public TileBase groundTile;
    
    public static GameManager instance;

    [Header("Recording Options")] 
    public bool recordForShorts; // Will toggle on the Player Camera instead of the current camera
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

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
