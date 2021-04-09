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
}
