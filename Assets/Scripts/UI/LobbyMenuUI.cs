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

    public async void StartGame()
    {
        await GameManager.Instance.StartGame();
    }
}
