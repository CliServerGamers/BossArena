using System;
using TMPro;
using UnityEngine;

namespace BossArena.UI
{
    /// <summary>
    /// Watches a lobby or relay code for updates, displaying the current code to lobby members.
    /// </summary>
    public class DisplayCodeUI : UIPanelBase
    {
        public enum CodeType { Lobby = 0, Relay = 1 }

        [SerializeField]
        TMP_InputField m_outputText;
        [SerializeField]
        CodeType m_codeType;

        void LobbyCodeChanged(string newCode)
        {
            if (!string.IsNullOrEmpty(newCode))
            {
                m_outputText.text = newCode;
                Show();
            }
            else
            {
                Hide();
            }
        }

        public override void Start()
        {
            base.Start();
            m_outputText.readOnly = true;
            if (m_codeType == CodeType.Lobby)
            {
                Manager.LocalLobby.LobbyCode.onChanged += LobbyCodeChanged;
                m_outputText.text = Manager.LocalLobby.LobbyCode.Value;
            }
            if (m_codeType == CodeType.Relay)
            {
                Manager.LocalLobby.RelayCode.onChanged += LobbyCodeChanged;
                m_outputText.text = Manager.LocalLobby.RelayCode.Value;
            }
           
        }
    }
}
