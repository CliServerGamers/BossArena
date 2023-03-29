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
        public static int MAX_BOSS_HEALTH = 1000;

        public static float speed = 5f;

        [SerializeField]
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
            SetHealth(MAX_BOSS_HEALTH);
            _root.boss = this.gameObject;
        }

        protected override Node SetupTree()
        {
            return _root;

/*            _root = new SequenceNode(new List<Node>
            {
                new Node()
            });*/
        }

    }
}
