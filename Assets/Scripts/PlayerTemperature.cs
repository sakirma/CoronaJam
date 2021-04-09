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

    private Vector3 _prevPosition;
    private UnityEvent<PositionData> _onPositionChanged;
    

    public void OnPlayerPositionChanged( UnityAction<PositionData> value )
    {
        _onPositionChanged.AddListener( value );
    }
    
    void Start()
    {
        _onPositionChanged ??= new UnityEvent<PositionData>();
        FindObjectOfType<TemperatureHandler>().OnPlayerRankingChanged(PlayerChanged);
    }

    // Update is called once per frame
    private void Update()
    {
        _temperature += _debuff * Time.deltaTime;
        Vector3 currentPosition = transform.position;
        
        if (_prevPosition != currentPosition)
        {
            PositionData position = new PositionData
            {
                Position = currentPosition,
                PlayerName = GetComponent<Player>().Name
            };
            
            _prevPosition = currentPosition;
            _onPositionChanged.Invoke(position);
        }
    }
    
    void PlayerChanged( Dictionary<string, float> changeDictionary )
    {
        _debuff = changeDictionary[ GetComponent<Player>().Name ];
    }
}
