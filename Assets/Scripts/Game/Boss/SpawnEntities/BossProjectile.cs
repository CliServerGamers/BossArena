using BossArena.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Boss
{
    class BossProjectile : TargetedAbilityBase
    {
        private float projectileSpeed = 5.0f;
        private float timeToLive = 5.0f;

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            return;
        }

        public override void ApplyEffect()
        {
            return;
        }

        protected override void Update()
        {
            timeToLive -= Time.deltaTime;
            if (timeToLive < 0)
            {
                Destroy(this.gameObject);
                return;
            }
            transform.position += transform.right * projectileSpeed * Time.deltaTime;
            //transform.Translate(Vector3.forward * Time.deltaTime * projectileSpeed);
        }
    }
}
