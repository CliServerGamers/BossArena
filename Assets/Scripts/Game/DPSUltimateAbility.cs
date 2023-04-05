using BossArena.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game
{
    class DPSUltimateAbility : TargetedAbilityBase, IRemoveEffect
    {
        [SerializeField]
        private GameObject BlastPrefab;

        private CircleCollider2D BlastPrefabCollider;

        private BoxCollider2D PlayerCollider;
        //private bool blastActivated = false;
        // Use for checking elapsed time while ulted.
        //private float timeStart;
        public GameObject playerObj;

        private SpriteRenderer spriteRenderer;

        private bool abilityEndabled;
        private Player player;
        private Vector3? targetPos;

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            if (onCoolDown.Value) return;

            targetPos = mosPos;

            if (abilityEndabled)
            {
                onCoolDown.Value = true;
                timeStart = Time.time;
                abilityEndabled = false;

                // Stop rendering the ability
                spriteRenderer.enabled = false;

                RemoveEffect();
            } else
            {
                abilityEndabled = true;

                // Start rendering the ability
                spriteRenderer.enabled = true;

                ApplyEffect();
            }
        }

        public override void ApplyEffect()
        {
            UnityEngine.Debug.Log("DPS Ultimate Ability");
            player.SetState(EntityState.STUNNED);

            BlastPrefabCollider.transform.GetComponent<CircleCollider2D>();

            //PlayerCollider = BlastPrefab.transform.parent.transform.GetComponent<BoxCollider2D>();
        }

        public void RemoveEffect()
        {
            player.SetState(EntityState.DEFUALT);
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            timeStart = Time.time;
            PlayerCollider = parentPlayer.transform.GetComponent<BoxCollider2D>();
            spriteRenderer = BlastPrefab.transform.GetComponent<SpriteRenderer>();
            player = parentPlayer.GetComponent<Player>();
            BlastPrefabCollider = GetComponent<CircleCollider2D>();

            // Initally false 
            spriteRenderer.enabled = false;

            //Set Ability State
            abilityEndabled = false;

            // Set scale of AbilityPrefab.
            BlastPrefab.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            // Negative Z-Axis Value, "Go closer towards Camera"
            BlastPrefab.transform.position = new Vector3(BlastPrefab.transform.position.x, BlastPrefab.transform.position.y, -2f);
            //mainCamera=Camera.main;
        }

        // Update is called once per frame
        protected override void Update()
        {
            checkCooldown();

        }

        public float calcElapsedTime()
        {
            return Time.time - timeStart;
        }
    }

}