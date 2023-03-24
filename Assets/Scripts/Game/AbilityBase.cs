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

<<<<<<< HEAD
        public abstract void ActivateAbility(Vector3? mosPos =  null);
        public abstract void ApplyEffect();


=======
        public abstract void ActivateAbility(Vector3 mosPos);
        public abstract void ApplyEffect();
>>>>>>> c1b37fc (Refactor AutoAttack)

        public abstract void DrawAbilityIndicator(Vector3 targetLocation);
        protected abstract void Update();
    }
}
