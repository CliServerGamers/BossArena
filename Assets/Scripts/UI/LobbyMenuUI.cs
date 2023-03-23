using BossArena;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenuUI : MonoBehaviour
{
    public void LeaveLobby()
    {
        GameManager.Instance.LeaveLobby();
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }
}
