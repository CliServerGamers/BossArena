using Assets.Scripts.Game.BehaviorTree;
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
        [field: SerializeField]
        public float threatRadius { get; private set; }

        private Node _root = null;

        protected override void Start()
        {
            _root = SetupTree();
            base.Start();
            SetHealth(MaxHealth.Value);
        }

        // no object that extends entity should be able to override this method as it takes care of running the tree
        protected override sealed void Update()
        {
            if(!IsOwner) return;
            _root?.Evaluate();
        }

        protected abstract Node SetupTree();

        protected void getTarget() {
            var colliders = Physics2D.OverlapCircleAll(transform.position, threatRadius);
            foreach (var collider in colliders)
            {
                Debug.Log($"{collider.gameObject.name} is threat");
            }
        }

    }
}
