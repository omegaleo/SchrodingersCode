using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Configuration")] 
    [SerializeField] private List<AudioClipAssociation> clips = new List<AudioClipAssociation>();

    public float Volume => PlayerPrefs.HasKey(_prefIdentifier) ? PlayerPrefs.GetFloat(_prefIdentifier) : 0.5f;

    public static SFXManager instance;

    private readonly string _prefIdentifier = "SfxVolume";
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound(SFXType type)
    {
        var clip = clips.FirstOrDefault(x => x.type == type)?.clip;

        if (clip != null)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = Volume;
            audioSource.clip = clip;
            audioSource.priority = 256;
            audioSource.Play();
            StartCoroutine(DestroyOnFinish(audioSource));
        }
    }

    public void SetVolume(float volume)
    {
        var normalizedVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(_prefIdentifier, normalizedVolume);
    }
    
    private IEnumerator DestroyOnFinish(AudioSource source)
    {
        while (source.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(source);
    }
}
