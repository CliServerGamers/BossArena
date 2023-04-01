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

        void Next()
        {
            Debug.Log("Nexting");
            Archetypes type = Manager.m_localUser.Archetype.Value.Next();
            Manager.SetLocalUserArchetype(type);
        }

        void Previous()
        {
            Archetypes type = Manager.m_localUser.Archetype.Value.Previous();
            Manager.SetLocalUserArchetype(type);
        }

        void ArchetypeChanged(Archetypes archetype)
        {
            m_ArchetypeText.text = archetype.ToString();
        }

        public override void Start()
        {
            base.Start();
            m_NextButton.onClick.AddListener(Next);
            m_PreviousButton.onClick.AddListener(Previous);
            Manager.m_localUser.Archetype.onChanged += ArchetypeChanged;
            m_ArchetypeText.text = Manager.m_localUser.Archetype.Value.ToString();
        }
    }
}