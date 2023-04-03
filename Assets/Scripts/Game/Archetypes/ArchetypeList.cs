using System;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game
{
    [Serializable]
    public struct ArchetypeItem
    {
        public Archetypes key;
        public Archetype value;
    }
    [CreateAssetMenu]
    public class ArchetypeList : ScriptableObject
    {
        [field: SerializeField]
        public List<ArchetypeItem> archetypeList { get; private set; }


    }

}