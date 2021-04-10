using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager INSTANCE;
    
    [SerializeField] private Text _waitingForPlayers;
    [SerializeField] private Text _pressToStart;
    [SerializeField] private Text _gamewon;
    [SerializeField] private Text _debug;

    private void Start()
    {
        INSTANCE = this;
    }

    public void SetWaitingForPlayers(bool value)
    {
        _waitingForPlayers.gameObject.SetActive(value);
    }

    public void SetPressToStart(bool value)
    {
        _pressToStart.gameObject.SetActive(value);
    }

    public void SetGameWon(bool value, string name = "")
    {
        _gamewon.text = "Game won by: " + name;
        _gamewon.gameObject.SetActive(value);
    }

    public void SetDebug(bool value, string state, string level, string mode)
    {
        _debug.text = "State: " + state + "\n" +
                        "Level: " + level + "\n" +
                        "Mode: " + mode;
        _debug.gameObject.SetActive(value);
    }
}
