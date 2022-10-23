using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !DISABLESTEAMWORKS
using Steamworks;
#endif

public class SteamController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if !DISABLESTEAMWORKS
        if(SteamManager.Initialized) {
            // Do Steam related stuff here
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
