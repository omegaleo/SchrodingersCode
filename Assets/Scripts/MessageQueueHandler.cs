using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MessageQueueHandler : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float hideY;
    [SerializeField] private float showY;
    [SerializeField] private float speed = 1f;
    [SerializeField] private List<string> messages = new List<string>();

    [Header("UI")] 
    [SerializeField] private TMP_Text text;

    public bool show;

    public static MessageQueueHandler instance;

    public UnityEvent OnMessagesFinished;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddToQueue(string message)
    {
        messages.Add(message);

        if (!show)
        {
            show = true;
            text.text = messages.FirstOrDefault();
        }
    }

    public void NextMessage()
    {
        messages.RemoveAt(0);
        
        if (messages.Any())
        {
            text.text = messages.FirstOrDefault();
        }
        else
        {
            show = false;

            try
            {
                OnMessagesFinished.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    private void Update()
    {
        if (show)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, showY, 0f), speed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, hideY, 0f), speed * Time.deltaTime);
        }
    }

    private void Start()
    {
        messages = new List<string>();

        SetPositions();
    }

    private void SetPositions()
    {
        var bottomOfScreen = 0f - (Screen.height / 2f);

        var rect = GetComponent<RectTransform>();
        var offset = rect.rect.height / 2f;

        showY = bottomOfScreen + offset;
        hideY = bottomOfScreen - offset;
        transform.localPosition = new Vector3(0f, hideY, 0f);
    }

    private void OnRectTransformDimensionsChange()
    {
        SetPositions();
    }
}
