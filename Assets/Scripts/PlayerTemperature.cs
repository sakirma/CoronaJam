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
    [SerializeField] private float _health;
    [SerializeField] private float _debuff;

    private Vector3 _prevPosition;
    private readonly UnityEvent<PositionData> _onPositionChanged;
    private readonly UnityEvent<string> _onPlayerDied;

    public bool GameStarted = false;
    
    public void OnPlayerPositionChanged( UnityAction<PositionData> value )
    {
        _onPositionChanged.AddListener( value );
    }
    
    public void OnPlayerDied( UnityAction<string> value )
    {
        _onPlayerDied.AddListener( value );
    }
    
    public PlayerTemperature()
    {
        _onPositionChanged ??= new UnityEvent<PositionData>();
        _onPlayerDied ??= new UnityEvent<string>();

        _health = 100;
    }
    
    void Start()
    {
        FindObjectOfType<TemperatureHandlerBase>().OnPlayerRankingChanged(PlayerChanged);
        StartCoroutine(Damage());
    }
    
    private void Update()
    {
        if (!GameStarted) return;
        
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
    
    IEnumerator Damage() 
    {
        while(true) 
        {
            if (_debuff < 0) { _health += _debuff; }
            
            if (_health < 1)
            {
                var name = GetComponent<Player>().Name;
                _onPlayerDied.Invoke(name);
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void ResetValues()
    {
        _health = 100;
        _debuff = 0;
        _temperature = 0;
    }
    
    void PlayerChanged( Dictionary<string, float> changeDictionary )
    {
        _debuff = changeDictionary[GetComponent<Player>().Name];
    }
}
