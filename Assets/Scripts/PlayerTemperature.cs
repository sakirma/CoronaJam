using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Events;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTemperature : MonoBehaviour
{
    [SerializeField] private float _temperature;
    [SerializeField] private float _debuff;
    private Gradient _gradient;

    private Vector3 _prevPosition;
    private UnityEvent<PositionData> _onPositionChanged;
    

    public void OnPlayerPositionChanged( UnityAction<PositionData> value )
    {
        _onPositionChanged.AddListener( value );
    }

    public PlayerTemperature()
    {
        _onPositionChanged ??= new UnityEvent<PositionData>();
    }
    
    void Start()
    {
        _gradient = new Gradient();
        
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        var colorKey = new GradientColorKey[3];
        colorKey[0].color = Color.blue;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.magenta;
        colorKey[1].time = 0.25f;
        colorKey[2].color = Color.red;
        colorKey[2].time = 0.5f;

        var alphaKey = new GradientAlphaKey[2];
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;


        _gradient.SetKeys(colorKey, alphaKey);
        GetComponent<MeshRenderer>().material.color = _gradient.Evaluate(0);
        FindObjectOfType<TemperatureHandler>().OnPlayerRankingChanged(PlayerChanged);
    }

    
    private void Update()
    {
        _temperature += _debuff * Time.deltaTime;
        GetComponent<MeshRenderer>().material.color = _gradient.Evaluate(_temperature / 100);
        Vector3 currentPosition = transform.position;

        if (_prevPosition == currentPosition) return;
        
        PositionData position = new PositionData
        {
            Position = currentPosition,
            PlayerName = GetComponent<Player>().Name
        };
            
        _prevPosition = currentPosition;
        _onPositionChanged.Invoke(position);
    }
    
    void PlayerChanged( Dictionary<string, float> changeDictionary )
    {
        _debuff = changeDictionary[GetComponent<Player>().Name];
    }
}
