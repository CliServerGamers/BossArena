using BossArena.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game
{
    class DPSUltimateAbility : TargetedAbilityBase, IRemoveEffect, IDrawIndicator
    {
        [SerializeField]
        private GameObject BlastPrefab;

        [SerializeField]
        private CircleCollider2D BlastPrefabCollider;

        private BoxCollider2D PlayerCollider;
        //private bool blastActivated = false;
        // Use for checking elapsed time while ulted.
        //private float timeStart;
        public GameObject playerObj;

        private SpriteRenderer spriteRenderer;

        private bool abilityEndabled;
        private bool isCooldownSet;
        private Player player;
        private Vector3? targetPos;
        private Vector3 currentMousePosition;

        private List<EntityBase> entitiesInRange = new List<EntityBase>();

        public void DrawAbilityIndicator(Vector3 targetLocation)
        {
            currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            currentMousePosition.z = 1f;
            transform.position = currentMousePosition;
            

            spriteRenderer.enabled = true;
        }

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            if (onCoolDown.Value) return;

            targetPos = currentMousePosition;

            if (abilityEndabled)
            {
                onCoolDown.Value = true;
                timeStart = Time.time;
                abilityEndabled = false;
                isCooldownSet = true;

                // Stop rendering the ability
                spriteRenderer.enabled = false;
                

                RemoveEffect();
            } else
            {
                abilityEndabled = true;

                // Start rendering the ability
                spriteRenderer.enabled = true;

                Transform blastPrefabColliderTransform = BlastPrefabCollider.transform;

                blastPrefabColliderTransform.position = currentMousePosition;
                ApplyEffect();
            }
        }

        public override void ApplyEffect()
        {
            UnityEngine.Debug.Log("DPS Ultimate Ability");
            player.SetState(EntityState.STUNNED);



            //PlayerCollider = BlastPrefab.transform.parent.transform.GetComponent<BoxCollider2D>();
        }

        public void RemoveEffect()
        {
            player.SetState(EntityState.DEFUALT);
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
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
            if (abilityEndabled)
            {

            }

            else if (isCooldownSet)
            {
                checkCooldown();
                isCooldownSet = false;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log("Collision with " + other.gameObject.name);
            var tempMonoArray = other.GetComponents<MonoBehaviour>();
            Debug.Log(tempMonoArray.Length);
            foreach (var monoBehaviour in tempMonoArray)
            {
                Debug.Log(monoBehaviour);
                if (monoBehaviour is IFriendly)
                {
                    ((IFriendly)monoBehaviour).HitFriendlyServerRpc(OwnerClientId);
                }
                if (monoBehaviour is IHostile)
                {
                    Debug.Log("Smack Bad man");
                    monoBehaviour.GetComponent<EntityBase>().TakeDamageServerRpc(damage);
                }
            }
        }

        public float calcElapsedTime()
        {
            return Time.time - timeStart;
        }
    }
}