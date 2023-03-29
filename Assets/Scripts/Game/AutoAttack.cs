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

        [SerializeField]
        private GameObject AutoAttackPrefab;

        // Parent Player Prefab MUST have AutoAttackCollider Prefab\
        [SerializeField]
        private BoxCollider2D AUTOATTACK_COLLIDER;


        Vector3 currentMousePosition;

        // Start is called before the first frame update
        protected override void Start()
        {
            Debug.Log($"{this.GetType().Name}: {System.Reflection.MethodBase.GetCurrentMethod().Name}");
            base.Start();
            //PlayerPrefab = transform.parent.gameObject;
            // Get the Prefab holding the BoxCollider2D
            AutoAttackPrefab = parentPlayer.transform.GetChild(0).gameObject;
            AUTOATTACK_COLLIDER = parentPlayer.transform.GetChild(0).GetComponent<BoxCollider2D>();
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
            //ApplyWindUp
            ApplyEffect();
            //ApplyCooldown

        }

        public override void ApplyEffect()
        {
            AUTOATTACK_COLLIDER.enabled = true;
            //Delay for length of attack
            AUTOATTACK_COLLIDER.enabled = false;
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
            Vector3 playerPos = parentPlayer.transform.position;

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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsServer)
            {
                HandleCollision(collision);
            }

            
        }

        protected void HandleCollision(Collision2D collision)
        {
            var tempMonoArray = collision.gameObject.GetComponents<MonoBehaviour>();
            foreach (var monoBehaviour in tempMonoArray)
            {
                if (monoBehaviour is IFriendly)
                {
                    Debug.Log("Hit friendly player");
                }
            }
        }
    }
}