using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector2 _movement;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private static readonly int Right = Animator.StringToHash(MoveRight);
    private static readonly int Left = Animator.StringToHash(MoveLeft);
    private static readonly int Up = Animator.StringToHash(MoveUp);
    private static readonly int Down = Animator.StringToHash(MoveDown);

    private const string MoveRight = "MoveRight"; 
    private const string MoveLeft = "MoveLeft"; 
    private const string MoveUp = "MovingUp"; 
    private const string MoveDown = "MovingDown"; 
    private const string HoldingBox = "HoldingBox";

    private bool _holding;
    private static readonly int Box = Animator.StringToHash(HoldingBox);

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _rb2d.MovePosition(_rb2d.position +
                           _movement * moveSpeed * Time.deltaTime);

        if (_movement.x > 0f)
        {
            _animator.SetBool(Right, true);
            _animator.SetBool(Left, false);
            _animator.SetBool(Up, false);
            _animator.SetBool(Down, false);
        }
        else if (_movement.x < 0f)
        {
            _animator.SetBool(Right, false);
            _animator.SetBool(Left, true);
            _animator.SetBool(Up, false);
            _animator.SetBool(Down, false);
        }
        else if (_movement.y > 0f)
        {
            _animator.SetBool(Right, false);
            _animator.SetBool(Left, false);
            _animator.SetBool(Up, true);
            _animator.SetBool(Down, false);
        }
        else if (_movement.y < 0f)
        {
            _animator.SetBool(Right, false);
            _animator.SetBool(Left, false);
            _animator.SetBool(Up, false);
            _animator.SetBool(Down, true);
        }
        else
        {
            _animator.SetBool(Right, false);
            _animator.SetBool(Left, false);
            _animator.SetBool(Up, false);
            _animator.SetBool(Down, false);
        }
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        _movement = value.ReadValue<Vector2>();
    }

    public void InteractDown(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _holding = !_holding;
            _animator.SetBool(Box, _holding);
        }
    }
}
