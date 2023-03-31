using BossArena.game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game 
{
    public class DPSBasicAbility : TargetedAbilityBase, IDrawIndicator
    {
        //Referencing Tank class structure...

        // Need to have reference to Parent Player Prefab
        //[SerializeField]
        //private GameObject PlayerPrefab;

        // Blink Prefab??
        [SerializeField]
        private GameObject BlinkPrefab;
        private CircleCollider2D BlinkPrefabCollider;
        private SpriteRenderer BlinkPrefabSpriteRenderer;

        private bool basicActivated = false;
        private bool withinTauntRange = false;

        Vector3 currentMousePosition;

        public override void ActivateAbility(Vector3? mousepos = null)
        {
            BlinkPrefabSpriteRenderer.enabled = false;
            if (onCoolDown)
                return;
            onCoolDown = true;
            timeStart = Time.time;
            ApplyEffect();
            BlinkPrefabCollider.enabled = true;
            BlinkPrefabCollider.enabled = false;
        }

        public override void ApplyEffect()
        {
            // since this is blink, this may just be apply the change of position
        }



        // Start is called before the first frame update
        void Start()
        {
            timeStart = Time.time;

            // Get Main Camera
            mainCamera = Camera.main;

            // Get Collider
            BlinkPrefabCollider = GetComponent<CircleCollider2D>();

            // Get SpriteRenderer
            BlinkPrefabSpriteRenderer = GetComponent<SpriteRenderer>();

            // Initially Disable
            BlinkPrefabCollider.enabled = false;
            BlinkPrefabSpriteRenderer.enabled = false;

            //BlinkPrefab.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {
            // Every frame, check for cooldowns, set bool accordingly.
            checkCooldown();

        }


        protected Vector3 calculateBasicAbilityCursor()
        {
            //leaving this blank for now

        }
    }

}
