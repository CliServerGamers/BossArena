using BossArena.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game
{
    public class DPSUltimateAbility : TargetedAbilityBase
    {
        [SerializeField]
        private GameObject BlastPrefab;

        private BoxCollider2D PlayerCollider;
        //private bool blastActivated = false;
        // Use for checking elapsed time while ulted.
        //private float timeStart;

        private SpriteRenderer spriteRenderer;

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            //blastActivated = true;
            if (onCoolDown)
                return;
            onCoolDown = true;
            timeStart = Time.time;

            // Start rendering the ability
            spriteRenderer.enabled = true;

            ApplyEffect();
        }

        public override void ApplyEffect()
        {
            UnityEngine.Debug.Log("DPS Ultimate Ability");
            //PlayerCollider = BlastPrefab.transform.parent.transform.GetComponent<BoxCollider2D>();
            PlayerCollider.enabled = false;
            StartCoroutine(WaitForAbilityEnd());
        }

        IEnumerator WaitForAbilityEnd()
        {
            yield return new WaitForSeconds(5);
            PlayerCollider.enabled = true;

            // Stop drawing the Ultimate Ability
            spriteRenderer.enabled = false;
        }


        // Start is called before the first frame update
        void Start()
        {
            timeStart = Time.time;
            PlayerCollider = parentPlayer.transform.GetComponent<BoxCollider2D>();
            spriteRenderer = ultimatePrefab.transform.GetComponent<SpriteRenderer>();

            // Initally false 
            spriteRenderer.enabled = false;

            // Set scale of AbilityPrefab.
            BlastPrefab.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            // Negative Z-Axis Value, "Go closer towards Camera"
            BlastPrefab.transform.position = new Vector3(ultimatePrefab.transform.position.x, ultimatePrefab.transform.position.y, -2f);
            //mainCamera=Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            checkCooldown();

        }

        public float calcElapsedTime()
        {
            return Time.time - timeStart;
        }
    }

}