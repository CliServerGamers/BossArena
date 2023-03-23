using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BossArena.game
{
    abstract class Enemy : EntityBase, IHostile
    {
        [SerializeField]
        public Player CurrentTarget { get; set; }
        [SerializeField]
        float threatRadius;

        protected override void Start()
        {
            base.Start();
            SetHealth(MaxHealth);
        }

        protected void getTarget() {
            var colliders = Physics2D.OverlapCircleAll(transform.position, threatRadius);
            foreach (var collider in colliders)
            {
                Debug.Log($"{collider.gameObject.name} is threat");
            }
        }
    }
}
