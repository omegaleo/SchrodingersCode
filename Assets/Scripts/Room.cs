using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !GameManager.instance.recordForShorts)
        {
            var position = transform.position;
            CameraController.instance.SetPosition(new Vector2(position.x, position.y));
        }
    }
}
