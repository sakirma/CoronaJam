using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPush : MonoBehaviour
{
    [SerializeField] private float pushDistance = 2.0f;
    [SerializeField] private float pushForce = 10.0f;
    
    // Input System
    // ReSharper disable once UnusedMember.Global
    public void PushInput(InputAction.CallbackContext context)
    {
        Transform curTransform = transform;
        Vector3 forward = curTransform.forward;
        Vector3 currentPos = curTransform.position;
        
        Debug.DrawRay(currentPos, forward, Color.red, 2.0f);
        if (Physics.Raycast(currentPos, forward, out RaycastHit hitInfo, pushDistance))
        {
            hitInfo.collider.GetComponent<PlayerMovement>().Push(pushForce, transform.position);
        }
    }
}
