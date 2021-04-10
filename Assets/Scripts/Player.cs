using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Parameters")] [SerializeField]
    private float MAX_ON_FIRE_TIME = 5f;

    private float TEMP_FIRE_OFF_THRESHOLD = 60f;
    
    [Header("Technical")]
    [SerializeField] private GameObject _onFireParticle;
    
    [SerializeField] private string _name = "Player";
    [SerializeField] private bool _alive;
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

    private bool _onFire;
    private float _onFireTimer;
    public bool OnFire
    {
        get => _onFire;
        set
        {
            if (_onFire != value)
            {
                _onFire = value;
                _onFireParticle.SetActive(value);
                _onFireTimer = 0f;
            }
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

    private void Update()
    {
        if (OnFire)
        {
            _onFireTimer += Time.deltaTime;
            if (_onFireTimer > MAX_ON_FIRE_TIME)
            {
                GetComponent<PlayerTemperature>().KillPlayer();
            }
            
            if (!GetComponent<PlayerTemperature>().ClosestToCampfire())
            {
                OnFire = false;
            }
        }
    }

    public void OnGameStateChanged(GameState state)
    {
        var temperature = GetComponent<PlayerTemperature>();
        switch (state)
        {
            case GameState.SETTING_UP:
                temperature.GameStarted = true;
                temperature.ResetValues();
                break;
            case GameState.GAME_WON:
                temperature.ResetValues();
                temperature.GameStarted = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
    
    private void PlayerDied(string name)
    {
        Debug.Log(name + " Died!");
        _alive = false;
        OnFire = false;
        // set active messes with player controls
        //gameObject.SetActive(false);
    }
}
