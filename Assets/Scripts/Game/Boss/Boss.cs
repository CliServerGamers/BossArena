using Assets.Scripts.Game.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BossArena.game
{

    public class Boss : Enemy
    {

        [SerializeField]
        private GameObject shadow;

        public static float speed = 5f;

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
            base.Start();
        }

        protected override Node SetupTree()
        {
            _root = new SequenceNode(new List<Node>
            {
                new JumpAttack(this, shadow)
            });

            return _root;
        }

    }
}
