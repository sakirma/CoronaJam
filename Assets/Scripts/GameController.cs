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
    private List<Player> _players = new List<Player>();

    public List<Player> GetPlayers() { return _players;}
    
    // keep track of active players (connected controllers)
    private PlayerInputManager _piManager; 
    
    //  what player is connected to what controller, sometimes 2 players on one controller

    private GameState _state;
    
    // settings
    [SerializeField] private float _gameOverWaitTime = 5f;
    [SerializeField] private int _minPlayers = 2;
    
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
        _players.Add(pi.GetComponent<Player>());
        Debug.Log("Player joined, count: " + _players.Count);
        Debug.Assert(_piManager.playerCount == _players.Count);

        if (_players.Count() >= _minPlayers)
        {
            _state = GameState.WAITING_FOR_START;
        }
    }
    
    private void OnPlayerLeft(PlayerInput pi)
    {
        _players.Remove(pi.GetComponent<Player>());
        Debug.Log("Player left, count: " + _players.Count);
        Debug.Assert(_piManager.playerCount == _players.Count);
        
        if (_players.Count() < _minPlayers)
        {
            _state = GameState.WAITING_FOR_START;
        }
    }
    
    public void StartGameInput()
    {
        if (_state == GameState.WAITING_FOR_START)
        {
            Debug.Log("Starting game");
            _state = GameState.SETTING_UP;
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
                    
                // event waits for input to start
                break;
            case GameState.SETTING_UP:
                UIManager.INSTANCE.SetWaitingForPlayers(false);
                UIManager.INSTANCE.SetPressToStart(false);
                
                // reset and initialize components
                foreach (Player pi in _players)
                {
                    // reset health
                    // set position on map
                }

                _state = GameState.PLAYING;
                
                break;
            case GameState.PLAYING:
                // TODO: abstract this out to multiple gamemodes, possibly event for player dying

                int aliveCounter = 0;
                foreach (Player pi in _players)
                {
                    if (pi.Alive) aliveCounter++;
                }

                if (aliveCounter == 1)
                {
                    _state = GameState.GAME_WON;
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
        UIManager.INSTANCE.SetGameWon(true, _players.Find(x => x.Alive).Name);
        yield return new WaitForSeconds(_gameOverWaitTime);
        UIManager.INSTANCE.SetGameWon(false);
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
