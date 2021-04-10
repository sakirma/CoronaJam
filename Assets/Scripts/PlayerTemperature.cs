using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Events;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTemperature : MonoBehaviour
{
    // gets lowed if player not closest to fire, how much lower determined by debuff
    [SerializeField] private float _temperature;
    public float Temperature { get => _temperature; }
    [SerializeField] private float _health;
    // determined by range to campfire
    [SerializeField] private float _debuff;
    public bool ClosestToCampfire() { return _debuff > 0f;}

    private Vector3 _prevPosition;
    private readonly UnityEvent<PositionData> _onPositionChanged;
    private readonly UnityEvent<string> _onPlayerDied;

    public bool GameStarted;
    
    public void OnPlayerPositionChanged( UnityAction<PositionData> value )
    {
        _onPositionChanged.AddListener( value );
    }
    
    public void OnPlayerDied( UnityAction<string> value )
    {
        _onPlayerDied.AddListener( value );
    }

    public void KillPlayer()
    {
        var name = GetComponent<Player>().Name;
        _onPlayerDied.Invoke(name);
    }
    
    public PlayerTemperature()
    {
        _onPositionChanged ??= new UnityEvent<PositionData>();
        _onPlayerDied ??= new UnityEvent<string>();
        
        _temperature = 40;
    }
    
    void Start()
    {
        FindObjectOfType<TemperatureHandlerBase>().OnPlayerRankingChanged(PlayerChanged);
    }
    
    private void Update()
    {
        if (!GameStarted) return;
        
        _temperature += _debuff * Time.deltaTime;
        var player = GetComponent<Player>();
        
        Vector3 currentPosition = transform.position;

        if (_prevPosition == currentPosition) return;
        
        PositionData position = new PositionData
        {
            Position = currentPosition,
            PlayerName = player.Name
        };
            
        _prevPosition = currentPosition;
        _onPositionChanged.Invoke(position);

        if (_temperature > 0 || !player.Alive) return;
        _onPlayerDied.Invoke(player.Name);
    }

    public void ResetValues()
    {
        _health = 100;
        _debuff = 0;
        _temperature = 160;
    }
    
    void PlayerChanged( Dictionary<string, float> changeDictionary )
    {
        _debuff = changeDictionary[GetComponent<Player>().Name];
    }
}
