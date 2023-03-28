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
        [SerializeField]
        float threatRadius;

        private Node _root = null;

        protected override void Start()
        {
            _root = SetupTree();
            base.Start();
            SetHealth(MaxHealth);
        }

        // no object that extends entity should be able to override this method as it takes care of running the tree
        protected override sealed void Update()
        {
            // flag for server or client
            // only do all the things on the server 

            // 1: checks health
            // 2: update the blackboard
            // 3: run the first attack in the queue, inside th

            // watch some tutorials on behaviour trees in unity for bosses
            // PAY ATTENTON to animation queues
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
