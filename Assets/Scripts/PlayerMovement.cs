using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float speed = 100.0f;

    private GameObject _directionSphere = null;

    private Vector3 _targetDirection;
    private Vector3 _lerpValue;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _directionSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _directionSphere.GetComponent<MeshRenderer>().material.color = Color.red;
        _directionSphere.GetComponent<SphereCollider>().isTrigger = true;
    }
    
    // Input System
    // ReSharper disable once UnusedMember.Global
    public void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 directionInput = context.ReadValue<Vector2>();
        Vector3 currentPosition = transform.position;

        _targetDirection = new Vector3(directionInput.x, 0, directionInput.y) + currentPosition;
        _directionSphere.transform.position = _targetDirection;
    }

    public void Update()
    {
        // _lerpValue = Vector3.Lerp();
    }

    public void FixedUpdate()
    {
        // _rigidbody.AddRelativeForce(targetVelocity * speed);
        // _rigidbody.AddRelativeTorque(torqueVelocity, ForceMode.VelocityChange);
    }
}