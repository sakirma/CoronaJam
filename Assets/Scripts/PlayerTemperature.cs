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
        FindObjectOfType<TemperatureHandlerBase>().OnPlayerRankingChanged(PlayerChanged);
    }

    
    private void Update()
    {
        _temperature += _debuff * Time.deltaTime;
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
