using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BossArena.game
{
    public abstract class Archetype
    {
        [SerializeField]
        public AbilityBase BasicAttack { get; protected set; }
        [SerializeField]
        public AbilityBase BasicAbility { get; protected set; }
        [SerializeField]
        public AbilityBase UltimateAbility { get; protected set; }

        [SerializeField]
        public int MaxHealth { get; }
        public Archetype() { }
    }
}
