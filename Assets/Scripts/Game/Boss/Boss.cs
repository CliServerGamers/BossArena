using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BossArena.game
{
    // selector node for root
    // leaf: passive movement
    // seq: check enemy in range -> task go to player
    class Boss : Enemy
    {

        private GameObject dummyPlayer = null;

        public static float range = 5f;

        private Node _root = null;

        protected override void FixedUpdate()
        {
            return;
        }

        protected override void LateUpdate()
        {
            return;
        }

        protected override void Start()
        {
            dummyPlayer = GameObject.Find("DummyPlayer");
            base.Start();
        }

        protected override Node SetupTree()
        {
            Node root = new SelectorNode(new List<Node>
            {
                new SequenceNode(new List<Node>
                {
                    new CheckPlayerInRange(transform),
                    new GoToPlayer(transform),
                }),
                new PassiveMoving(transform)
            });

            return root;
        }

    }
}
