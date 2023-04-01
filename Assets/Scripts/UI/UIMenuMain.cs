using BossArena;
using BossArena.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;



public class UIMenuMain : UIPanelBase
{
    public bool isPrivate = true;
    [SerializeField]
    TMP_InputField m_inputText;

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
    public void ToJoinMenu()
    {
        Manager.UIChangeMenuState(GameState.JoinMenu);
    }
    public void HostLobby()
    {
        Debug.Log("Hosting Lobby");
        string lobbyName = "new lobby";
        int maxPlayers = 6;
        Manager.CreateLobby(lobbyName, isPrivate, maxPlayers);
        //GameManager.Instance.CreateLobby(lobbyName, , maxPlayers);
    }

    public void JoinLobby()
    {
        Debug.Log("Joinging Lobby");
        Manager.JoinLobby(null, m_inputText.text.ToUpper());
        //GameManager.Instance.JoinLobby();
    }

    public void SetPrivacy()
    {
        isPrivate = !isPrivate;
    }
}
