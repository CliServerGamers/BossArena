using BossArena;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;



public class UIMenuMain : MonoBehaviour
{
    public bool isPrivate = true;
    [SerializeField]
    TMP_InputField m_inputText;

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void HostLobby()
    {
        Debug.Log("Hosting Lobby");
        string lobbyName = "new lobby";
        int maxPlayers = 4;
        GameManager.Instance.CreateLobby(lobbyName, isPrivate, maxPlayers);
    }

    public void JoinLobby()
    {
        Debug.Log("Joinging Lobby");
        GameManager.Instance.JoinLobby(null, m_inputText.text.ToUpper());
    }

    public void SetPrivacy()
    {
        isPrivate = !isPrivate;
    }
}
