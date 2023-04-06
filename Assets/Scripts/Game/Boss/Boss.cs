using Assets.Scripts.Game.BehaviorTree;
using Assets.Scripts.Game.Boss.BossAbilities;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game
{

    class Boss : Enemy
    {
        [SerializeField]
        public float BOSSMAXHEALTH;

        [SerializeField]
        public Animator animator;
        [SerializeField]
        public AudioSource skydiveSFX;

        [SerializeField]
        private GameObject skydiveHitbox;
        [field: SerializeField]
        public float skydiveSmallHitBoxDamage { get; private set; }
        [field: SerializeField]
        public float skydiveLargeHitBoxDamage { get; private set; }

        [SerializeField]
        private GameObject projectilePrefab;

        [SerializeField]
        private GameObject shadow;

        [SerializeField]
        private GameObject eod;

        [field: SerializeField]
        public float jumpSpeed { get; private set; }
        //public static float speed = 5f;

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
            SetHealth(BOSSMAXHEALTH);

            // Ignore collisions with the boss shadow
            //Collider2D bossCollider = GetComponent<Collider2D>();
            //Collider2D shadowCollider = shadow.GetComponent<Collider2D>();
            //Physics2D.IgnoreCollision(bossCollider, shadowCollider, true);
        }

        protected override Node SetupTree()
        {
            //new TargetSelectionNode(this.gameObject)
            List<Node> nodes = GetSwirlProjectileSequence();
            nodes.Add(new TargetSelectionNode(this.gameObject));
            //nodes.AddRange(GetPassiveJumpsSequence());
            //nodes.AddRange(GetSkyDiveSequence());
            //nodes.AddRange(GetPassiveJumpsSequence());
            //nodes.AddRange(GetProjectileSequence());
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
            sequence.Add(new IdleNode(this.gameObject, 0.5f));
            sequence.Add(new ProjectileAttackNode(this.gameObject, projectilePrefab, 8, 0));
            sequence.Add(new IdleNode(this.gameObject, 0.5f));
            sequence.Add(new ProjectileAttackNode(this.gameObject, projectilePrefab, 8, 45));
            sequence.Add(new IdleNode(this.gameObject, 0.5f));
            sequence.Add(new ProjectileAttackNode(this.gameObject, projectilePrefab, 8, 0));
            return sequence;
        }

        private List<Node> GetSwirlProjectileSequence()
        {
            List<Node> sequence = new List<Node>();

            for (int i = 0; i < 10; i++)
            {
                sequence.Add(new IdleNode(this.gameObject, 0.5f));
                sequence.Add(new ProjectileAttackNode(this.gameObject, projectilePrefab, 8, i * 10 + 10));
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


        protected override void HandleTrigger(Collider2D collision)
        {
            this.GetComponent<Collider2D>().enabled = false;
            var tempMonoArray = collision.gameObject.GetComponents<MonoBehaviour>();
            foreach (var monoBehaviour in tempMonoArray)
            {
                if (monoBehaviour is Player)
                {
                    ((Player)monoBehaviour).TakeDamageClientRpc(DamageToPlayer);
                }
            }
            return;
        }

    }
}
