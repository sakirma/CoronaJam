using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float speed = 100.0f;

    private GameObject _directionSphere = null;

    private Vector3 _targetDirection;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _directionSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _directionSphere.GetComponent<MeshRenderer>().material.color = Color.red;
        _directionSphere.GetComponent<SphereCollider>().isTrigger = true;
    }

    public void Update()
    {
        Vector3 directionInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 currentPosition = transform.position;
        
        Vector3 targetDirection = directionInput + currentPosition;
        _directionSphere.transform.position = targetDirection;
        
        // _targetDirection = Vector3.;
    }

    public void FixedUpdate()
    {

        // _rigidbody.AddRelativeForce(targetVelocity * speed);
        // _rigidbody.AddRelativeTorque(torqueVelocity, ForceMode.VelocityChange);

    }
}
