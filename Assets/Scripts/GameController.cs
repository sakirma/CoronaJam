using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // playerlist
    
    // keep track of active players (connected controllers)
    
    //  what player is connected to what controller, sometimes 2 players on one controller
    
    // gamestate
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private enum GameState
    {
        WAITING_FOR_START = 0,
        SETTING_UP,
        PLAYING,
        GAME_WON
    }
}
