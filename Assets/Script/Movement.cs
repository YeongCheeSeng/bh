using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{

    Rigidbody2D _rigidbody2D;
    Collider2D _collider2D;
    //protected WeaponHandler _weaponHandler;

    public float MovementSpeed = 10;
    protected const float m_MovementSmoothing = 0.05f;

    protected Vector2 _inputDirection;

    Vector2 m_Velocity = Vector2.zero;
    protected Vector2 _targetVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        //_weaponHandler = GetComponent<WeaponHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRotation();
    }

    protected virtual void HandleRotation()
    {
        if (_inputDirection == Vector2.zero)
            return;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, upwards: (Vector3)_targetVelocity);
    }

    protected virtual void HandleMovement()
    {
        if (_collider2D == null || _rigidbody2D == null)
            return;

        Vector2 targetVelocity = Vector2.zero;
        targetVelocity = new Vector2(x: _inputDirection.x * MovementSpeed, y: _inputDirection.y * MovementSpeed);

        _rigidbody2D.velocity = Vector2.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        transform.rotation = Quaternion.LookRotation(Vector3.forward, upwards: (Vector3)targetVelocity);

        _targetVelocity = targetVelocity;
    }

    protected virtual void HandleInput()
    {

    }
}
