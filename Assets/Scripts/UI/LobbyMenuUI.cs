using BossArena;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.UI
{

    public class LobbyMenuUI : UIPanelBase
    {
        public void LeaveLobby()
        {
            GameManager.Instance.UIChangeMenuState(GameState.Menu);
        }

        public async void StartGame()
        {
            await GameManager.Instance.StartGame();
        }
    }

}