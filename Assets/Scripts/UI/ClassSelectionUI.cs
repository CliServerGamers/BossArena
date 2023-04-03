using BossArena.UI;
using BossArena;
using UnityEngine;
using BossArena.game;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SocialPlatforms;

namespace BossArena.UI
{
    /// <summary>
    /// Button callbacks to set the currenct Archetype
    /// </summary>
    public class ClassSelectionUI : UIPanelBase
    {
        [SerializeField]
        TMP_Text m_ArchetypeText;
        [SerializeField]
        Button m_NextButton;
        [SerializeField]
        Button m_PreviousButton;
        LocalPlayer m_LocalPlayer;

        void Next()
        {
            Archetypes type = m_LocalPlayer.Archetype.Value.Next();
            m_LocalPlayer.Archetype.Value = type;
            Manager.SetLocalUserArchetype(type);
        }

        void Previous()
        {
            Archetypes type = m_LocalPlayer.Archetype.Value.Previous();
            m_LocalPlayer.Archetype.Value = type;
            Manager.SetLocalUserArchetype(type);
        }

        void ArchetypeChanged(Archetypes archetype)
        {
            m_ArchetypeText.text = archetype.ToString();
        }

        public void SetUser(LocalPlayer localPlayer)
        {
            m_LocalPlayer = localPlayer;
            SubscribeToPlayerUpdates();

            if (m_LocalPlayer.ID.Value != Manager.m_localUser.ID.Value) return;
            m_NextButton.GetComponent<Image>().enabled = true;
            m_PreviousButton.GetComponent<Image>().enabled = true;
        }

        public void ResetUI()
        {
            if (m_LocalPlayer == null)
                return;
            UnsubscribeToPlayerUpdates();
            m_LocalPlayer = null;
        }

        void SubscribeToPlayerUpdates()
        {
            m_LocalPlayer.Archetype.onChanged += ArchetypeChanged;
            m_LocalPlayer.UserStatus.onChanged += OnUserStateChanged;
        }

        void UnsubscribeToPlayerUpdates()
        {
            if (m_LocalPlayer == null)
                return;
            if (m_LocalPlayer.Archetype?.onChanged != null)
                m_LocalPlayer.Archetype.onChanged -= ArchetypeChanged;
        }

        public override void Start()
        {
            base.Start();
            m_NextButton.onClick.AddListener(Next);
            m_PreviousButton.onClick.AddListener(Previous);
            m_ArchetypeText.text = Manager.m_localUser.Archetype.Value.ToString();
        }

        void OnUserStateChanged(PlayerStatus state)
        {
            Debug.Log($"{m_LocalPlayer.ID.Value} : {Manager.m_localUser.ID.Value}");
            if (m_LocalPlayer.ID.Value != Manager.m_localUser.ID.Value) return;
            if (state == PlayerStatus.Ready)
            {
                Debug.Log("It's lobbying time");
                m_NextButton.GetComponent<Image>().enabled = false;
                m_PreviousButton.GetComponent<Image>().enabled = false;
            }
            if (state == PlayerStatus.Lobby)
            {
                Debug.Log("G A Y M E R");
                m_NextButton.GetComponent<Image>().enabled = true;
                m_PreviousButton.GetComponent<Image>().enabled = true;

            }
        }
    }
}