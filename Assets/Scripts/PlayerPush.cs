using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPush : MonoBehaviour
{
    [SerializeField] private float pushDistance = 2.0f;
    [SerializeField] private float pushForce = 10.0f;
    [SerializeField] private float coolDownTime = 2.0f;

    private float _currentCooldown;

    private void Awake()
    {
        _currentCooldown = coolDownTime;
    }

    private void Update()
    {
        if (_currentCooldown > 0)
            _currentCooldown -= Time.deltaTime;
    }

    // Input System
    // ReSharper disable once UnusedMember.Global
    public void PushInput(InputAction.CallbackContext context)
    {
        if (_currentCooldown > 0)
            return;

        Transform curTransform = transform;
        Vector3 forward = curTransform.forward;
        Vector3 currentPos = curTransform.position;

        Debug.DrawRay(currentPos, forward, Color.red, 2.0f);
        
        if (!Physics.Raycast(currentPos, forward, out RaycastHit hitInfo, pushDistance)) return;
        
        hitInfo.collider.GetComponent<PlayerMovement>().Push(pushForce, transform.position);
        _currentCooldown = coolDownTime;
    }
}