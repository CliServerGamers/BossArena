using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

namespace BossArena.game
{
    abstract class AbilityBase : NetworkBehaviour, IApplyEffect
    {
        [SerializeField]
        int windUpDelay;
        [SerializeField]
        int coolDownDelay;
        [SerializeField]
        private float range;
        [SerializeField]
        private bool onCoolDown;
        private void Start()
        {
            onCoolDown = false;
        }

        public virtual void ActivateAbility(Vector3 mosPos) { }
        public virtual void ApplyEffect() { }

        protected abstract void Update();
    }
}
