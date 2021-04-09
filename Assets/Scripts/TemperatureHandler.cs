using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Events;
using UnityEngine;
using UnityEngine.Events;

public class TemperatureHandler : TemperatureHandlerBase
{
    private void Start()
    {
        _temperatureRadius = 10;
        _maxDebuff = 5;
        _minDebuff = -5;
        
        FindObjectsOfType<Player>().ToList().ForEach(p =>
        {
            if (_debuffs.ContainsKey(p.Name)) { p.Name += _debuffs.Count; }
            p.GetComponent<PlayerTemperature>().OnPlayerPositionChanged(PositionChanged);
            _debuffs.Add(p.Name, 0);
        });
    }

    private void Update()
    {
        if (CalculateDistance() && CalculateDebuffs())
        {
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

        var debuff = _temperatureRadius;
        
        var changed = false;
        var rankedPositions = _distances.OrderBy(x => x.Value).ToList();
        
        rankedPositions.ForEach(e =>
        {
            if (!_debuffs[e.Key].Equals(debuff))
            {
                _debuffs[e.Key] = Mathf.Clamp(debuff - e.Value, _minDebuff, _maxDebuff);
                changed = true;
            }
            debuff -= 2;
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
