using Assets.Scripts.Game.BehaviorTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace BossArena.game
{

    class Boss : Enemy
    {

        [SerializeField]
        private GameObject projectilePrefab;

        [SerializeField]
        private GameObject shadow;

        [SerializeField]
        private GameObject eod;

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
            shadow.gameObject.transform.position = new Vector3(shadow.transform.position.x, shadow.transform.position.y, 3);
            eod.gameObject.transform.position = new Vector3(eod.transform.position.x, eod.transform.position.y, -1);


            // Ignore collisions with the boss shadow
            Collider2D bossCollider = GetComponent<Collider2D>();
            Collider2D shadowCollider = shadow.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(bossCollider, shadowCollider, true);
        }

        protected override Node SetupTree()
        {
            Node _root = new InOrderSequenceNode(new List<Node>
            {
                new InOrderSequenceNode(new List<Node>
                {
                    new IdleNode(),
                    new BossExitScreen(this.gameObject, shadow),
                    new SkyDive(this.gameObject, eod, shadow),

                    new IdleNode(),
                    new PassiveJump(this.gameObject, shadow),
                    new IdleNode(),
                    new PassiveJump(this.gameObject, shadow),
                    new IdleNode(),
                    new PassiveJump(this.gameObject, shadow),
                    new IdleNode(),
                    new PassiveJump(this.gameObject, shadow),
                    new IdleNode(),
                    new PassiveJump(this.gameObject, shadow),

                    new ProjectileAttackNode(this.gameObject, projectilePrefab),
                })
            });

            return _root;
        }

        protected override void HandleCollision(Collision2D collision)
        {
            return;
        }
    }
}
