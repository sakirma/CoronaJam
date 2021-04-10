using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Waypoint : MonoBehaviour
{
    public Vector3 From => FromObject.transform.position;
    public Vector3 To => ToObject.transform.position;

    public GameObject FromObject;
    public GameObject ToObject;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(From, To);
    } 
}