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

        Quaternion rot = new Quaternion();

        Vector3 currentMousePosition;

        // Start is called before the first frame update
        protected override void Start()
        {

            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            base.Start();
            //PlayerPrefab = transform.parent.gameObject;
            //// Get the Prefab holding the BoxCollider2D
            //AUTOATTACK_COLLIDER = AutoAttackPrefab.transform.parent.transform.GetChild(0).GetComponent<BoxCollider2D>();
            //AUTOATTACK_COLLIDER.enabled = false;

            //mainCamera = Camera.main

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

        public void Initialize()
        {
            //PlayerPrefab = transform.parent.gameObject;
            //// Get the Prefab holding the BoxCollider2D
            //AUTOATTACK_COLLIDER = PlayerPrefab.transform.GetChild(0).GetComponent<BoxCollider2D>();
            //AUTOATTACK_COLLIDER.enabled = false;
        }
        // Update is called once per frame
        protected override void Update()
        {
            // if (!IsOwner) return;

            checkCooldown();




            //DrawAbilityIndicator(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            //if (Input.GetMouseButtonDown(0))
            //{
            //Debug.Log("peepeepoopoo");
            //ActivateAbility(Input.mousePosition);

            //}
            //if (!IsOwner) return;

            //DrawAbilityIndicator(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            //if (Input.GetMouseButtonDown(0))
            //{
            //    //Debug.Log("peepeepoopoo");
            //    ActivateAbility(Input.mousePosition);

            //}

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
            //AutoAttackPrefabSpriteRenderer.enabled = false;
            anim.ResetTrigger("onAttack");
        }
        public override void ApplyEffect()
        {

            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            //Play AutoAttack Animation
            anim.SetTrigger("onAttack");
            //AUTOATTACK_COLLIDER.enabled = true;

            Debug.Log("Collider.enabled = " + AUTOATTACK_COLLIDER.enabled);
            //Delay for length of attack
            //AUTOATTACK_COLLIDER.enabled = false;
            StartCoroutine(WaitToDisableHitbox());
        }

        IEnumerator WaitToDisableHitbox()
        {
            yield return new WaitForSeconds(lengthOfAttackInSec);
            //AUTOATTACK_COLLIDER.enabled = false;
            Debug.Log("Collider.enabled = " + AUTOATTACK_COLLIDER.enabled);
        }

        public void DrawAbilityIndicator(Vector3 targetLocation)
        {
            //Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            Vector3 targetWorldLocation = mainCamera.ScreenToWorldPoint(targetLocation);
            // Get and Convert Mouse Position into World Coordinates
            currentMousePosition = new Vector3(targetWorldLocation.x, targetWorldLocation.y, 0f);
            // Calculate Focus Cursor
            Vector2 focusCursor = calculateFocusCursor();

            AutoAttackPrefab.transform.position = focusCursor;


            Vector3 diff = currentMousePosition - parentPlayer.transform.position;
            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            AutoAttackPrefab.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
            //UnityEngine.Debug.Log("COLLIDER: " + transform.position);
        }

        private void OnDrawGizmos()
        {
            Vector3 focusCursor = calculateFocusCursor();
            Gizmos.color = new Color(1, 1, 1, 0.25f);
            drawFocusCursor();

            /*        if (Input.GetMouseButtonDown(0))
                    {
                        drawFocusCursor();
                    }*/

            /*        Gizmos.DrawCube(new Vector3(3, 4, currentMousePosition.z), new Vector3(1, 1, 1)); */


            /*        Vector2 mousePosition = Input.mousePosition;
                    UnityEngine.Debug.Log(mousePosition);

                    Gizmos.color = new Color(1, 0, 0, 0.5f);
                    Gizmos.DrawCube(new Vector2(mousePosition.x, mousePosition.y), new Vector3(1, 1, 1));*/
        }

        protected Vector3 calculateFocusCursor()
        {
            /*Vector2 mousePosition = Input.mousePosition;
            UnityEngine.Debug.Log("MousePosition - X:" + mousePosition.x + "Y:" + mousePosition.y);*/
            //Vector3 playerPos = AutoAttackPrefab.transform.parent.position;

            Vector3 playerPos = parentPlayer.transform.position;


            float angle = Mathf.Atan2(this.currentMousePosition.y - playerPos.y, currentMousePosition.x - playerPos.x);

            //UnityEngine.Debug.Log("Transform Position: " + playerPos);

            float focusX = playerPos.x + (Mathf.Cos(angle) * range);
            float focusY = playerPos.y + (Mathf.Sin(angle) * range);

            Vector3 focusCursorPosition = new Vector3(focusX, focusY, 0f);
            //UnityEngine.Debug.Log(focusCursorPosition);

            return focusCursorPosition;


            /* Gizmos.color = new Color(1, 0, 0, 0.5f);
             Gizmos.DrawCube(new Vector2(mousePosition.x, mousePosition.y), new Vector3(1, 1, 1));*/
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
            if (IsServer)
            {
            }


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
                    Debug.Log("Smack Bad man");
                    monoBehaviour.GetComponent<EntityBase>().TakeDamageServerRpc(damage);
                }
            }
        }


    }
}