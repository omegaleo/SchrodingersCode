using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("Configuration")] 
    [SerializeField] private List<AudioClipAssociation> clips = new List<AudioClipAssociation>();

    [Range(0f,1f)]
    [SerializeField] private float volume = 0.5f;

    public static SFXManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            var volumeString = PlayerPrefs.GetString("SfxVolume");
            if (float.TryParse(volumeString, out var value))
            {
                volume = value;
            }
        }
    }

    public void PlaySound(SFXType type)
    {
        var clip = clips.FirstOrDefault(x => x.type == type)?.clip;

        if (clip != null)
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = volume;
            audioSource.clip = clip;
            audioSource.priority = 256;
            audioSource.Play();
            StartCoroutine(DestroyOnFinish(audioSource));
        }
    }

    private IEnumerator DestroyOnFinish(AudioSource source)
    {
        while (!source.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        Destroy(source);
    }
}
