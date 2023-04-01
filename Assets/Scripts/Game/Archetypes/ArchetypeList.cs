using UnityEngine;

namespace BossArena.game
{

    [CreateAssetMenu]
    public class ArchetypeList : ScriptableObject
    {
        [field: SerializeField]
        public Archetype[] archetypeList { get; private set; }

    }

}