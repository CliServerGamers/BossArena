using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.Experimental.AI;

namespace BossArena.game
{
    abstract class AbilityBase : NetworkBehaviour, IApplyEffect
    {
        [SerializeField]
        protected int windUpDelay;
        [SerializeField]
        protected int coolDownDelay;
        [SerializeField]
        protected float range;
        [SerializeField]
        protected bool onCoolDown;
        protected void Start()
        {
            onCoolDown = false;
        }

        public abstract void ActivateAbility(Vector3? mosPos =  null);
        public abstract void ApplyEffect();


        protected abstract void Update();
    }
}
