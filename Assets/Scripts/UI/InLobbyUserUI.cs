using BossArena.UI;
using BossArena;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BossArena.game;

namespace BossArena.UI
{
    /// <summary>
    /// When inside a lobby, this will show information about a player, whether local or remote.
    /// </summary>
    public class InLobbyUserUI : UIPanelBase
    {
        [SerializeField]
        TMP_Text m_DisplayNameText;

        [SerializeField]
        TMP_Text m_StatusText;

        [SerializeField]
        Image m_ArchetypeIcon;

        [SerializeField]
        Archetype[] m_Archetypes;

        [SerializeField]
        ClassSelectionUI classSelection;

        public bool IsAssigned => UserId != null;
        public string UserId { get; set; }
        public LocalPlayer m_LocalPlayer;
        public void SetUser(LocalPlayer localPlayer)
        {
            Show();
            m_LocalPlayer = localPlayer;
            UserId = localPlayer.ID.Value;
            //SetIsHost(localPlayer.IsHost.Value);
            SetArchetype(localPlayer.Archetype.Value);
            SetUserStatus(localPlayer.UserStatus.Value);
            SetDisplayName(m_LocalPlayer.DisplayName.Value);
            SubscribeToPlayerUpdates();

            classSelection.SetUser(m_LocalPlayer);
        }

        void SubscribeToPlayerUpdates()
        {
            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            m_LocalPlayer.DisplayName.onChanged += SetDisplayName;
            m_LocalPlayer.UserStatus.onChanged += SetUserStatus;
            m_LocalPlayer.Archetype.onChanged += SetArchetype;

            //m_LocalPlayer.IsHost.onChanged += SetIsHost;
        }

        void UnsubscribeToPlayerUpdates()
        {
            if (m_LocalPlayer == null)
                return;
            if (m_LocalPlayer.DisplayName?.onChanged != null)
                m_LocalPlayer.DisplayName.onChanged -= SetDisplayName;
            if (m_LocalPlayer.UserStatus?.onChanged != null)
                m_LocalPlayer.UserStatus.onChanged -= SetUserStatus;
            if (m_LocalPlayer.Archetype?.onChanged != null)
                m_LocalPlayer.Archetype.onChanged -= SetArchetype;
            //if (m_LocalPlayer.IsHost?.onChanged != null)
            //    m_LocalPlayer.IsHost.onChanged -= SetIsHost;
        }

        public void ResetUI()
        {
            if (m_LocalPlayer == null)
                return;
            UserId = null;
            //SetEmote(EmoteType.None);
            SetUserStatus(PlayerStatus.Lobby);
            Hide();
            UnsubscribeToPlayerUpdates();
            m_LocalPlayer = null;
        }

        void SetDisplayName(string displayName)
        {
            m_DisplayNameText.SetText(displayName);
        }

        void SetUserStatus(PlayerStatus statusText)
        {
            m_StatusText.SetText(SetStatusFancy(statusText));
        }

        void SetArchetype(Archetypes archetype)
        {
            ArchetypeIcon(archetype);
        }



        //void SetIsHost(bool isHost)
        //{
        //    m_HostIcon.enabled = isHost;
        //}

        /// <summary>
        /// Archetypes to Archetypes Color
        /// m_Archetypes[0] = Tank
        /// m_EmoteIcon[1] = Frown
        /// m_EmoteIcon[2] = UnAmused
        /// m_EmoteIcon[3] = Tongue
        /// </summary>
        void ArchetypeIcon(Archetypes type)
        {
            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: Changing to archetype ${type}");
            m_ArchetypeIcon.GetComponent<Image>().color = m_Archetypes[(int)type].classColor;
            //switch (type)
            //{
            //    case Archetypes.Tank:
            //        //return null;
            //        break;
            //    case Archetypes.Test:
            //        m_ArchetypeIcon.color = m_Archetypes[0].classColor;
            //        //return m_EmoteIcons[0];
            //        break;
            //    default:
            //        break;
            //}
        }

        string SetStatusFancy(PlayerStatus status)
        {
            switch (status)
            {
                case PlayerStatus.Lobby:
                    return "<color=#56B4E9>In Lobby</color>"; // Light Blue
                case PlayerStatus.Ready:
                    return "<color=#009E73>Ready</color>"; // Light Mint
                case PlayerStatus.Connecting:
                    return "<color=#F0E442>Connecting...</color>"; // Bright Yellow
                case PlayerStatus.InGame:
                    return "<color=#005500>In Game</color>"; // Green
                default:
                    return "";
            }
        }
    }
}