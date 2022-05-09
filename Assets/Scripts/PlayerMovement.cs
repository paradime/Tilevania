using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _moveInput;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private CapsuleCollider2D _collider;

    private static readonly string IS_RUNNING_ANIMATOR_FIELD = "IsRunning";
    private static readonly string GROUND_LAYER = "Ground";

    [SerializeField]
    private float baseSpeed = 10f;
    [SerializeField]
    private float jumpSpeed = 5f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        Run();
        FlipSprite();
    }

    private bool HasHorizontalVelocity() => Mathf.Abs(_rigidbody.velocity.x) > Mathf.Epsilon;
    private void FlipSprite()
    {
        if (!HasHorizontalVelocity())
        {
            return;
        }
        transform.localScale = new Vector2(Mathf.Sign(_rigidbody.velocity.x), 1);
    }

    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        Debug.Log(_moveInput);
    }

    void Run()
    {
        var playerVelocity = new Vector2(_moveInput.x * baseSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity = playerVelocity;

        _animator.SetBool(IS_RUNNING_ANIMATOR_FIELD, HasHorizontalVelocity());
    }

    void OnJump(InputValue value)
    {
        if (!_collider.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER)))
        {
            return;
        }
        if (value.isPressed)
        {
            _rigidbody.velocity += new Vector2(0, jumpSpeed);
        }
    }
}
