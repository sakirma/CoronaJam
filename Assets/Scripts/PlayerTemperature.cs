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
    private Gradient _gradient;

    private Vector3 _prevPosition;
    private readonly UnityEvent<PositionData> _onPositionChanged;
    private readonly UnityEvent<string> _onPlayerDied;
    
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
                Debug.Log(name + " Died!");
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    
    void PlayerChanged( Dictionary<string, float> changeDictionary )
    {
        _debuff = changeDictionary[GetComponent<Player>().Name];
    }
}
