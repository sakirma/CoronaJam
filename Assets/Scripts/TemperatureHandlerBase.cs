using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.Events;

public abstract class TemperatureHandlerBase : MonoBehaviour
{
    protected Dictionary<string, float> _debuffs;
    protected Dictionary<string, Vector3> _positions;
    protected Dictionary<string, float> _distances;
    
    protected PlayerChangeEvent _onPlayerChanged;
    protected UnityEvent<Vector3> _onObjectMoved;

    protected float _temperatureRadius;
    protected float _maxDebuff;
    protected float _minDebuff;
    
    public void OnPlayerRankingChanged(UnityAction<Dictionary<string, float>> value)
    { 
        _onPlayerChanged.AddListener(value);
    }
    
    public UnityAction<Vector3> OnObjectMoved
    {
        set => _onObjectMoved.AddListener(value);
    }

    protected TemperatureHandlerBase()
    {
        _debuffs = new Dictionary<string, float>();
        _positions = new Dictionary<string, Vector3>();
        _distances = new Dictionary<string, float>();
        
        _onPlayerChanged ??= new PlayerChangeEvent();
        _onObjectMoved ??= new UnityEvent<Vector3>();
    }

}
