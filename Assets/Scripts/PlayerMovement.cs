using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float speed = 100;
    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float maxSpeed = 10.0f;
    
    private Vector3 _targetDirection;
    private float _axisMagnitude;
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Input System
    // ReSharper disable once UnusedMember.Global
    public void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 directionInput = context.ReadValue<Vector2>();

        _targetDirection = new Vector3(directionInput.x, 0, directionInput.y) * 2;
        _axisMagnitude = _targetDirection.sqrMagnitude;
    }

    public void FixedUpdate()
    {
        if (_targetDirection.x <= 0.2f && _targetDirection.z <= 0.2f && _targetDirection.x >= -0.2f &&
            _targetDirection.z >= -0.2f)
        {
            return;
        }

        Transform selfTransforms = transform;

        if (_rigidbody.velocity.sqrMagnitude < maxSpeed)
        {
            _rigidbody.AddForce(selfTransforms.forward * (speed * _axisMagnitude), ForceMode.VelocityChange);
        }
        
        Vector3 position = selfTransforms.position;
        Vector3 targetDirection = _targetDirection + position;
        Quaternion qTo = Quaternion.LookRotation(targetDirection - position);
        qTo = Quaternion.Slerp(transform.rotation, qTo, rotationSpeed);
        _rigidbody.MoveRotation(qTo);
    }

    public void Push(float pushForce, Vector3 fromPosition)
    {
        Vector3 pushDirection = transform.position - fromPosition;
        
        _rigidbody.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
    }
}