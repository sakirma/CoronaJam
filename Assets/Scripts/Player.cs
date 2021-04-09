using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private string _name = "Player";
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    private bool _alive;
    public bool Alive
    {
        get => _alive;
        set
        {
            _alive = value;
        }
    }

    public void StartGameInput(InputAction.CallbackContext context)
    {
        GameController.INSTANCE.StartGameInput();
    }

    private void Start()
    {
        GetComponent<PlayerTemperature>().OnPlayerDied(PlayerDied);
    }

    private void PlayerDied(string name)
    {
        Debug.Log(name + " Died!");
        _alive = false;
        Destroy(gameObject);
    }
}
