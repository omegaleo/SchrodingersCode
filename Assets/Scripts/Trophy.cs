using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Trophy : MonoBehaviour
{
    [Header("Configuration")] 
    [SerializeField] private string prefName;

    [SerializeField] private float frameInterval = 0.145f;
    [SerializeField] private List<Sprite> frames;
    [SerializeField] private Sprite claimedFrame;
    
    private int _frame = 0;
    private Image _img;

    public void Claim()
    {
        PlayerPrefs.SetString(prefName, true.ToString());
        
        // Trigger Animation
        
        // Show Message
    }

    public bool IsClaimed()
    {
        if (PlayerPrefs.HasKey(prefName))
        {
            var claimedString = PlayerPrefs.GetString(prefName);
            if (bool.TryParse(claimedString, out var claimed))
            {
                return claimed;
            }
        }

        return false;
    }

    private void Start()
    {
        _img = GetComponent<Image>();

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (!IsClaimed() && frames.Any())
        {
            _img.sprite = frames[_frame];

            _frame++;

            if (_frame >= frames.Count)
            {
                _frame = 0;
            }

            yield return new WaitForSeconds(frameInterval);
        }

        yield return new WaitForSeconds(0.1f);
        _img.sprite = claimedFrame;
    }
}
