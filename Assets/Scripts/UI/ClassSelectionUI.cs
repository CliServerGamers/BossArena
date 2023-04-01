using BossArena.UI;
using BossArena;
using UnityEngine;
using BossArena.game;
using TMPro;
using UnityEngine.UI;

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
    }
}