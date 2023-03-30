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
        public GameObject BasicAttack;
        [SerializeField]
        public GameObject BasicAbility;
        [SerializeField]
        public GameObject UltimateAbility;

        [SerializeField]
        public int MaxHealth;
        [SerializeField]
        public Color classColor;
        public Archetype() { }
    }
}
