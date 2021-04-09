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

    private Vector3 _targetDirection;
    private float _axisMag;
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
        _axisMag = _targetDirection.sqrMagnitude;
    }

    // private float deadLock = 0.5f;
    public void FixedUpdate()
    {
        if (_targetDirection.x <= 0.2f && _targetDirection.z <= 0.2f && _targetDirection.x >= -0.2f &&
            _targetDirection.z >= -0.2f)
        {
            return;
        }

        Transform selfTransforms = transform;
        Vector3 position = selfTransforms.position;
        Vector3 targetDirection = _targetDirection + position;

        _rigidbody.AddForce(selfTransforms.forward * (speed * _axisMag), ForceMode.Acceleration);

        Quaternion qTo = Quaternion.LookRotation(targetDirection - position);
        qTo = Quaternion.Slerp(transform.rotation, qTo, 0.1f);
        _rigidbody.MoveRotation(qTo);
    }
}