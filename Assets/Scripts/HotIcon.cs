using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotIcon : MonoBehaviour
{
    [SerializeField] private PlayerTemperature playerTemp;
    private MeshRenderer mesh;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        mesh.enabled = playerTemp.ClosestToCampfire();
    }
}
