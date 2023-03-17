using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UIMenuMain : MonoBehaviour
{

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void HostLobby()
    {
        Debug.Log("Hosting Lobby");
    }

    public void JoinLobby()
    {
        Debug.Log("Joinging Lobby");
    }
}
