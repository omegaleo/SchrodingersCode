using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool isShortsCamera;
    
    void Start()
    {
        var camera = GetComponent<Camera>();
        
        if (GameManager.instance.recordForShorts)
        {
            camera.enabled = isShortsCamera;
        }
        else
        {
            camera.enabled = !isShortsCamera;
        }
    }
}
