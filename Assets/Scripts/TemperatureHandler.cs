using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Events;
using UnityEngine;
using UnityEngine.Events;

public class TemperatureHandler : MonoBehaviour
{
    
    //Key: Player-id Value: Debuff object
    private Dictionary<string, float> _debuffs;
    private Dictionary<string, Vector3> _positions;
    private Dictionary<string, float> _distances;

    private PlayerChangeEvent _onPlayerChanged;
    private UnityEvent<Vector3> _onObjectMoved;
    
    public void OnPlayerRankingChanged(UnityAction<Dictionary<string, float>> value)
    { 
        _onPlayerChanged.AddListener(value);
    }
    
    public UnityAction<Vector3> OnObjectMoved
    {
        set => _onObjectMoved.AddListener(value);
    }

    public TemperatureHandler()
    {
        _debuffs = new Dictionary<string, float>();
        _positions = new Dictionary<string, Vector3>();
        _distances = new Dictionary<string, float>();
        
        _onPlayerChanged ??= new PlayerChangeEvent();
    }

    private void Start()
    {
        FindObjectsOfType<Player>().ToList().ForEach(p =>
        {
            if (_debuffs.ContainsKey(p.Name)) { p.Name += _debuffs.Count; }
            p.GetComponent<PlayerTemperature>().OnPlayerPositionChanged(PositionChanged);
            _debuffs.Add(p.Name, 0);
        });
    }

    void Update()
    {
        if (CalculateDistance() && CalculateDebuffs())
        {
            Debug.Log("Updating!");
            _onPlayerChanged.Invoke(_debuffs);
        }
    }
    
    private void PositionChanged(PositionData positionData)
    {
        if (!_positions.ContainsKey(positionData.PlayerName))
        {
            _positions.Add(positionData.PlayerName, positionData.Position);
            return;
        }
        _positions[positionData.PlayerName] = positionData.Position;
    }

    private bool CalculateDebuffs()
    {
        
        using var positionKeys = _positions.Keys.GetEnumerator();
        using var debuffKeys = _debuffs.Keys.GetEnumerator();

        float debuff = 0;
        
        var changed = false;
        var rankedPositions = _distances.OrderBy(x => x.Value).ToList();
        
        rankedPositions.ForEach(e =>
        {
            if (!_debuffs[e.Key].Equals(debuff))
            {
                _debuffs[e.Key] = debuff;
                changed = true;
            }
            debuff += 10;
        });
        
        return changed;
    }
    
    private bool CalculateDistance()
    { 
        using var keys = _positions.Keys.GetEnumerator();
        
        var changed = false;
        
        while (keys.MoveNext())
        {
            if(keys.Current == null) continue;
            
            var distance = Vector3.Distance(_positions[keys.Current], transform.position);
            
            if (!_distances.ContainsKey(keys.Current))
            {
                _distances.Add(keys.Current, distance);
                changed = true;
            }
            else if (Math.Abs(_distances[keys.Current] - distance) > 0.2f)
            {
                _distances[keys.Current] = distance;
                changed = true;
            }
        }
        return changed;
    }

}
