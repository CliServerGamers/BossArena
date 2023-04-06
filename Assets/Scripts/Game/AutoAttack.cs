using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace BossArena.game
{
    class AutoAttack : TargetedAbilityBase, IDrawIndicator
    {
        // Need to have reference to Parent Player Prefab
        //[SerializeField]
        //private GameObject PlayerPrefab;

        // Need reference to AutoAttack Prefab
        [SerializeField]
        private GameObject AutoAttackPrefab;

        [SerializeField]
        private Animator anim;
        [SerializeField]
        private SpriteRenderer AutoAttackPrefabSpriteRenderer;

        // Parent Player Prefab MUST have AutoAttackCollider Prefab\
        [SerializeField]
        private BoxCollider2D AUTOATTACK_COLLIDER;

        // Use for checking elapsed time while ulted.
        //private bool autoActivated = false;
        [SerializeField]
        private float lengthOfAttackInSec;

        [SerializeField]
        private int AutoAttack_ThreatGen;

        Quaternion rot = new Quaternion();

        Vector3 currentMousePosition;

        // Start is called before the first frame update
        protected override void Start()
        {

            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            base.Start();

            // Get the Prefab holding the BoxCollider2D
            AutoAttackPrefab = gameObject;

            // Get SpriteRenderer
            AutoAttackPrefabSpriteRenderer = AutoAttackPrefab.GetComponent<SpriteRenderer>();

            // Set AutoAttackPrefab Scale
            //AutoAttackPrefab.transform.localScale = new Vector3(2f, 2f, 2f);

            // Intially Off
            //AutoAttackPrefabSpriteRenderer.enabled = false;

            //AUTOATTACK_COLLIDER = GetComponent<BoxCollider2D>();
            AUTOATTACK_COLLIDER.enabled = false;
        }

        // Update is called once per frame
        protected override void Update()
        {
            checkCooldown();
        }

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            UnityEngine.Debug.Log("Activate AutoAttack");

            //autoActivated = true;
            //AutoAttackPrefabSpriteRenderer.enabled = true;
            //ApplyWindUp
            ApplyEffect();
            //ApplyCooldown
            StartCoroutine(WaitForAbilityEnd());
        }


        IEnumerator WaitForAbilityEnd()
        {
            yield return new WaitForSeconds(.5f);
            anim.ResetTrigger("onAttack");
        }
        public override void ApplyEffect()
        {

            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            //Play AutoAttack Animation
            anim.SetTrigger("onAttack");

            Debug.Log("Collider.enabled = " + AUTOATTACK_COLLIDER.enabled);

            //Delay for length of attack
            //StartCoroutine(WaitToDisableHitbox());
        }

        //IEnumerator WaitToDisableHitbox()
        //{
        //    yield return new WaitForSeconds(lengthOfAttackInSec);
        //    Debug.Log("Collider.enabled = " + AUTOATTACK_COLLIDER.enabled);
        //}

        public void DrawAbilityIndicator(Vector3 targetLocation)
        {
            Vector3 targetWorldLocation = mainCamera.ScreenToWorldPoint(targetLocation);
            // Get and Convert Mouse Position into World Coordinates
            currentMousePosition = new Vector3(targetWorldLocation.x, targetWorldLocation.y, 0f);
            // Calculate Focus Cursor
            Vector2 focusCursor = calculateFocusCursor();

            AutoAttackPrefab.transform.position = focusCursor;


            Vector3 diff = currentMousePosition - parentPlayer.transform.position;
            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            AutoAttackPrefab.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        }

        private void OnDrawGizmos()
        {
            Vector3 focusCursor = calculateFocusCursor();
            Gizmos.color = new Color(1, 1, 1, 0.25f);
            drawFocusCursor();

        }

        protected Vector3 calculateFocusCursor()
        {

            Vector3 playerPos = parentPlayer.transform.position;

            float angle = Mathf.Atan2(this.currentMousePosition.y - playerPos.y, currentMousePosition.x - playerPos.x);

            float focusX = playerPos.x + (Mathf.Cos(angle) * range);
            float focusY = playerPos.y + (Mathf.Sin(angle) * range);

            Vector3 focusCursorPosition = new Vector3(focusX, focusY, 0f);

            return focusCursorPosition;
        }

        protected void drawFocusCursor()
        {
            //UnityEngine.Debug.Log("Auto Attacking");
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(calculateFocusCursor(), new Vector3(1, 1, 1));
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            HandleCollision(collider);
        }

        protected void HandleCollision(Collider2D collider)
        {
            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            if (!IsOwner) return;

            var tempMonoArray = collider.gameObject.GetComponents<MonoBehaviour>();
            foreach (var monoBehaviour in tempMonoArray)
            {
                if (monoBehaviour is IFriendly)
                {
                    ((IFriendly)monoBehaviour).HitFriendlyServerRpc(OwnerClientId);
                }
                if (monoBehaviour is IHostile)
                {
                    //Debug.Log("Smack Bad man");
                    monoBehaviour.GetComponent<EntityBase>().TakeDamageServerRpc(damage);
                    parentPlayer.GetComponent<EntityBase>().ThreatLevel.Value += AutoAttack_ThreatGen;
                }
            }
        }


    }
}