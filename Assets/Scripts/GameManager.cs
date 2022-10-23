using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private Canvas _canvas;

    [Header("UI")] [SerializeField] private GameObject _scanlines;
    [SerializeField] private GameObject tutorial;

    [Header("Configuration")] public Material glitchedMaterial;

    public TileBase activatedCircuitTile;
    public TileBase deActivatedCircuitTile;
    public TileBase wallTile;
    public TileBase groundTile;

    public static GameManager instance;

    public bool IsDemo
    {
        get
        {
#if DEMO
            return true;
#else
            return false;
#endif
        }
    }

    public bool IsBeta
    {
        get
        {
#if BETA
            return true;
#else
            return false;
#endif
        }
    }

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

    public bool TutorialOpen => tutorial.activeSelf;

    public void CloseTutorial()
    {
        tutorial.SetActive(false);
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

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            var fullscreenActive = PlayerPrefs.GetString("Fullscreen");
            if (bool.TryParse(fullscreenActive, out var active))
            {
                Screen.fullScreen = active;
            }
        }

        if (_canvas.worldCamera == null)
        {
            _canvas.worldCamera = Camera.main;
        }

        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        if (arg1.buildIndex == 1)
        {
            if (!PlayerPrefs.HasKey("Tutorial"))
            {
                tutorial.SetActive(true);
            }
        }

        if (_canvas.worldCamera == null)
        {
            _canvas.worldCamera = Camera.main;
        }
    }

    public void SetCamera(Camera camera)
    {
        _canvas.worldCamera = camera;
    }

    public bool ScanLinesOn => _scanlines.activeSelf;

    /// <summary>
    /// Called by Editor
    /// </summary>
    public void ToggleScanLines()
    {
        bool active = !_scanlines.activeSelf;
        _scanlines.SetActive(active);
        PlayerPrefs.SetString("ScanLines", active.ToString());
    }

    public void ToggleScanLines(bool value)
    {
        _scanlines.SetActive(value);
        PlayerPrefs.SetString("ScanLines", value.ToString());
    }
}