using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageCollider : MonoBehaviour
{
    [SerializeField] private List<string> messages = new List<string>();

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            messages.ForEach(msg => MessageQueueHandler.instance.AddToQueue(msg));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            messages.ForEach(msg => MessageQueueHandler.instance.AddToQueue(msg));
        }
    }

    private void Start()
    {
        MessageQueueHandler.instance.OnMessagesFinished.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
}
