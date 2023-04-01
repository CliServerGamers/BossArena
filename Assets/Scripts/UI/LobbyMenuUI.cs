using BossArena;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace BossArena.UI
{
    public class LobbyMenuUI : UIPanelBase
    {
        [SerializeField]
        Button m_StartGameButton;
        public void LeaveLobby()
        {
            GameManager.Instance.UIChangeMenuState(GameState.Menu);
        }

        public async void StartGame()
        {
            await GameManager.Instance.StartGame();
        }

        public override void Start()
        {
            Manager.LocalLobby.LocalLobbyState.onChanged += OnLobbyStateChanged;
        }

        //TODO: Set ready
        void OnLobbyStateChanged(LobbyState state)
        {
            if (state == LobbyState.Lobby)
            {
                m_StartGameButton.interactable = false;
            }
            if (state == LobbyState.AllReady)
            {
                if (NetworkManager.Singleton.IsHost)
                {
                    m_StartGameButton.interactable = true;
                }
            }
        }
    }

}