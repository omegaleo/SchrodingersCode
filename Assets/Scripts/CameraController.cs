using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool isShortsCamera;
    [SerializeField] private float speed = 1f;
    
    public static CameraController instance;

    private Vector3 targetPosition;
    
    void Awake()
    {
        if (instance == null && !isShortsCamera)
        {
            instance = this;
        }
    }
    
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

        if (camera.enabled)
        {
            GameManager.instance.SetCamera(camera);
        }
    }

    private void Update()
    {
        if (!GameManager.instance.recordForShorts)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    public void SetPosition(Vector2 position)
    {
        targetPosition = new Vector3(position.x, position.y, transform.position.z);
    }
}
