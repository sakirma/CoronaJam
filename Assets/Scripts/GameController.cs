using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    // playerlist
    
    // keep track of active players (connected controllers)
    private PlayerInputManager _piManager; 
    
    //  what player is connected to what controller, sometimes 2 players on one controller

    private GameState _state;
    
    // settings
    [SerializeField] private float _gameOverWaitTime = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case GameState.WAITING_FOR_START:
                if (_piManager.playerCount < _piManager.maxPlayerCount)
                {
                    // show text if not 4 players are in yet
                    UIManager.INSTANCE.SetWaitingForPlayers(true);
                    UIManager.INSTANCE.SetPressToStart(false);
                }
                else
                {
                    // show "press A to start" if 4 players are in
                    UIManager.INSTANCE.SetWaitingForPlayers(false);
                    UIManager.INSTANCE.SetPressToStart(true);
                    
                    // wait for input
                    
                }
                break;
            case GameState.SETTING_UP:
                UIManager.INSTANCE.SetWaitingForPlayers(false);
                UIManager.INSTANCE.SetPressToStart(false);
                
                // reset and initialize components
                break;
            case GameState.PLAYING:
                // keep track of dead players
                break;
            case GameState.GAME_WON:
                // countdown timer to new game
                StartCoroutine(GameWon());
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
        WAITING_FOR_START = 0,
        SETTING_UP,
        PLAYING,
        GAME_WON
    }
}
