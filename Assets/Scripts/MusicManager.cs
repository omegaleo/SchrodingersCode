using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private readonly string _prefIdentifier = "MusicVolume";
    private AudioSource _source;

    public bool loaded = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.volume = Volume;

        loaded = true;
    }

    public float Volume => PlayerPrefs.HasKey(_prefIdentifier) ? PlayerPrefs.GetFloat(_prefIdentifier) : 0.5f;
    
    public void SetVolume(float volume)
    {
        var normalizedVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(_prefIdentifier, normalizedVolume);
        _source.volume = normalizedVolume;
    }
}
