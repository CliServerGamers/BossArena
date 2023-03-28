using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BossArena.game
{
    [CreateAssetMenu]
    public class Archetype : ScriptableObject
    {
        [SerializeField]
        public AbilityBase BasicAttack;
        [SerializeField]
        public AbilityBase BasicAbility;
        [SerializeField]
        public AbilityBase UltimateAbility;

        [SerializeField]
        public int MaxHealth;
        public Archetype() { }
    }
}
