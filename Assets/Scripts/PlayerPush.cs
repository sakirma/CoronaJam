using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPush : MonoBehaviour
{
    [SerializeField] private float pushForce = 10.0f;
    [SerializeField] private float coolDownTime = 2.0f;
    [SerializeField] private Animator animator;
    
    
    private float _currentCooldown;

    private List<PlayerMovement> _playersToPush = new List<PlayerMovement>();
    private static readonly int Push = Animator.StringToHash("Push");

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
        if (_currentCooldown > 0 && _playersToPush.Count == 0)
            return;
        animator.SetTrigger(Push);

        foreach (PlayerMovement playerMovement in _playersToPush)
        {
            playerMovement.Push(pushForce, transform.position);
        }
        
        _currentCooldown = coolDownTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (!playerMovement)
            return;

        _playersToPush.Add(playerMovement);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (!playerMovement)
            return;

        _playersToPush.Remove(playerMovement);
    }
}