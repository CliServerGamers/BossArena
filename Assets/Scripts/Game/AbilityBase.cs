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
    public abstract class AbilityBase : NetworkBehaviour, IApplyEffect
    {
        [SerializeField]
        protected int windUpDelay;
        [SerializeField]
        protected int coolDownDelay;
        [SerializeField]
        protected float range;
        [SerializeField]
        public CallbackValue<bool> onCoolDown = new CallbackValue<bool>();
        [SerializeField]
        protected float timeStart;

        [SerializeField]
        protected float damage;

        protected virtual void Start()
        {
            onCoolDown.Value = false;
        }

        public abstract void ActivateAbility(Vector3? mosPos =  null);
        public abstract void ApplyEffect();


        protected abstract void Update();

        protected void checkCooldown()
        {
            if (Time.time - timeStart >= coolDownDelay)
            {
                // Enough time has passed, set ultimatedActivated as off.
                onCoolDown.Value = false;
            }
        }

        public float GetCoolDown()
        {
            return Time.time - timeStart;
        }
    }
}
