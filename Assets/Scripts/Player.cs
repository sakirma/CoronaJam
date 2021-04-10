using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private string _name = "Player";
    
    private bool _alive;
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    public bool Alive
    {
        get => _alive;
        set => _alive = value;
    }
    
    
    public void StartGameInput(InputAction.CallbackContext context)
    {
        GameController.INSTANCE.StartGameInput();
    }

    private void Start()
    {
        GetComponent<PlayerTemperature>().OnPlayerDied(PlayerDied);
    }

    public void DisableTemperature()
    {
        var temperature = GetComponent<PlayerTemperature>();
        temperature.GameStarted = false;
    }
    
    public void EnableTemperature()
    {
        var temperature = GetComponent<PlayerTemperature>();
        temperature.GameStarted = true;
        temperature.ResetValues();
    }
    
    private void PlayerDied(string name)
    {
        Debug.Log(name + " Died!");
        _alive = false;
        // set active messes with player controls
        //gameObject.SetActive(false);
    }
}
