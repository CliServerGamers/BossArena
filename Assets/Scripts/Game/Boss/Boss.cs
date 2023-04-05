using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss.BossAbilities;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game
{

    class Boss : Enemy
    {
        [SerializeField]
        public Animator animator;

        [SerializeField]
        private GameObject skydiveHitbox;

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


            // Ignore collisions with the boss shadow
            Collider2D bossCollider = GetComponent<Collider2D>();
            Collider2D shadowCollider = shadow.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(bossCollider, shadowCollider, true);
        }

        protected override Node SetupTree()
        {
            // turn off the shadow initially
            shadow.GetComponent<SpriteRenderer>().enabled = false;

            List<Node> nodes = new List<Node>();
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.AddRange(GetSwirlProjectileSequence());
            nodes.AddRange(GetPassiveJumpsSequence());
            nodes.AddRange(GetSkyDiveSequence());
            nodes.AddRange(GetPassiveJumpsSequence());
            nodes.AddRange(GetProjectileSequence());
            nodes.AddRange(GetSkyDiveSequence());
            nodes.AddRange(GetSkyDiveSequence());
            nodes.AddRange(GetPassiveJumpsSequence());
            nodes.Add(new TeleportToRandomGoop(this.gameObject));

            Node _root = new InOrderSequenceNode(new List<Node>
            {
                new InOrderSequenceNode(nodes)
            });

            return _root;
        }

        private List<Node> GetProjectileSequence()
        {
            List<Node> sequence = new List<Node>();

            for (int i = 0; i < 16; i++)
            {
                sequence.Add(new IdleNode(this.gameObject, 0.5f));
                sequence.Add(new ProjectileAttackNode(this.gameObject, projectilePrefab, 8, 0, i * 22.5f));
            }

            return sequence;
        }

        private List<Node> GetSwirlProjectileSequence()
        {
            List<Node> sequence = new List<Node>();

            for (int i = 0; i < 10; i++)
            {
                sequence.Add(new IdleNode(this.gameObject, 0.5f));
                sequence.Add(new ProjectileAttackNode(this.gameObject, projectilePrefab, 8, i * 10 + 10, 0));
            }

            return sequence;
        }

        private List<Node> GetPassiveJumpsSequence()
        {
            List<Node> sequence = new List<Node>();
            sequence.Add(new IdleNode(this.gameObject, 0.5f));
            sequence.Add(new PassiveJump(this.gameObject, projectilePrefab));
            sequence.Add(new IdleNode(this.gameObject, 0.5f));
            sequence.Add(new PassiveJump(this.gameObject, projectilePrefab));
            sequence.Add(new IdleNode(this.gameObject, 0.5f));
            sequence.Add(new PassiveJump(this.gameObject, projectilePrefab));
            sequence.Add(new IdleNode(this.gameObject, 0.5f));
            sequence.Add(new PassiveJump(this.gameObject, projectilePrefab));
            sequence.Add(new IdleNode(this.gameObject, 0.5f));
            sequence.Add(new PassiveJump(this.gameObject, projectilePrefab));
            return sequence;
        }

        private List<Node> GetSkyDiveSequence()
        {
            List<Node> sequence = new List<Node>();
            sequence.Add(new IdleNode(this.gameObject, 1.0f));
            sequence.Add(new BossExitScreen(this.gameObject, shadow));
            sequence.Add(new SkyDive(this.gameObject, eod, shadow, skydiveHitbox));
            sequence.Add(new IdleNode(this.gameObject, 1.5f));
            return sequence;
        }

        protected override void HandleCollision(Collision2D collision)
        {
            return;
        }

    }
}
