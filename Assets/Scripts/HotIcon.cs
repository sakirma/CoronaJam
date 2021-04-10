using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotIcon : MonoBehaviour
{
    [SerializeField] private PlayerTemperature playerTemp;
    private ParticleSystem sys;

    private void Start()
    {
        sys = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (playerTemp.ClosestToCampfire() != sys.isPlaying)
        {
            if (playerTemp.ClosestToCampfire())
            {
                sys.Play();
            }
            else
            {
                sys.Stop();
            }
        } 
    }
}
