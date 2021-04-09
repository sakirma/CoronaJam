using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE;
    
    // playerlist
    private List<PlayerInput> _players = new List<PlayerInput>();
    
    // keep track of active players (connected controllers)
    private PlayerInputManager _piManager; 
    
    //  what player is connected to what controller, sometimes 2 players on one controller

    private GameState _state;
    
    // settings
    [SerializeField] private float _gameOverWaitTime = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        
        _state = GameState.WAITING_FOR_PLAYERS;

        _piManager = GetComponent<PlayerInputManager>();
        
        _piManager.onPlayerJoined += OnPlayerJoined;
        _piManager.onPlayerLeft += OnPlayerLeft;
    }

    private void OnPlayerJoined(PlayerInput pi)
    {
        _players.Add(pi);
        Debug.Log("Player joined, count: " + _players.Count);
        Debug.Assert(_piManager.playerCount == _players.Count);

        if (_players.Count() == _piManager.maxPlayerCount)
        {
            _state = GameState.WAITING_FOR_START;
        }
    }
    
    private void OnPlayerLeft(PlayerInput pi)
    {
        _players.Remove(pi);
        Debug.Log("Player left, count: " + _players.Count);
        Debug.Assert(_piManager.playerCount == _players.Count);
        
        if (_players.Count() < _piManager.maxPlayerCount)
        {
            _state = GameState.WAITING_FOR_START;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case GameState.WAITING_FOR_PLAYERS:
                // show text if not 4 players are in yet
                UIManager.INSTANCE.SetWaitingForPlayers(true);
                UIManager.INSTANCE.SetPressToStart(false);
                break;
            case GameState.WAITING_FOR_START:
                // show "press A to start" if 4 players are in
                UIManager.INSTANCE.SetWaitingForPlayers(false);
                UIManager.INSTANCE.SetPressToStart(true);
                    
                // wait for input

                break;
            case GameState.SETTING_UP:
                UIManager.INSTANCE.SetWaitingForPlayers(false);
                UIManager.INSTANCE.SetPressToStart(false);
                
                // reset and initialize components
                break;
            case GameState.PLAYING:
                // keep track of dead players
                foreach (PlayerInput pi in _players)
                {
                    //pi.GetComponent<PlayerMovement>()
                }

                break;
            case GameState.GAME_WON:
                // countdown timer to new game
                StartCoroutine(GameWon());
                
                _state = GameState.WAIT_FOR_NEXT_GAME;
                break;
            case GameState.WAIT_FOR_NEXT_GAME:

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator GameWon()
    {
        yield return new WaitForSeconds(_gameOverWaitTime);
        _state = GameState.SETTING_UP;
    }

    private enum GameState
    {
        WAITING_FOR_PLAYERS = 0,
        WAITING_FOR_START,
        SETTING_UP,
        PLAYING,
        GAME_WON,
        WAIT_FOR_NEXT_GAME
    }
}
