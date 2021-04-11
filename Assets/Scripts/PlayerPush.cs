using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPush : MonoBehaviour
{
    [SerializeField] private float pushForce = 10.0f;
    [SerializeField] private float coolDownTime = 2.0f;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip[] _audioClips;
    
    
    private AudioSource _audioSource;
    private float _currentCooldown;

    private List<PlayerMovement> _playersToPush = new List<PlayerMovement>();
    private static readonly int Push = Animator.StringToHash("Push");

    private void Awake()
    {
        _currentCooldown = coolDownTime;
        _audioSource = GetComponent<AudioSource>();
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
        int index = Random.Range(0, _audioClips.Length);
        _audioSource.clip = _audioClips[index];
        _audioSource.Play();


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