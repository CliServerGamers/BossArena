using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BossArena.game
{
    class AutoAttack : TargetedAbilityBase
    {
        // Need to have reference to Parent Player Prefab
        //[SerializeField]
        //private GameObject PlayerPrefab;

        // Need reference to AutoAttack Prefab
        [SerializeField]
        private GameObject AutoAttackPrefab;

        // Parent Player Prefab MUST have AutoAttackCollider Prefab\
        private BoxCollider2D AUTOATTACK_COLLIDER;

        // Use for checking elapsed time while ulted.
        private float timeStart;
        private bool autoActivated = false;


        Vector3 currentMousePosition;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            //PlayerPrefab = transform.parent.gameObject;
            //// Get the Prefab holding the BoxCollider2D
            AUTOATTACK_COLLIDER = AutoAttackPrefab.transform.parent.transform.GetChild(0).GetComponent<BoxCollider2D>();
            AUTOATTACK_COLLIDER.enabled = false;

            mainCamera = Camera.main;

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

            DrawAbilityIndicator(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("peepeepoopoo");
                ActivateAbility(Input.mousePosition);

            }



        }

        public override void ActivateAbility(Vector3? mosPos = null)
        {
            autoActivated = true;
            //ApplyWindUp
            ApplyEffect();
            //ApplyCooldown

        }

        public override void ApplyEffect()
        {
            AUTOATTACK_COLLIDER.enabled = true;


            AUTOATTACK_COLLIDER.enabled = false;
        }

        public override void DrawAbilityIndicator(Vector3 targetLocation)
        {
            // Get and Convert Mouse Position into World Coordinates
            currentMousePosition = new Vector3(targetLocation.x, targetLocation.y, 0f);
            // Calculate Focus Cursor
            Vector2 focusCursor = calculateFocusCursor();

            transform.position = focusCursor;
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
            Vector3 playerPos = AutoAttackPrefab.transform.parent.position;

            float angle = Mathf.Atan2(this.currentMousePosition.y - playerPos.y, currentMousePosition.x - playerPos.x);

            //UnityEngine.Debug.Log("Transform Position: " + playerPos);

            float focusX = playerPos.x + Mathf.Cos(angle);
            float focusY = playerPos.y + Mathf.Sin(angle);

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

        public void checkCooldown()
        {
            if (Time.time - timeStart >= coolDownDelay)
            {
                // Enough time has passed, set ultimatedActivated as off.
                autoActivated = false;
            }
        }

    }
}