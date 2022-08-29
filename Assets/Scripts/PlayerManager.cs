using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform boxHolder;
    [SerializeField] private Transform ground;
    [SerializeField] private List<DirectionPlacementAnchors> anchors;
    private Vector2 _movement;

    private static readonly int Right = Animator.StringToHash(MoveRight);
    private static readonly int Left = Animator.StringToHash(MoveLeft);
    private static readonly int Up = Animator.StringToHash(MoveUp);
    private static readonly int Down = Animator.StringToHash(MoveDown);
    private static readonly int Box = Animator.StringToHash(HoldingBox);

    private const string MoveRight = "MoveRight";
    private const string MoveLeft = "MoveLeft";
    private const string MoveUp = "MovingUp";
    private const string MoveDown = "MovingDown";
    private const string HoldingBox = "HoldingBox";

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private bool _holding;
    private GameObject _colliding;
    private GameObject _holdingBox;
    private LookingDirection _direction;
    private Trophy _trophy;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("CodeBlock"))
        {
            _colliding = col.gameObject;
        }

        if (col.gameObject.CompareTag("Trophy"))
        {
            _trophy = col.gameObject.GetComponent<Trophy>();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("CodeBlock"))
        {
            _colliding = null;
        }
        
        if (other.gameObject.CompareTag("Trophy"))
        {
            _trophy = null;
        }
    }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _rb2d.MovePosition(_rb2d.position +
                           _movement * moveSpeed * Time.deltaTime);

        if (_holding && _holdingBox != null)
        {
            var position = transform.localPosition;
            var anchor = anchors.FirstOrDefault(x => x.direction == LookingDirection.Up);
            
            _holdingBox.transform.localPosition = new Vector3(position.x + anchor.offset.x, 
                position.y + anchor.offset.y);
        }
        
        if (_movement != Vector2.zero)
        {
            if (_movement.x > 0f)
            {
                _animator.SetBool(Right, true);
                _animator.SetBool(Left, false);
                _animator.SetBool(Up, false);
                _animator.SetBool(Down, false);
                _direction = LookingDirection.Right;
            }
            else if (_movement.x < 0f)
            {
                _animator.SetBool(Right, false);
                _animator.SetBool(Left, true);
                _animator.SetBool(Up, false);
                _animator.SetBool(Down, false);
                _direction = LookingDirection.Left;
            }
            else if (_movement.y > 0f)
            {
                _animator.SetBool(Right, false);
                _animator.SetBool(Left, false);
                _animator.SetBool(Up, true);
                _animator.SetBool(Down, false);
                _direction = LookingDirection.Up;
            }
            else if (_movement.y < 0f)
            {
                _animator.SetBool(Right, false);
                _animator.SetBool(Left, false);
                _animator.SetBool(Up, false);
                _animator.SetBool(Down, true);
                _direction = LookingDirection.Down;
            }
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
        if (MessageQueueHandler.instance.show || GameManager.instance.TutorialOpen)
        {
            _movement = Vector2.zero;
            return;
        }
        
        _movement = value.ReadValue<Vector2>();
    }

    public void MenuDown(InputAction.CallbackContext ctx)
    {
        // Return to title screen for now
        SceneManager.LoadScene(0);
    }
    
    public void InteractDown(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (GameManager.instance.TutorialOpen)
            {
                PlayerPrefs.SetString("Tutorial", true.ToString());
                GameManager.instance.CloseTutorial();
            }
            
            if (CanPickupBlock() || CanDropBlock())
            {
                _holding = !_holding;
                _animator.SetBool(Box, _holding);

                if (_holding)
                {
                    _holdingBox = _colliding;
                    _holdingBox.transform.SetParent(boxHolder);
                    _holdingBox.GetComponent<CodeBlock>().RemoveFromEval();
                    var anchor = anchors.FirstOrDefault(x => x.direction == LookingDirection.Up);
                    _holdingBox.transform.position = new Vector3(0f, anchor.offset.y, 0f);
                    _holdingBox.GetComponent<BoxCollider2D>().enabled = false;
                    SFXManager.instance.PlaySound(SFXType.Pickup);
                }
                else
                {
                    var position = transform.localPosition;
                    var placePosition = Vector3.zero;
                    var anchor = anchors.FirstOrDefault(x => x.direction == _direction);
                    placePosition = anchor.offset;

                    _holdingBox.transform.localPosition = new Vector3(position.x + placePosition.x, 
                        position.y + placePosition.y);
                    _holdingBox.transform.SetParent(ground);
                    _holdingBox.GetComponent<BoxCollider2D>().enabled = true;
                    _holdingBox = null;
                    SFXManager.instance.PlaySound(SFXType.Drop);
                }
            }

            if (_trophy != null && !_trophy.IsClaimed())
            {
                _trophy.Claim();
                SFXManager.instance.PlaySound(SFXType.Trophy);
                _trophy = null;
            }
        }
    }

    private bool CanDropBlock()
    {
        return (_holding && _holdingBox != null);
    }

    private bool CanPickupBlock()
    {
        return (!_holding && _colliding != null && !_colliding.GetComponent<CodeBlock>().locked);
    }
}
