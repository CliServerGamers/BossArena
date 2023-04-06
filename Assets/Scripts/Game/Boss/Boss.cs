using Assets.Scripts.Game.BehaviorTree;
using System.Collections;
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
        public AudioSource audioSrc;
        [SerializeField]
        public SpriteRenderer rend;

        [SerializeField]
        public List<AudioClip> sounds;

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
            SetHealth(500);
            shadow.gameObject.transform.position = new Vector3(shadow.transform.position.x, shadow.transform.position.y, 3);
            SetHealth(BOSSMAXHEALTH);

            // Ignore collisions with the boss shadow
            //Collider2D bossCollider = GetComponent<Collider2D>();
            //Collider2D shadowCollider = shadow.GetComponent<Collider2D>();
            //Physics2D.IgnoreCollision(bossCollider, shadowCollider, true);
        }

        protected override Node SetupTree()
        {
            List<Node> nodes = new List<Node>();
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.Add(new IdleNode(this.gameObject, 1.0f));
            nodes.Add(new PassiveJump(this.gameObject, shadow));
            nodes.Add(new IdleNode(this.gameObject, 0.5f));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.Add(new PassiveJump(this.gameObject, shadow));
            nodes.Add(new IdleNode(this.gameObject, 0.5f));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.Add(new PassiveJump(this.gameObject, shadow));
            nodes.Add(new IdleNode(this.gameObject, 1.0f));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.AddRange(GetSwirlProjectileSequence(2));
            nodes.Add(new IdleNode(this.gameObject, 1.0f));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.Add(new PassiveJump(this.gameObject, shadow));
            nodes.AddRange(GetSkyDiveSequence());
            nodes.Add(new IdleNode(this.gameObject, 2.5f));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.Add(new PassiveJump(this.gameObject, shadow));
            nodes.Add(new IdleNode(this.gameObject, 0.5f));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.Add(new PassiveJump(this.gameObject, shadow));
            nodes.Add(new IdleNode(this.gameObject, 1.0f));
            nodes.Add(new TargetSelectionNode(this.gameObject));

            nodes.Add(new PassiveJump(this.gameObject, shadow));
            nodes.AddRange(GetSwirlProjectileSequence(1));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.Add(new PassiveJump(this.gameObject, shadow));
            nodes.AddRange(GetSwirlProjectileSequence(1));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.Add(new PassiveJump(this.gameObject, shadow));
            nodes.AddRange(GetSwirlProjectileSequence(1));
            nodes.Add(new TargetSelectionNode(this.gameObject));

            nodes.Add(new IdleNode(this.gameObject, 2.5f));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.AddRange(GetSkyDiveSequence());
            nodes.Add(new IdleNode(this.gameObject, 0.5f));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.AddRange(GetSkyDiveSequence());
            nodes.Add(new PassiveJump(this.gameObject, shadow));
            nodes.Add(new IdleNode(this.gameObject, 0.5f));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.Add(new PassiveJump(this.gameObject, shadow));
            nodes.Add(new IdleNode(this.gameObject, 0.5f));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.Add(new TeleportToRandomGoop(this.gameObject));
            nodes.Add(new IdleNode(this.gameObject, 0.5f));
            nodes.Add(new TargetSelectionNode(this.gameObject));
            nodes.AddRange(GetSwirlProjectileSequence(10));
            nodes.Add(new IdleNode(this.gameObject, 3.0f));

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

        private List<Node> GetSwirlProjectileSequence(int count)
        {
            List<Node> sequence = new List<Node>();

            for (int i = 0; i < count; i++)
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

        public void Teleport()
        {
            StartCoroutine(TeleportSequence());
            // teleport to random eod in the scene if any
            
        }

        IEnumerator TeleportSequence()
        {
            GameObject[] eods = GameObject.FindGameObjectsWithTag("EOD");
            if (eods != null && eods.Length > 0)
            {
                yield return StartCoroutine(FadeTo(0.0f, 0.5f));
                int randomIndex = UnityEngine.Random.Range(0, eods.Length);
                transform.position = eods[randomIndex].transform.position;
                yield return StartCoroutine(FadeTo(1.0f, 0.5f));
            }
        }

        IEnumerator FadeTo(float aValue, float aTime)
        {
            float alpha = transform.GetComponent<SpriteRenderer>().material.color.a;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
                transform.GetComponent<SpriteRenderer>().material.color = newColor;
                yield return null;
            }
        }


        protected override void HandleCollision(Collision2D collision)
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

        public void PlaySound(string clipName, float volume, float pitch, bool loop = false)
        {
            AudioClip clip = sounds.Find(sound => sound.name == clipName);
            if (clip != null)
            {
                audioSrc.clip = clip;
                audioSrc.volume = volume;
                audioSrc.pitch = pitch;
                audioSrc.Play();
                if (loop)
                {
                    audioSrc.loop = true;
                }
                return;
            }
            throw new KeyNotFoundException("Sound with the following name " + clipName + "Does not exist!");
        }

        public void StopClip()
        {
            audioSrc.Stop();
        }

    }
}
