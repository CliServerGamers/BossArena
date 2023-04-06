using UnityEngine;

namespace BossArena.UI
{
    /// <summary>
    /// Handles the menu for a player creating a new lobby.
    /// </summary>
    public class CreateMenuUI : UIPanelBase
    {
        public JoinCreateLobbyUI m_JoinCreateLobbyUI;
        string m_ServerName;
        bool m_IsServerPrivate;

        public override void Start()
        {
            base.Start();
            m_JoinCreateLobbyUI.m_OnTabChanged.AddListener(OnTabChanged);
        }

        void OnTabChanged(JoinCreateTabs tabState)
        {
            if (tabState == JoinCreateTabs.Create)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public void SetServerName(string serverName)
        {
            m_ServerName = serverName;
        }

        public void SetServerPrivate(bool priv)
        {
            m_IsServerPrivate = priv;
        }

        public void OnCreatePressed()
        {
            int maxPlayers = 6;
            Manager.CreateLobby(m_ServerName, m_IsServerPrivate, maxPlayers);
        }
    }
}