using BossArena.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossArena.game 
{
    class DPSBasicAbility : TargetedAbilityBase, IDrawIndicator
    {
        //Referencing Tank class structure...

        // Need to have reference to Parent Player Prefab
        //[SerializeField]
        //private GameObject PlayerPrefab;

        // Blink Prefab??
        [SerializeField]
        private GameObject BlinkPrefab;
        [SerializeField]
        private CircleCollider2D BlinkPrefabCollider;
        private Rigidbody2D PlayerRigidBody;
        private SpriteRenderer BlinkPrefabSpriteRenderer;
        private ParticleSystem ps;
        private Vector3 pos;
        private Rigidbody2D rb;
        private float horizVelocity;
        private float vertVelocity;

        private bool basicActivated = false;
        private bool withinBlinkRange = false;

        Vector3 currentMousePosition;

        public override void ActivateAbility(Vector3? mousepos = null)
        {
            BlinkPrefabSpriteRenderer.enabled = false;
            if (onCoolDown.Value)
                return;
            onCoolDown.Value = true;
            timeStart = Time.time;
            ApplyEffect();
        }

        public override void ApplyEffect()
        {
            UnityEngine.Debug.Log("Blink ability");

            BlinkPrefabCollider.enabled = true;
            // since this is blink, this may just be apply the change of position
            var psemit = ps.emission;
            psemit.enabled = true;
            ps.Play();
            PlayerRigidBody = parentPlayer.GetComponent<Rigidbody2D>();
            PlayerRigidBody.position= BlinkPrefab.transform.position;
        }

        public void DrawAbilityIndicator(Vector3 targetLocation)
        {
            if (onCoolDown.Value) return;

            // Update MousePosition
            currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Check MousePosition distance from Player for TauntRange
            if (Vector2.Distance(currentMousePosition, parentPlayer.transform.position) < range)
            {
                withinBlinkRange = true;
            }
            else
            {
                withinBlinkRange = false;
            }

            Vector3 newPos = calculateBasicAbilityCursor();

            BlinkPrefab.transform.position = newPos;

            //TauntPrefabSpriteRenderer.enabled = true;
            BlinkPrefabSpriteRenderer.enabled = true;

            //TauntPrefab.SetActive(true);
        }

        protected Vector3 calculateBasicAbilityCursor()
        {
            Vector3 playerPos = parentPlayer.transform.position;

            Vector3 cursorPosition = currentMousePosition;
            cursorPosition.z = 1f;

            UnityEngine.Debug.Log("withinTauntRange: " + withinBlinkRange);

            // Mouse Cursor not in Ability Range
            if (!withinBlinkRange)
            {

                float angle = Mathf.Atan2(this.currentMousePosition.y - playerPos.y, currentMousePosition.x - playerPos.x);

                //UnityEngine.Debug.Log("Transform Position: " + playerPos);

                float focusX = playerPos.x + Mathf.Cos(angle) * range;
                float focusY = playerPos.y + Mathf.Sin(angle) * range;

                Vector3 focusCursorPosition = new Vector3(focusX, focusY, 0f);
                //UnityEngine.Debug.Log(focusCursorPosition);

                return focusCursorPosition;

            }
            return cursorPosition;

        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            timeStart = Time.time;

            rb = parentPlayer.GetComponent<Rigidbody2D>();

            // Get Main Camera
            mainCamera = Camera.main;

            // Get Collider
            BlinkPrefabCollider = GetComponent<CircleCollider2D>();

            // Get SpriteRenderer
            BlinkPrefabSpriteRenderer = GetComponent<SpriteRenderer>();

            // Initially Disable
            BlinkPrefabCollider.enabled = false;
            BlinkPrefabSpriteRenderer.enabled = false;

            //Particles!
            ps = parentPlayer.GetComponent<ParticleSystem>();



            //BlinkPrefab.SetActive(false);

        }

        // Update is called once per frame
        protected override void Update()
        {
            // Every frame, check for cooldowns, set bool accordingly.
            checkCooldown();

        }


    }

}
